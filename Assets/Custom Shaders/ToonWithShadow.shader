// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ToonWithShadow"
{
     Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Ramp("Toon Ramp", 2D) = "white" {}
        _Threshold("Threshold", Range(0,1)) = 1
    }

    SubShader
    {
        Pass
        {
        Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"


            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 diff : COLOR0;
                SHADOW_COORDS(1)
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0;
                TRANSFER_SHADOW(o);

                return o;
            }

            sampler2D _MainTex;
            sampler2D _Ramp;
            float _Threshold;
            float4 _Color;

            float4 frag(v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);
                 fixed shadow = SHADOW_ATTENUATION(i);
                 col.rgb *= i.diff;
                float ramp = tex2D(_Ramp, float2(col.r, 0.5)).r;
                col.rgb = step(ramp, _Threshold) * col.rgb;
                fixed4 finalColour = shadow ? col : fixed4(0.0f, 1.0f, 0.0f, 1.0f);
                return finalColour;
                //return col;
            }
            ENDCG
        }
        Pass{
            Tags {"LightMode" = "ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            };
             struct v2f{
                V2F_SHADOW_CASTER;
                
            };

            v2f vert(appdata v){
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }
             float4 frag(v2f i) : SV_Target{
                

                SHADOW_CASTER_FRAGMENT(i)

                
            }
            ENDCG
        
        }
    }
    FallBack "Diffuse"
}