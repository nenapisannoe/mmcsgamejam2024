using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : LineOfSightObject {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    public CharacterController m_CharacterController;
    public float m_MoveSpeed;
    public float m_Gravity;
    public Color Color = Color.white;
    [SerializeField] private Transform m_SightOrigin;
    [SerializeField] private Transform m_AttackOrigin;
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private EnemyAnimationHandler animationHandler;
    [SerializeField] private ParticleSystem deathParticle;
    public float PatrolDistanceLeft;
    public float PatrolDistanceRight;
    
    private Material rendererMaterial;
    
    private bool isCharacterDetected;

    private float patrolBase;
    
    private Vector3 direction;
    private float move;
    private bool patrolToLeft = true;

    private bool attackTriggered;
    [NonSerialized]
    public bool deathTriggered;

    private void Awake() {
        if (gameObject.layer != 15) {
            throw new Exception("Enemy object must be on layer 15");
        }

        var rendererComponent = GetComponent<Renderer>();
        if (rendererComponent != null) {
            rendererMaterial = rendererComponent.material;
        }
        UpdateMaterial();

        patrolBase = transform.position.x;
        
        animationHandler.OnShotTrigger += AnimationHandlerOnShotTrigger;
    }

    private async void AnimationHandlerOnShotTrigger() {
        if (isCharacterDetected) {
            GameController.Instance.ShotCharacter(m_AttackOrigin, Color);
        }
        else {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            attackTriggered = false;
        }
    }

    public async UniTask Death() {
        deathTriggered = true;
        //TODO: enemy death animation & await Task.Delay(TimeSpan.FromSeconds(1f));
        deathParticle.Play();
        await transform.DOScaleY(0f, 0.5f);
    }

    private void FixedUpdate() {
        if (attackTriggered || deathTriggered) {
            return;
        }
        if (PatrolDistanceLeft != 0f || PatrolDistanceRight != 0f) {
            var currentX = transform.position.x;
            if (patrolToLeft) {
                if (currentX < patrolBase - PatrolDistanceLeft) {
                    patrolToLeft = false;
                    move = 0f;
                }
                else {
                    transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                    move = -1f;
                    enemyAnimator.SetBool("IsMoving", true);
                }
            }
            else {
                if (currentX > patrolBase + PatrolDistanceRight) {
                    patrolToLeft = true;
                    move = 0f;
                }
                else {
                    transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                    move = 1f;
                    enemyAnimator.SetBool("IsMoving", true);
                }
            }
        }
        
        Move();
    }

    private void OnValidate() {
        PatrolDistanceLeft = Math.Max(0f, PatrolDistanceLeft);
        PatrolDistanceRight = Math.Max(0f, PatrolDistanceRight);
    }

    private void Move() {
        if (m_CharacterController.isGrounded) {
            direction = new Vector3(move, 0f, 0f) * m_MoveSpeed;
        }

        var delta = Time.deltaTime;
        direction.y -= m_Gravity * delta;
        m_CharacterController.Move(direction * delta);
    }

    private void LateUpdate() {
        var origin = GetOrigin();
        var player = GameController.Instance.PlayerController;
        var boxCollider = player.GetComponentInChildren<BoxCollider>();
        var target = boxCollider.ClosestPoint(origin);
        var isInLineOfSight = IsPointInLineOfSight(target, attackTriggered);
        if (isInLineOfSight != isCharacterDetected) {
            isCharacterDetected = isInLineOfSight;
            if (isCharacterDetected && !attackTriggered) {
                attackTriggered = true;
                enemyAnimator.SetTrigger("ShootTrigger");
            }
        }
    }

    private void UpdateMaterial() {
        if (rendererMaterial != null) {
            rendererMaterial.SetColor(BaseColor, Color);
        }
    }
    
    protected override Vector3 GetOrigin() {
        return m_SightOrigin.position;
    }

    protected override Vector3 GetDirection() {
        return -transform.right;
    }

    protected override float GetLength() {
        return 5f;
    }

    protected override float GetAngle() {
        return 30f;
    }

    protected override Color GetColor() {
        return Color;
    }
    
    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, 2f, 0f), 0.1f);
        if (PatrolDistanceLeft != 0f || PatrolDistanceRight != 0f) {
            var baseX = Application.isPlaying ? patrolBase : transform.position.x;
            Gizmos.DrawCube(new Vector3(baseX - PatrolDistanceLeft, transform.position.y + 1f, transform.position.z), new Vector3(0.01f, 2f, 1f));
            Gizmos.DrawCube(new Vector3(baseX + PatrolDistanceRight, transform.position.y + 1f, transform.position.z), new Vector3(0.01f, 2f, 1f));
        }
    }
    
}