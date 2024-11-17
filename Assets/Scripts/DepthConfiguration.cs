using UnityEngine;

[CreateAssetMenu(fileName = "DepthConfiguration", menuName = "AllTheColors/DepthConfiguration", order = 0)]
public class DepthConfiguration : ScriptableObject {
    
    public float MainGeometryDepth;
    public float BackgroundGeometryDepth;
    public float MainDepth;
    public float ProjectorDepth;
    public float ProjectorControllerDepth;
    public float Decor1Depth;
    public float Decor2Depth;
    public float Decor3Depth;
    
}