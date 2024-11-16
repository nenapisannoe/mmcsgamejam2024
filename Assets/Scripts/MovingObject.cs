using System;
using UnityEngine;

public class MovingObject : MonoBehaviour {

    public Rigidbody Rigidbody;
    public float DistanceLeft;
    public float DistanceRight;
    public float Speed;
    private float BaseX;
    private bool MovingToLeft;

    [NonSerialized]
    public float FixedDeltaX;
    
    private void Awake() {
        if (gameObject.layer != 16) {
            throw new Exception("Moving object must be on layer 16");
        }
        
        BaseX = transform.position.x;
    }
    
    private void FixedUpdate() {
        var currentX = transform.position.x;
        var move = 0f;
        if (MovingToLeft) {
            if (currentX < BaseX - DistanceLeft) {
                MovingToLeft = false;
            }
            else {
                move = -1f;
            }
        }
        else {
            if (currentX > BaseX + DistanceRight) {
                MovingToLeft = true;
            }
            else {
                move = 1f;
            }
        }
        FixedDeltaX = move * Speed * Time.fixedDeltaTime;
        Rigidbody.MovePosition(new Vector3(transform.position.x + FixedDeltaX, transform.position.y, transform.position.z));
    }
    
    private void OnValidate() {
        DistanceLeft = Math.Max(0f, DistanceLeft);
        DistanceRight = Math.Max(0f, DistanceRight);
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        if (DistanceLeft != 0f || DistanceRight != 0f) {
            var baseX = Application.isPlaying ? BaseX : transform.position.x;
            Gizmos.DrawCube(new Vector3(baseX - DistanceLeft, transform.position.y + 1f, transform.position.z), new Vector3(0.01f, 2f, 1f));
            Gizmos.DrawCube(new Vector3(baseX + DistanceRight, transform.position.y + 1f, transform.position.z), new Vector3(0.01f, 2f, 1f));
        }
    }
    
}