using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Paint : MonoBehaviour {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    
    public Color Color = Color.white;
    
    private Material rendererMaterial;

    private void Awake() {
        if (gameObject.layer != 13) {
            throw new Exception("Paint object must be on layer 13");
        }

        var rendererComponent = GetComponent<Renderer>();
        if (rendererComponent != null) {
            rendererMaterial = rendererComponent.material;
        }
        UpdateMaterial();
    }

    public void SetColor(Color color) {
        Color = color;
        UpdateMaterial();
    }

    private void UpdateMaterial() {
        if (rendererMaterial != null) {
            rendererMaterial.SetColor(BaseColor, Color);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, 0.5f, 0f), 0.1f);
    }
    
}