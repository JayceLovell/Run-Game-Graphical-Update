Shader "Custom/Distortion"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _DistortionMap ("Distortion Map", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.0

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _DistortionMap;
            float _DistortionStrength;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float4 dist = tex2D(_DistortionMap, v.uv);
                float2 perturb = float2(dist.r, dist.g) * _DistortionStrength;

                o.vertex.xy += perturb;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
