using UnityEngine;

public class LightProjector : MonoBehaviour {

    public Light Spotlight;

    private Vector3 baseRotation;

    private void Awake() {
        baseRotation = Spotlight.transform.localRotation.eulerAngles;
    }

    public void SetData(LightProjectorControllerData data) {
        Spotlight.enabled = data.IsEnabled;
        Spotlight.transform.localRotation = Quaternion.Euler(new Vector3(baseRotation.x, baseRotation.y + data.CurrentAngleDelta, baseRotation.z));
        Spotlight.color = data.Color;
    }
    
}