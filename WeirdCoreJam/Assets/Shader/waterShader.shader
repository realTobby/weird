Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Range(0.0, 99.0)) = 0.1
        _CustomTime ("Custom Time", Range(0, 1000)) = 0
        _NormalMap ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade

        sampler2D _MainTex;
        sampler2D _NormalMap;
        float _WaveSpeed;
        float _CustomTime;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            float2 waveUV = IN.uv_MainTex;
            waveUV.y += _CustomTime * _WaveSpeed;
            waveUV.x += _CustomTime * _WaveSpeed;

            o.Albedo = tex2D(_MainTex, waveUV).rgb;
            o.Alpha = tex2D(_MainTex, waveUV).a;

            float3 normal = UnpackNormal(tex2D(_NormalMap, waveUV));
            o.Normal = normal;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
