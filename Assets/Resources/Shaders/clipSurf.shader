Shader "Xd/Unlit/clipSurf"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="XD" }
        LOD 100

        Cull Off
        ZWrite On
        ZTest Less

        //AlphaToMask On
        Blend One Zero, One Zero
        BlendOp Add

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct pixel_data
            {
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            pixel_data vert (const appdata v)
            {
                pixel_data o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            float4 frag(const pixel_data p) : SV_Target
            {
                // UNITY_MATRIX_V[2】 是当前相机在世界空间中的视线方向 - float3
                // p.normal 是当前像素在世界空间中的法线方向 - float3
                return dot(UNITY_MATRIX_V[2].xyz, p.normal) > 0 ?
                    float4(0, 0, 0, 0) : float4(1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
