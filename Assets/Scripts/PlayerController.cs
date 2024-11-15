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

    
    private void Update() {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<float>();    
    }

    public void OnJump(InputAction.CallbackContext context)
    {
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
    
    private void OnTriggerEnter(Collider other) {
        if (GameController.Instance.CurrentLevel.m_LevelEndCollider == other) {
            GameController.Instance.LevelComplete();
        }
    }
    
}