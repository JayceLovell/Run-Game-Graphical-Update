Shader "Custom/WindowWithOutline"
{
    // Define editable properties for this shader in the Unity Editor
    Properties
    {
        _MainTex ("Diffuse", 2D) = "white" {}        // Window texture
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)    // Outline color
        _Outline ("Outline Width", Range (.0002,0.1)) = .0005    // Outline width
    }

    // Define the rendering code for this shader
    SubShader
    {
        // Set the rendering order for this shader
        Tags { "Queue" = "Geometry-1" }

        // Disable color writes and depth buffer writes for this shader
        ColorMask 0
        ZWrite off

        // Set up the stencil buffer to replace any existing value with 1
        Stencil{
            Ref 1
            Comp always
            Pass replace
        }

        // Define the actual shader code using the CG programming language
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert    // Set the shader model and vertex function

        // Define shader variables
        sampler2D _MainTex;        // Window texture variable
        float _Outline;        // Outline width variable
        float4 _OutlineColor;        // Outline color variable

        // Define the input struct that holds the UV coordinates for the texture
        struct Input{
            float2 uv_MainTex;
        };

        // Define the vertex function that offsets the vertex position to create the outline
        void vert (inout appdata_full v){
            v.vertex.xyz += v.normal * _Outline;
        }

        // Define the surface function that samples the texture and sets the output colors
        void surf(Input IN, inout SurfaceOutput o){
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);    // Sample the texture at the current UV coordinates
            o.Albedo = c.rgb;    // Set the output Albedo color to the sampled color
            o.Emission = _OutlineColor.rgb;    // Set the output Emission color to the outline color
        }
        ENDCG

    }

    // Define a fallback shader to use if this custom shader fails to compile or run
    FallBack "Diffuse"
}
