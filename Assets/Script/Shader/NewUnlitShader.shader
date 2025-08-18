Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Texture", 2D) = "white" {}
        _PlayerPos("PlayerPos", Vector) = (0, 0, 0, 0)
        _Radius ("Transparency Radius", Float) = 2.0
        _Smootheness("Smootheness", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }


        Pass
        {
            Tags { "LightMode"="Universal2D" }
            Blend SrcAlpha OneMinusSrcAlpha
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float4 worldPos     : TEXCOORD1;
                float4 positionHCS  : SV_POSITION;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _PlayerPos;  
                float _Radius;
                float _Smootheness;
            CBUFFER_END

            Varyings vert(Attributes i)
            {
                Varyings o = (Varyings)0;
                o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
                o.uv = TRANSFORM_TEX(i.uv, _BaseMap);
                 o.worldPos = mul(unity_ObjectToWorld, i.positionOS);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
                
                float dist = distance(i.worldPos.xy, _PlayerPos.xy);
                float fadeValue = saturate((dist - _Radius)/_Smootheness);
                col.a *= fadeValue;

                return col;
            }
            ENDHLSL
        }
    }
}