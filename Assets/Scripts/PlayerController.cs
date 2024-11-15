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
        isControlsAvailable = value;
    }

    public void ChangeControllerEnabled(bool value) {
        jump = false;
        move = 0f;
        m_CharacterController.enabled = value;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (GameController.Instance.Killbox == other) {
            //лочим контроллер юнити, иначе будет падать в бесконечность, что не круто
            ChangeControllerEnabled(false);
            GameController.Instance.LevelFailed();
        }
        else if (GameController.Instance.CurrentLevel.m_LevelEndCollider == other) {
            GameController.Instance.LevelComplete();
        }
    }
    
}