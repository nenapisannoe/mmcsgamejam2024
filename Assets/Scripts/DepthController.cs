using UnityEngine;

[ExecuteInEditMode]
public class DepthController : MonoBehaviour {
    
    #if UNITY_EDITOR
    
    public float MainGeometryDepth;
    public float BackgroundGeometryDepth;
    public float CharactersDepth;
    public float LightsDepth;

    public static DepthController Instance;
    
    private void OnValidate() {
        Instance = this;
    }
    
    #endif
    
}