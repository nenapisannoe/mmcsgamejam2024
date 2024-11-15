using UnityEngine;

[ExecuteInEditMode]
public class DepthController : MonoBehaviour {
    
    #if UNITY_EDITOR
    
    public float MainGeometryDepth;
    public float BackgroundGeometryDepth;
    public float MainDepth;
    public float LightsDepth;

    public float Decor1Depth;
    public float Decor2Depth;
    public float Decor3Depth;

    public static DepthController Instance;
    
    private void OnValidate() {
        Instance = this;
    }
    
    #endif
    
}