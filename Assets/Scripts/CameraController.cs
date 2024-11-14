using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera m_Camera;
    public Transform m_Target;

    private void LateUpdate() {
        m_Camera.transform.position = new Vector3(m_Target.position.x, m_Camera.transform.position.y, m_Camera.transform.position.z);
    }
}