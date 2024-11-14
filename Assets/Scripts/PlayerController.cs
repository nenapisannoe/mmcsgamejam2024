using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController m_CharacterController;
    public float m_MoveSpeed;
    public float m_Gravity;
    public float m_JumpHeight;

    private Vector3 direction;

    private void Update() {
        if (m_CharacterController.isGrounded) {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f) * m_MoveSpeed;
            if (Input.GetAxis("Vertical") > 0f) {
                direction.y += m_JumpHeight;
            }
        }

        var delta = Time.deltaTime;
        direction.y -= m_Gravity * delta;
        m_CharacterController.Move(direction * delta);
    }
    
}