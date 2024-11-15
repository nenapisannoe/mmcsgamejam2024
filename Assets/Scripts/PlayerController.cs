using System;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public CharacterController m_CharacterController;
    public float m_MoveSpeed;
    public float m_Gravity;
    public float m_JumpHeight;
    private Vector3 direction;
    private float move;
    private bool jump = false;

    private bool isControlsAvailable;
    
    private ProjectorController currentProjectorController;
    
    private void Update() {
        if (!m_CharacterController.enabled) {
            return;
        }
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isControlsAvailable) {
            return;
        }
        move = context.ReadValue<float>();    
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isControlsAvailable) {
            return;
        }
        jump = true;
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (!isControlsAvailable) {
            return;
        }

        if (currentProjectorController != null) {
            GameController.Instance.ShowProjectorControllerDialog(currentProjectorController.Data);
        }
    }

    private void Move()
    {
        if (m_CharacterController.isGrounded) {
            direction = new Vector3(move, 0f, 0f) * m_MoveSpeed;
            if (jump)
            {
                direction.y = m_JumpHeight;
            }
        }

        var delta = Time.deltaTime;
        direction.y -= m_Gravity * delta;
        m_CharacterController.Move(direction * delta);
        jump = false;
    }

    public void ChangeControlsAvailable(bool value) {
        jump = false;
        move = 0f;
        isControlsAvailable = value;
    }

    public void ChangeControllerEnabled(bool value) {
        jump = false;
        move = 0f;
        m_CharacterController.enabled = value;
    }
    
    private void OnTriggerEnter(Collider other) {
        var layer = other.gameObject.layer;
        if (layer == 10) { //killbox
            //лочим контроллер юнити, иначе будет падать в бесконечность, что не круто
            ChangeControllerEnabled(false);
            GameController.Instance.LevelFailed();
        }
        else if (layer == 11) { //level complete
            GameController.Instance.LevelComplete();
        }
        else if (layer == 12) { //projector control
            if (currentProjectorController != null) {
                throw new Exception("Multiple projector controllers is not supported");
            }
            var projectorControl = other.GetComponent<ProjectorController>();
            projectorControl.PlayerCanInteract(true);
            currentProjectorController = projectorControl;
        }
    }

    private void OnTriggerExit(Collider other) {
        var layer = other.gameObject.layer;
        if (layer == 12) { //projector control
            var projectorControl = other.GetComponent<ProjectorController>();
            projectorControl.PlayerCanInteract(false);
            if (currentProjectorController == projectorControl) {
                currentProjectorController = null;
            }
        }
    }

}