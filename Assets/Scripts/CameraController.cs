using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public CinemachineConfiner3D CameraConfiner;

    public void Assign(BoxCollider cameraConfiner) {
        CameraConfiner.BoundingVolume = cameraConfiner;
    }

    public void Reset() {
        CameraConfiner.BoundingVolume = null;
    }
    
}