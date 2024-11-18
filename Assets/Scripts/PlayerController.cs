using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour {

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public CharacterController m_CharacterController;
    public PlayableDirector m_PlayableDirector;
    public float m_MoveSpeed;
    public float m_Gravity;
    public float m_JumpHeight;
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpTimeLeniency = 0.1f;
    //[SerializeField] private float airFriction = 0.65f;
    private float timeToStopLeniency;
    private Vector3 direction;
    private float move;
    private bool canJump = false;

    private bool isControlsAvailable;

    private LightProjectorController currentLightProjectorController;

    private EnemyController currentEnemyToKill;
    
    [SerializeField] private Material m_CharacterPaintSharedMaterial;
    [SerializeField] private Material m_CharacterBandSharedMaterial;

    private Color _shirtColor = Color.black;

    private List<LightProjector> projectorsList = new List<LightProjector>();

    private MovingObject currentMovingObject;

    bool isLastFrameGrounded = false;

    public event Action onPlayerHitFloor;

    [SerializeField] ParticleSystem landingEffect;
    [SerializeField] ParticleSystem paintEffect;
     [SerializeField] PlayerAudioPlayer audioPlayer;

    void Start () {
        transform.forward = new Vector3(1, 0, 0);
    }

    private void FixedUpdate() {
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

        if (currentEnemyToKill != null && !currentEnemyToKill.deathTriggered) {
            animator.SetTrigger("Attack");
            GameController.Instance.KillEnemy(currentEnemyToKill);
        }
        else if (currentLightProjectorController != null) {
            GameController.Instance.ShowProjectorControllerDialog(currentLightProjectorController.Data);
        }
    }

    private void Move() {
        var isGrounded = m_CharacterController.isGrounded;
        isLastFrameGrounded = isGrounded;
        if (isGrounded) {
            timeToStopLeniency = Time.time + jumpTimeLeniency;
            direction = new Vector3(move, 0f, 0f) * m_MoveSpeed;
            if (canJump) {
                direction.y = m_JumpHeight;
            }
            else {
                animator.SetBool("IsJumping", false);
            }
        }
        else {
            direction.x = move /** airFriction*/ * m_MoveSpeed;
            if (canJump && Time.time < timeToStopLeniency) {
                direction.y = m_JumpHeight;
            }
        }

        var delta = Time.fixedDeltaTime;
        direction.y -= m_Gravity * delta;
        var motion = direction * delta;

        if (currentMovingObject != null && isGrounded) {
            motion.x += currentMovingObject.FixedDeltaX;
        }

        m_CharacterController.Move(motion);

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
        _shirtColor = color;
        m_CharacterPaintSharedMaterial.SetColor(BaseColor, color);
        m_CharacterPaintSharedMaterial.SetColor(EmissionColor, color * 0.8f);
    }
    
    public void SetBandColor(Color color) {
        m_CharacterBandSharedMaterial.SetColor(BaseColor, color);
        m_CharacterBandSharedMaterial.SetColor(EmissionColor, color);
    }

    public void ChangeProjector(LightProjector projector, bool value) {
        if (value) {
            projectorsList.Add(projector);
        }
        else {
            projectorsList.Remove(projector);
        }
        
        if (GetMixedColor(out var color)) {
            SetBandColor(color);
        }
        else {
            SetBandColor(Color.red);
        }
    }

    private bool GetMixedColor(out Color color) {
        color = Color.clear;
        if (projectorsList.Count == 0) {
            return false;
        }
        var colors = projectorsList.Select(p => p.GetColor()).ToArray();
        color = Utils.MixedColor(colors);
        return true;
    }

    public bool Visible {
        get {
            if (GetMixedColor(out var mixedColor)) {
                return !Utils.CloseColors(mixedColor, _shirtColor);
            }
            return false;
        }
    }

    public async void PlayStart() {
        projectorsList.Clear();
        gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        SetColor(Color.black);
        SetBandColor(Color.red);
        ChangeControllerEnabled(true);
        ChangeControlsAvailable(false);
        gameObject.SetActive(true);
        m_PlayableDirector.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        ChangeControlsAvailable(true);
    }
   public void Kill() {
        animator.SetTrigger("Death");
        ChangeControlsAvailable(false);
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
            paintEffect.startColor = paint.Color;
            paintEffect.Play();
            SetColor(paint.Color);
        }
        else if (layer == 17) { //enemy kill trigger
            if (currentLightProjectorController != null) {
                throw new Exception("Multiple enemy controllers is not supported");
            }
            var enemyController = other.transform.parent.GetComponent<EnemyController>();
            currentEnemyToKill = enemyController;
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
        else if (layer == 17) { //enemy kill trigger
            var enemyController = other.transform.parent.GetComponent<EnemyController>();
            if (currentEnemyToKill == enemyController) {
                currentEnemyToKill = null;
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        var layer = hit.collider.gameObject.layer;
        if (layer == 16) { //moving object
            var movingObject = hit.collider.gameObject.GetComponent<MovingObject>();
            currentMovingObject = movingObject;
        }
        else {
            currentMovingObject = null;
        }
        if(hit.gameObject.CompareTag("Floor") && !isLastFrameGrounded)
        {
            landingEffect.Play();
            audioPlayer.PlayJumpLand();
        }
    }
}
