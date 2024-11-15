using System;
using UnityEngine;

[Serializable]
public class ProjectorControllerData {

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

public class ProjectorController : MonoBehaviour {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    public Projector Projector;
    
    private Material rendererMaterial;

    public ProjectorControllerData Data = new ProjectorControllerData();

    private void Awake() {
        var rendererComponent = GetComponent<Renderer>();
        if (rendererComponent != null) {
            rendererMaterial = rendererComponent.material;
            rendererMaterial.SetColor(BaseColor, Color.red);
        }
        Data.ProjectorDataChanged += OnProjectorDataChanged;
    }

    private void OnProjectorDataChanged() {
        Projector.SetData(Data);
    }

    public void PlayerCanInteract(bool value) {
        if (rendererMaterial != null) {
            rendererMaterial.SetColor(BaseColor, value ? Color.yellow : Color.red);
        }
    }

}