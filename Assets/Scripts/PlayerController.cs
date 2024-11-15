using System;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public CharacterController m_CharacterController;
    public float m_MoveSpeed;
    public float m_Gravity;
    public float m_JumpHeight;
    [SerializeField] private Animator animator;
    private Vector3 direction;
    private float move;
    private bool canJump = false;

    private bool isControlsAvailable;

    void Start ()
    {
        transform.forward = new Vector3(1, 0, 0);
    }
    
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

        if (context.performed && move !=0) {
            transform.forward = new Vector3(move, 0, 0);
            animator.SetBool("IsMoving", true);
        }
        else if (move == 0)
        {
            animator.SetBool("IsMoving", false);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isControlsAvailable) {
            return;
        }
        if (context.performed)
        {
            animator.SetBool("IsJumping", true);
            canJump = true;
        }
    }

    private void Move()
    {
        if (m_CharacterController.isGrounded) {
            direction = new Vector3(move, 0f, 0f) * m_MoveSpeed;
            if (canJump)
                direction.y = m_JumpHeight;
            else
                animator.SetBool("IsJumping", false);
        }

        var delta = Time.deltaTime;
        direction.y -= m_Gravity * delta;
        m_CharacterController.Move(direction * delta);

        canJump = false;
    }

    public void ChangeControlsAvailable(bool value) {
        isControlsAvailable = value;
    }

    public void ChangeControllerEnabled(bool value) {
        canJump = false;
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