using System;
using UnityEngine;

public class EnemyController : LineOfSightObject {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    public CharacterController m_CharacterController;
    public float m_MoveSpeed;
    public float m_Gravity;
    public Color Color = Color.white;
    [SerializeField] private Transform m_SightOrigin;
    [SerializeField] private Animator enemyAnimator;
    public float PatrolDistanceLeft;
    public float PatrolDistanceRight;
    
    private Material rendererMaterial;
    private bool isCharacterDetected;

    private float patrolBase;
    
    private Vector3 direction;
    private float move;
    private bool patrolToLeft = true;

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
    }

    private void Update() {
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
        var isInLineOfSight = IsPointInLineOfSight(target);
        if (isInLineOfSight != isCharacterDetected) {
            isCharacterDetected = isInLineOfSight;
            enemyAnimator.SetTrigger("ShootTrigger");
            Debug.Log($"Character detected by {this}");
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
        var baseX = Application.isPlaying ? patrolBase : transform.position.x;
        if (PatrolDistanceLeft != 0f || PatrolDistanceRight != 0f) {
            Gizmos.DrawCube(new Vector3(baseX - PatrolDistanceLeft, transform.position.y + 1f, transform.position.z), new Vector3(0.01f, 2f, 1f));
            Gizmos.DrawCube(new Vector3(baseX + PatrolDistanceRight, transform.position.y + 1f, transform.position.z), new Vector3(0.01f, 2f, 1f));
        }
    }
    
}