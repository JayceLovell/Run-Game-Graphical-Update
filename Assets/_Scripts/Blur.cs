using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blur : MonoBehaviour
{
    [Range(1,64)]
    public int integerRange = 1;
    [Range(1, 16)]
    public int iterations = 1;
    //method which is automatically called by unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width / integerRange;
        int height = source.height / integerRange;
        RenderTextureFormat format = source.format;
        RenderTexture[] textures = new RenderTexture[16];
        RenderTexture currentDestination = textures[0] =
        RenderTexture.GetTemporary(width, height, 0, format);
        Graphics.Blit(source, currentDestination);
        RenderTexture currentSource = currentDestination;
        Graphics.Blit(currentSource, destination);
        RenderTexture.ReleaseTemporary(currentSource);
        int i = 1;
        for (; i < iterations; i++)
        {
            width /= 2;
            height /= 2;
            currentDestination = textures[i] =
            RenderTexture.GetTemporary(width, height, 0, format);
            if (height < 2)
            {
                break;
            }
            currentDestination =
            RenderTexture.GetTemporary(width, height, 0, format);
            Graphics.Blit(currentSource, currentDestination);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }
        for (; i < iterations; i++)
        {
            Graphics.Blit(currentSource, currentDestination);
            // RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }
        for (i -= 2; i >= 0; i--)
        {
            currentDestination = textures[i];
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }
        Graphics.Blit(currentSource, destination);
    }
}