Shader "Custom/Diffuse"
{
    Properties
    {
         _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
     
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = mainTex.rgb;
            o.Metallic = 0;
            o.Smoothness = 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
