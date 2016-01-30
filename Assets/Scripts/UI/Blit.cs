using UnityEngine;
using System.Collections;

public class Blit : MonoBehaviour {

    [SerializeField]
    protected Material BlitMaterial;

    Material instantiatedBlitMaterial;

    public float fadeProgress; //0 = no fade, 1  = fade

    void Awake()
    {
        instantiatedBlitMaterial = Instantiate(BlitMaterial);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (fadeProgress == 0)
        {
            Graphics.Blit(source, destination);
        }
        else
        {
            instantiatedBlitMaterial.SetFloat(Tags.ShaderParams.cutoff, fadeProgress);
            Graphics.Blit(source, destination, instantiatedBlitMaterial);
        }
    }
}
