// Defines the shader and its name
Shader "Custom/WindowWithOutline"
{
    // Defines the properties that can be edited in the inspector
    Properties
    {
        // The main texture of the material, a 2D image
        _MainTex ("Diffuse", 2D) = "white" {}

        // The color of the outline, a Color value
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)

        // The width of the outline, a float value between 0.0002 and 0.1
        _Outline ("Outline Width", Range (.0002,0.1)) = .0005
    }

    // Defines the rendering settings for the material
    SubShader
    {
        // Defines the rendering queue for the material
        Tags { "Queue" = "Geometry-1" }

        // Disables color writing
        ColorMask 0

        // Disables writing to the depth buffer
        ZWrite off

        // Sets up a stencil operation for the outline
        Stencil{
            // Sets the reference value to 1
            Ref 1
            // Compares always to the stencil buffer
            Comp always
            // Replaces the value in the stencil buffer
            Pass replace
        }

        // The main shader code
        CGPROGRAM

        // Uses the surface shader model with the Lambert lighting model
        #pragma surface surf Lambert vertex:vert

        // Declares the properties used in the shader
        sampler2D _MainTex;     // The main texture
        float _Outline;         // The width of the outline
        float4 _OutlineColor;   // The color of the outline

        // Defines the input structure for the vertex and surface shaders
        struct Input{
            float2 uv_MainTex;  // The UV coordinates for the main texture
        };

        // The vertex shader, which offsets the vertices by the outline width
        void vert (inout appdata_full v){
            v.vertex.xyz += v.normal * _Outline;
        }

        // The surface shader, which sets the color of the material
        void surf(Input IN, inout SurfaceOutput o){
            // Samples the main texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            // Sets the material's albedo color to the main texture color
            o.Albedo = c.rgb;
            // Sets the material's emission color to the outline color
            o.Emission = _OutlineColor.rgb;
        }

        // Ends the CGPROGRAM section
        ENDCG

    }

    // The fallback shader to use if the main shader is not supported
    FallBack "Diffuse"
}
