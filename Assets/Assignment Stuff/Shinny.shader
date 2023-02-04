Shader "Custom/Shinny"
{
    Properties{        
        _MainTex("Main Texture", 2D) = "white" {}
        _SpecColor("Color", Color) = (1.0,0.843137,1.0)
        _Shininess("Shininess", Float) = 10
    }

    SubShader{
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            //user defined variables
            uniform sampler2D _MainTex;
            uniform float4 _SpecColor;
            uniform float _Shininess;                

            // unity defined variables
            uniform float4 _LightColor0;

            // base input structs
            struct vertexInput {
                float4 vertex: POSITION;
                float3 normal: NORMAL;
                float2 uv: TEXCOORD0;
            };
            
            struct vertexOutput {
                float4 pos: SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float4 normalDir : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };
            
            // vertex functions
            vertexOutput vert(vertexInput v) {
                vertexOutput o;

                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject));
                o.uv = v.uv;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            // fragment function
            float4 frag(vertexOutput i) : COLOR
            {
                //vectors
                float3 normalDirection = i.normalDir;
                float atten =1.0;
                float4 color = tex2D(_MainTex, i.uv);

                // lighting
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 diffuseReflection = atten * _LightColor0.xyz * max(0.0,dot(normalDirection,lightDirection));

                // specular direction
                float3 lightReflectDirection = reflect(-lightDirection, normalDirection);
                float3 viewDirection = normalize(float3(float4(_WorldSpaceCameraPos.xyz, 1.0) -i.posWorld.xyz));
                float3 lightSeeDirection = max(0.0,dot(lightReflectDirection, viewDirection));
                float3 shininessPower = pow(lightSeeDirection, _Shininess);
                float3 specularReflection = atten * _SpecColor.rgb * shininessPower;
                float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT;
                
                return float4(lightFinal * color.rgb, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
