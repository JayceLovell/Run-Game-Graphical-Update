//This shader is a combination of Stencil and Normal Maps
Shader "Custom/Shader For Walls"
{
    Properties
    {
        _WallTex ("Wall Texture", 2D) = "white" {}
        _BumpMap ("Bumpmap", 2D) = "bump" {}
        _Detail ("Detail", 2D) = "gray" {}
    }
    SubShader
    {
       Tags { "RenderType"="Opaque" }

       //To See through Walls
       Stencil {
            Ref 1               // use 1 as the value to check against
            Comp notequal// If this stencil Ref 1 is not equal to what's in the stencil buffer, then we will keep this pixel that belongs to the Wall
            Pass keep           // If you do find a 1, don't draw it.            
        }
       
       // The rest is Normal Maps
        CGPROGRAM
       
        #pragma surface surf Lambert            

        sampler2D _FloorTex;
        sampler2D _BumpMap;
        sampler2D _Detail;

        struct Input
        {
            float2 uv_FloorTex;
            float2 uv_BumpMap;
            float2 uv_Detail;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
          o.Albedo = tex2D (_FloorTex, IN.uv_FloorTex).rgb;
          o.Albedo *= tex2D (_Detail, IN.uv_Detail).rgb * 2;
          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
