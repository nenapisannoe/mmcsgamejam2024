using System;
using UnityEngine;

[ExecuteInEditMode]
public class DepthController : MonoBehaviour {
    
    #if UNITY_EDITOR
    
    public float MainGeometryDepth;
    public float BackgroundGeometryDepth;
    public float MainDepth;
    public float ProjectorDepth;
    public float ProjectorControllerDepth;

    public float Decor1Depth;
    public float Decor2Depth;
    public float Decor3Depth;

    public static DepthController Instance;
    
    private void OnValidate() {
        if (Instance != null) {
            throw new Exception("DepthController already exists!");
        }
        Instance = this;
    }
    
    #endif
    
}