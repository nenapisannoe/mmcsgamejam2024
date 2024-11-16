using System;
using UnityEngine;

[Serializable]
public class LightProjectorControllerData {

    [Header("Enabled/Disabled")]
    public bool IsEnabled;
    public bool PlayerCanModifyEnabled;
    
    [Header("Delta Angle from base")]
    public int CurrentAngleDelta;
    public int MinAngleDelta;
    public int MaxAngleDelta;
    public bool PlayerCanModifyAngle;

    [Header("Color")]
    public Color Color = Color.white;
    public bool PlayerCanModifyRed;
    public bool PlayerCanModifyGreen;
    public bool PlayerCanModifyBlue;
    
    public event Action ProjectorDataChanged;

    public void ProjectorDataChangedEvent() {
        ProjectorDataChanged?.Invoke();
    }

}

public class LightProjectorController : MonoBehaviour {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public LightProjector LightProjector;
    
    private Material rendererMaterial;

    public LightProjectorControllerData Data = new LightProjectorControllerData();

    private void Awake() {
        var rendererComponent = GetComponent<Renderer>();
        if (rendererComponent != null) {
            rendererMaterial = rendererComponent.material;
            rendererMaterial.SetColor(BaseColor, Color.red);
        }
        Data.ProjectorDataChanged += OnProjectorDataChanged;
    }

    private void OnValidate() {
        Data.CurrentAngleDelta = Math.Min(Math.Max(Data.MinAngleDelta, Data.CurrentAngleDelta), Data.MaxAngleDelta);
        OnProjectorDataChanged();
    }

    public void OnProjectorDataChanged() {
        if (LightProjector == null) {
            return;
        }
        LightProjector.SetData(Data);
    }

    public void PlayerCanInteract(bool value) {
        if (rendererMaterial != null) {
            rendererMaterial.SetColor(BaseColor, value ? Color.yellow : Color.red);
        }
    }

}