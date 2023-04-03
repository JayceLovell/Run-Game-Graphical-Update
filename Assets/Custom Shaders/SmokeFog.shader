Shader "Custom/SmokeFog"
{
   Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "gray" {}
        _GradientTex ("Gradient Texture", 2D) = "gray" {}
        _FogColor("Fog Color", Color) = (0.5,0.5,0.5,1)
        _FogDensity("Fog Density", Range(0.0, 1.0)) = 0.1
        _Speed("Speed", Range(0.0, 10.0)) = 1.0
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float fogCoord : FOGCOORD;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            sampler2D _GradientTex;
            float4 _FogColor;
            float _FogDensity;
            float _Speed;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Sample noise and gradient textures
                float noise = tex2D(_NoiseTex, i.uv * _Speed).r;
                float gradient = tex2D(_GradientTex, i.uv).r;

                // Calculate fog factor based on noise and gradient
                float fogFactor = (noise + gradient) * 0.5 * _FogDensity;
                fogFactor = 1.0 - exp(-fogFactor * fogFactor * 1.442695);
                fogFactor = saturate(fogFactor);

                // Calculate final color
                fixed4 c = tex2D(_MainTex, i.uv);
                c.rgb = lerp(c.rgb, _FogColor.rgb, fogFactor);
                c.a = fogFactor;

                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
