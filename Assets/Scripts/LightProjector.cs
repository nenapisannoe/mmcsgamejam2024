using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightProjector : MonoBehaviour {
    
    public Light SpotLight;
    public MeshFilter MeshFilter;

    private void Start() {
        Init();
    }

    public void Init() {
        //var radius = SpotLight.;
        MeshFilter.mesh = ConeCreator.GetMesh(0.5f);
    }
    
}