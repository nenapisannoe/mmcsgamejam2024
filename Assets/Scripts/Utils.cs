using System.Linq;
using UnityEngine;

public static class Utils {

    public static readonly System.Random Random = new System.Random();

    public static bool CloseColors(Color color1, Color color2, float eps = 0.1f) {
        var dR = Mathf.Abs(color1.r - color2.r);
        var dB = Mathf.Abs(color1.b - color2.b);
        var dG = Mathf.Abs(color1.g - color2.g);

        return dR < eps && dB < eps && dG < eps;
    }

    public static Color MixedColor(Color[] colors) {
        var sumR = 0.0f;
        var sumG = 0.0f;
        var sumB = 0.0f;
        foreach (var color in colors) {
            sumR += color.r;
            sumG += color.g;
            sumB += color.b;
        }
        var maxComponent = new []{sumR, sumB, sumG}.Max();
        return new Color(sumR / maxComponent, sumG / maxComponent, sumB / maxComponent);
    }
}
