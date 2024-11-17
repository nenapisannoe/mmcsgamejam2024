using System;
using UnityEngine;

public enum BaseGameComponentType {
    Unrestricted,
    MainGeometry,
    BackgroundGeometry,
    Main,
    SpawnPoint,
    Projector,
    ProjectorController,
    Decor1,
    Decor2,
    Decor3,
}

[ExecuteInEditMode]
public class DepthRestriction : MonoBehaviour {

    private static DepthConfiguration DepthConfiguration;

    public BaseGameComponentType m_Type = BaseGameComponentType.Unrestricted;
    
    #if UNITY_EDITOR
    private void Awake() {
        if (DepthConfiguration == null) {
            DepthConfiguration = Resources.Load<DepthConfiguration>("DepthConfiguration");
        }
    }
    
    private void OnValidate() {
        if (DepthConfiguration == null) {
            DepthConfiguration = Resources.Load<DepthConfiguration>("DepthConfiguration");
        }
    }

    private float GetDepth() {
        return m_Type switch {
            BaseGameComponentType.MainGeometry => DepthConfiguration.MainGeometryDepth,
            BaseGameComponentType.BackgroundGeometry => DepthConfiguration.BackgroundGeometryDepth,
            BaseGameComponentType.Main => DepthConfiguration.MainDepth,
            BaseGameComponentType.SpawnPoint => -DepthConfiguration.BackgroundGeometryDepth + DepthConfiguration.MainDepth,
            BaseGameComponentType.Projector => DepthConfiguration.ProjectorDepth,
            BaseGameComponentType.ProjectorController => DepthConfiguration.ProjectorControllerDepth,
            BaseGameComponentType.Decor1 => DepthConfiguration.Decor1Depth,
            BaseGameComponentType.Decor2 => DepthConfiguration.Decor2Depth,
            BaseGameComponentType.Decor3 => DepthConfiguration.Decor3Depth,
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