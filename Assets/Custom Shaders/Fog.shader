Shader "Custom/Fog"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor("Fog Color", Color) = (0.5,0.5,0.5,1)
        _FogDensity("Fog Density", Range(0.0, 1.0)) = 0.1
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}
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
            float4 _FogColor;
            float _FogDensity;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float fogFactor = (o.vertex.z / o.vertex.w) * _FogDensity;
                fogFactor = 1.0 - exp(-fogFactor * fogFactor * 1.442695);
                fogFactor = saturate(fogFactor);
                o.fogCoord = fogFactor;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv);
                c.a = i.fogCoord;
                c = lerp(c, _FogColor, i.fogCoord);
                return c;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
