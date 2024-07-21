Shader "Custom/TerrainGrassShader"
{
    Properties
    {
        _Color ("Color", Color) = (0.5, 0.8, 0.2, 1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _WindStrength ("Wind Strength", Float) = 1.0
        _WindFrequency ("Wind Frequency", Float) = 1.0
        _BladeHeight ("Blade Height", Float) = 1.0
        _Density ("Density", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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
            fixed4 _Color;
            float _WindStrength;
            float _WindFrequency;
            float _BladeHeight;
            float _Density;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.y *= _BladeHeight;
                v.vertex.x *= _Density;
                v.vertex.z *= _Density;
                v.vertex.x += sin(_Time.y * _WindFrequency + v.vertex.x) * _WindStrength;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
