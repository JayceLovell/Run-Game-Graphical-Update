using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blur : MonoBehaviour
{
    // Variables for controlling blur strength and iterations in editor
    [Range(1, 64)]
    public int integerRange = 1;
    [Range(1, 16)]
    public int iterations = 1;

    // Method which is automatically called by Unity after the camera is done rendering
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Reduce the resolution of the source texture by integerRange
        int width = source.width / integerRange;
        int height = source.height / integerRange;

        // Create the initial destination texture
        RenderTextureFormat format = source.format;
        RenderTexture[] textures = new RenderTexture[16];
        RenderTexture currentDestination = textures[0] =
            RenderTexture.GetTemporary(width, height, 0, format);

        // Blit the source texture to the initial destination texture
        Graphics.Blit(source, currentDestination);

        // Set the current source texture to the initial destination texture
        RenderTexture currentSource = currentDestination;

        // Loop through the specified number of iterations
        int i = 1;
        for (; i < iterations; i++)
        {
            // Reduce the width and height by half for each iteration
            width /= 2;
            height /= 2;

            // Create a new destination texture
            currentDestination = textures[i] =
                RenderTexture.GetTemporary(width, height, 0, format);

            // If the height is less than 2, stop iterating
            if (height < 2)
            {
                break;
            }

            // Blit the current source texture to the new destination texture
            Graphics.Blit(currentSource, currentDestination);

            // Release the current source texture
            RenderTexture.ReleaseTemporary(currentSource);

            // Set the current source texture to the new destination texture
            currentSource = currentDestination;
        }

        // Loop through any remaining iterations
        for (; i < iterations; i++)
        {
            Graphics.Blit(currentSource, currentDestination);
            currentSource = currentDestination;
        }

        // Loop through the RenderTextures in reverse order and create the final blurred image
        for (i -= 2; i >= 0; i--)
        {
            currentDestination = textures[i];
            textures[i] = null;
            Graphics.Blit(currentSource, currentDestination);
            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = currentDestination;
        }

        // Blit the final blurred image to the destination RenderTexture parameter
        Graphics.Blit(currentSource, destination);
    }
}
