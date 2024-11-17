using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Paint : MonoBehaviour {
    
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    
    public Color Color = Color.white;
    
    public Renderer ColorRenderer;
    public Renderer SpecularColorRenderer;

    private Material colorMaterial;
    private Material specularColorMaterial;

    private void Awake() {
        if (gameObject.layer != 13) {
            throw new Exception("Paint object must be on layer 13");
        }

        colorMaterial = ColorRenderer.material;
        specularColorMaterial = SpecularColorRenderer.material;
        UpdateMaterial();
    }

    public void SetColor(Color color) {
        Color = color;
        UpdateMaterial();
    }

    private void UpdateMaterial() {
        colorMaterial.SetColor(BaseColor, Color);
        specularColorMaterial.SetColor(BaseColor, Color);
        specularColorMaterial.SetColor(EmissionColor, Color * 2f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, 1.5f, 0f), 0.1f);
    }
    
}