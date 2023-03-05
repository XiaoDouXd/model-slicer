Shader "Xd/Unlit/clipSurf"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
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

            struct v2f
            {
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(v.normal);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = float4(0.03, 0.04, 0.02, 0);// = tex2D(_MainTex, i.uv);
                float3 vCamera = normalize(UNITY_MATRIX_V[2].xyz);
                float a = dot(float3(-1, 0, 0), i.normal) > 0 ? 1 : 0; //dot(vCamera, i.normal) * 0.5 + 0.5;// > 0 ? 0 : 1;

                col.a = a;
                col.rgb = float3(lerp(0.03, 1, a), 0.04, 0.02);

                return col;
            }
            ENDCG
        }
    }
}
