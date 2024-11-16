using System;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public CharacterController m_CharacterController;
    public float m_MoveSpeed;
    public float m_Gravity;
    public float m_JumpHeight;
    [SerializeField] private Animator animator;
    private Vector3 direction;
    private float move;
    private bool canJump = false;

    private bool isControlsAvailable;
    
    private LightProjectorController currentLightProjectorController;
    
    [SerializeField] private Renderer m_AnyCharacterFrameRenderer;
    private Material m_CharacterPaintSharedMaterial;

    void Start () {
        transform.forward = new Vector3(1, 0, 0);
        if (m_AnyCharacterFrameRenderer != null) {
            m_CharacterPaintSharedMaterial = m_AnyCharacterFrameRenderer.sharedMaterial;
        }
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

    public void OnInteract(InputAction.CallbackContext context) {
        if (!isControlsAvailable) {
            return;
        }

        if (currentLightProjectorController != null) {
            GameController.Instance.ShowProjectorControllerDialog(currentLightProjectorController.Data);
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
        canJump = false;
        move = 0f;
        isControlsAvailable = value;
    }

    public void ChangeControllerEnabled(bool value) {
        canJump = false;
        move = 0f;
        m_CharacterController.enabled = value;
    }

    public void SetColor(Color color) {
        if (m_CharacterPaintSharedMaterial) {
            m_CharacterPaintSharedMaterial.SetColor(BaseColor, color);
        }
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
            if (currentLightProjectorController != null) {
                throw new Exception("Multiple projector controllers is not supported");
            }
            var projectorControl = other.GetComponent<LightProjectorController>();
            projectorControl.PlayerCanInteract(true);
            currentLightProjectorController = projectorControl;
        }
        else if (layer == 13) { //paint
            var paint = other.GetComponent<Paint>();
            SetColor(paint.Color);
        }
    }

    private void OnTriggerExit(Collider other) {
        var layer = other.gameObject.layer;
        if (layer == 12) { //projector control
            var projectorControl = other.GetComponent<LightProjectorController>();
            projectorControl.PlayerCanInteract(false);
            if (currentLightProjectorController == projectorControl) {
                currentLightProjectorController = null;
            }
        }
    }

}