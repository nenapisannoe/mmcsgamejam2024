using UnityEngine;

public class LightProjector : LineOfSightObject {

    public Light Spotlight;

    private Vector3 baseRotation;
    private bool isCharacterDetected;

    private void Awake() {
        baseRotation = Spotlight.transform.localRotation.eulerAngles;
    }

    public void SetData(LightProjectorControllerData data) {
        Spotlight.enabled = data.IsEnabled;
        Spotlight.transform.localRotation = Quaternion.Euler(new Vector3(baseRotation.x, data.CurrentAngleDelta, baseRotation.z));
        Spotlight.color = data.Color;
    }

    private void LateUpdate() {
        var origin = GetOrigin();
        var player = GameController.Instance.PlayerController;
        var boxCollider = player.GetComponentInChildren<BoxCollider>();
        var target = boxCollider.ClosestPoint(origin);
        var isInLineOfSight = IsPointInLineOfSight(target);
        if (isInLineOfSight != isCharacterDetected) {
            isCharacterDetected = isInLineOfSight;
            GameController.Instance.PlayerController.ChangeProjector(this, isCharacterDetected);
        }
    }

    protected override Vector3 GetOrigin() {
        return Spotlight.transform.position;
    }

    protected override Vector3 GetDirection() {
        return Spotlight.transform.forward;
    }

    protected override float GetLength() {
        return Spotlight.range;
    }

    protected override float GetAngle() {
        return Spotlight.innerSpotAngle / 2f;
    }

    public override Color GetColor() {
        return Spotlight.color;
    }

}
