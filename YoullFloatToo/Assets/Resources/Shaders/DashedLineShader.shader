Shader "Custom/DashedLineURP"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _DashSize ("Dash Size", Float) = 1.0
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent"}
        Pass
        {
            ZWrite On
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask RGB
            Offset -1, -1

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            uniform float _DashSize;
            uniform float4 _Color;
            uniform sampler2D _MainTex;
            uniform float _ScrollSpeed;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float2 uv = i.uv;
                float d = frac(uv.x * _DashSize);
                uv.x -= _ScrollSpeed * _Time.y; // Ajuste para el desplazamiento
                return d < 0.5 ? _Color * tex2D(_MainTex, uv) : half4(0, 0, 0, 0);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}