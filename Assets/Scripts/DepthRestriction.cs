using System;
using UnityEngine;

public enum BaseGameComponentType {
    Unrestricted,
    MainGeometry,
    BackgroundGeometry,
    Main,
    Projector,
    ProjectorController,
    Decor1,
    Decor2,
    Decor3,
}

[ExecuteInEditMode]
public class DepthRestriction : MonoBehaviour {

    public BaseGameComponentType m_Type = BaseGameComponentType.Unrestricted;
    
    #if UNITY_EDITOR
    private float GetDepth() {
        return m_Type switch {
            BaseGameComponentType.MainGeometry => DepthController.Instance.MainGeometryDepth,
            BaseGameComponentType.BackgroundGeometry => DepthController.Instance.BackgroundGeometryDepth,
            BaseGameComponentType.Main => DepthController.Instance.MainDepth,
            BaseGameComponentType.Projector => DepthController.Instance.ProjectorDepth,
            BaseGameComponentType.ProjectorController => DepthController.Instance.ProjectorControllerDepth,
            BaseGameComponentType.Decor1 => DepthController.Instance.Decor1Depth,
            BaseGameComponentType.Decor2 => DepthController.Instance.Decor2Depth,
            BaseGameComponentType.Decor3 => DepthController.Instance.Decor3Depth,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private void Update() {
        if(!Application.isPlaying && m_Type is not BaseGameComponentType.Unrestricted) {
           transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, GetDepth());
        }
    }
    #endif
    
}