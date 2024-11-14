using System;
using UnityEngine;

public enum BaseGameComponentType {
    Unrestricted,
    MainGeometry,
    BackgroundGeometry,
    Characters,
    Lights,
}

[ExecuteInEditMode]
public class DepthRestriction : MonoBehaviour {

    public BaseGameComponentType m_Type = BaseGameComponentType.Unrestricted;
    
    #if UNITY_EDITOR
    private float GetDepth() {
        return m_Type switch {
            BaseGameComponentType.MainGeometry => DepthController.Instance.MainGeometryDepth,
            BaseGameComponentType.BackgroundGeometry => DepthController.Instance.BackgroundGeometryDepth,
            BaseGameComponentType.Characters => DepthController.Instance.CharactersDepth,
            BaseGameComponentType.Lights => DepthController.Instance.LightsDepth,
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