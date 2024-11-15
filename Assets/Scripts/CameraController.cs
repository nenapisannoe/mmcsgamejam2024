using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera m_Camera;
    private Transform cameraTarget;

    private Vector3 basePosition;
    private Quaternion baseRotation;
    
    private void Awake() {
        basePosition = m_Camera.transform.position;
        baseRotation = m_Camera.transform.rotation;
    }

    public void AssignTarget(Transform target) {
        cameraTarget = target;
    }

    public void Reset() {
        cameraTarget = null;
        m_Camera.transform.position = basePosition;
        m_Camera.transform.rotation = baseRotation;
    }

    private void LateUpdate() {
        if (cameraTarget == null) {
            return;
        }
        m_Camera.transform.position = new Vector3(cameraTarget.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z);
    }
    
}