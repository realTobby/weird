Shader "Custom/BendWorldShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BendAmount ("Bend Amount", Float) = 0.001
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _BendAmount;

            v2f vert (appdata v)
            {
                v2f o;
                float3 pos = v.vertex.xyz;

                // Bend effect along the z-axis
                float theta = pos.z * _BendAmount;
                float cosTheta = cos(theta);
                float sinTheta = sin(theta);

                float newX = pos.x * cosTheta - pos.y * sinTheta;
                float newY = pos.x * sinTheta + pos.y * cosTheta;

                pos.x = newX;
                pos.y = newY;

                o.vertex = UnityObjectToClipPos(float4(pos, 1.0));
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
