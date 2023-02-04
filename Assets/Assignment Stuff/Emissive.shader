Shader "Custom/Emissive"
{
    Properties
    {   
        _Color ("Color", Color) = (1,0,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Emission ("Emission", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature __ _EMISSION

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
            float4 _MainTex_ST;
            float4 _Color;
            float _Emission;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = _Color * tex2D(_MainTex, i.uv);
                col.rgb += _Emission;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}