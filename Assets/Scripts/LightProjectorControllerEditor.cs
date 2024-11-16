using System;
using UnityEditor;

[CustomEditor(typeof(LightProjectorController))]
public class LightProjectorControllerEditor : Editor {
    
    private LightProjectorController controller;
    
    void OnEnable() {
        controller = (LightProjectorController)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var data = controller.Data;
        data.CurrentAngleDelta = Math.Min(Math.Max(data.MinAngleDelta, data.CurrentAngleDelta), data.MaxAngleDelta);
        
        controller.OnProjectorDataChanged();
    }
        
}