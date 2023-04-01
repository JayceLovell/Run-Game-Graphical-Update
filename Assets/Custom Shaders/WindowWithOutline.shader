Shader "Custom/WindowWithOutline"
{
    Properties
    {
        _MainTex ("Diffuse", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline Width", Range (.0002,0.1)) = .0005
    }
    SubShader
    {
        Tags { "Queue" = "Geometry-1" }

        ColorMask 0
        ZWrite off

        Stencil{
            Ref 1
            Comp always
            Pass replace
        }

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        float _Outline;
        float4 _OutlineColor;

        struct Input{
            float2 uv_MainTex;
        };

        void vert (inout appdata_full v){
        v.vertex.xyz += v.normal * _Outline;
       }

        void surf(Input IN, inout SurfaceOutput o){
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Emission = _OutlineColor.rgb;
        }
        ENDCG

         ZWrite on

        CGPROGRAM
        #pragma surface surf Lambert
        struct Input{
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        void surf (Input IN, inout SurfaceOutput o){
            o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
