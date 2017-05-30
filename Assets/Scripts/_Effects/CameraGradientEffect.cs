using UnityEngine;

[ExecuteInEditMode]
public class CameraGradientEffect : MonoBehaviour {

    public Material material;

    /// <summary>
    /// OnRenderImage is called after all rendering is complete to render image.
    /// </summary>
    /// <param name="src">The source RenderTexture.</param>
    /// <param name="dst">The destination RenderTexture.</param>
    void OnRenderImage (RenderTexture src, RenderTexture dst) {

		Graphics.Blit(src, dst, material);

    }
}