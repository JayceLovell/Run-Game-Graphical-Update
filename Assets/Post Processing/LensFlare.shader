Shader "Custom/LensFlare" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _Threshold ("Threshold", Range(0, 1)) = 0.5
        _Brightness ("Brightness", Range(0, 1)) = 0.5
        _Falloff ("Falloff", Range(0, 1)) = 0.1
    }
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard

        struct Input {
            float2 uv_MainTex;
            sampler2D _MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Calculate the brightness of the lens flare
            float brightness = tex2D(IN._MainTex, IN.uv_MainTex).a;
            brightness = pow(brightness, _Brightness);

            // Apply falloff to the lens flare
            float falloff = saturate(1.0 - pow(brightness, _Falloff));

            // Apply the tint color and brightness to the output
            o.Albedo = _Color.rgb * brightness * falloff;
            o.Alpha = falloff;
        }

        sampler2D _MainTex;

        ENDCG
    }
    FallBack "Diffuse"
}