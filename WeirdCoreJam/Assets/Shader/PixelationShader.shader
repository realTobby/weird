Shader "Hidden/PixelationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelationAmount ("Pixelation Amount", Range(1, 1024)) = 128
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _PixelationAmount;

            half4 frag (v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                uv = floor(uv * _PixelationAmount) / _PixelationAmount;
                half4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack Off
}
