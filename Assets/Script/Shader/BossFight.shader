Shader "Unlit/BossFight"
{
    Properties
    {
        _BackgroundCol("BackgroundColor", Color) = (0, 0, 0, 1)
        _Color("Color", Color) = (1, 0.3, 0.3, 1)
        _MoveTime("Time", Float) = 0.1
        _Smoothness("Smoothness", Range(0.01,1)) = 0.1
        _Center("Center", Vector) = (0.5, 0.5, 0, 0)
        _MovementRadius("MovementRadius", float) = 0.5
        _Radius("Radius", Range(0.01, 1)) = 0.1
        _FlameCount("FlameCount", Int) = 5
        _Seed("Seed", Float) = 0.0
        _GlowIntensity("GlowIntensity", Float) = 2.0
        _GlowRange("GlowRange", Float) = 1.5
        _AlphaFadeStrength("AlphaFadeStrength", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        // 첫 번째 Pass: 어두운 배경 + 알파 조절
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            int _FlameCount;
            float4 _BackgroundCol;
            float4 _Color;
            float _MoveTime;
            float _Smoothness;
            float4 _Center;
            float _MovementRadius;
            float _Radius;
            float _Seed;
            float _GlowIntensity;
            float _GlowRange;
            float _AlphaFadeStrength;

            float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            float noise(float2 p) {
                float2 i = floor(p); float2 f = frac(p);
                float a = hash(i); float b = hash(i + float2(1,0)); float c = hash(i + float2(0,1)); float d = hash(i + float2(1,1));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };
            v2f vert(appdata v) { v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv = v.uv; return o; }
            fixed4 frag(v2f i) : SV_Target
            {
                float maxGlow = 0.0;
                for (int flame = 0; flame < _FlameCount; flame++) {
                    float2 noiseInput = float2(_MoveTime * 0.1 * _Smoothness + flame * 10.0 + _Seed, _MoveTime * 0.1 * _Smoothness + flame * 20.0 + _Seed);
                    float angle = noise(noiseInput + float2(10.0, 0)) * 6.2831;
                    float radius = noise(noiseInput + float2(0.0, 10.0)) * _MovementRadius;
                    float2 offset = float2(cos(angle), sin(angle)) * radius;
                    float2 center = _Center.xy + offset;
                    center = clamp(center, _Radius, 1.0 - _Radius);
                    float dist = distance(i.uv, center);  // 여기서 dist 선언
                    float glow = pow(1.0 - smoothstep(0.0, _Radius * _GlowRange, dist), _GlowIntensity);
                    maxGlow = max(maxGlow, glow);
                }
                float alpha = pow(1.0 - maxGlow, _AlphaFadeStrength);
                return float4(_BackgroundCol.rgb, alpha);
            }
            ENDCG
        }

        // 두 번째 Pass: 불꽃 시각적 렌더링
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            int _FlameCount;
            float4 _BackgroundCol;
            float4 _Color;
            float _MoveTime;
            float _Smoothness;
            float4 _Center;
            float _MovementRadius;
            float _Radius;
            float _Seed;
            float _GlowIntensity;
            float _GlowRange;
            float _AlphaFadeStrength;

            float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
            float noise(float2 p) {
                float2 i = floor(p); float2 f = frac(p);
                float a = hash(i); float b = hash(i + float2(1,0)); float c = hash(i + float2(0,1)); float d = hash(i + float2(1,1));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };
            v2f vert(appdata v) { v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv = v.uv; return o; }
            fixed4 frag(v2f i) : SV_Target
            {
                float maxGlow = 0.0;
                for (int flame = 0; flame < _FlameCount; flame++) {
                    float2 noiseInput = float2(_MoveTime * 0.1 * _Smoothness + flame * 10.0 + _Seed, _MoveTime * 0.1 * _Smoothness + flame * 20.0 + _Seed);
                    float angle = noise(noiseInput + float2(10.0, 0)) * 6.2831;
                    float radius = noise(noiseInput + float2(0.0, 10.0)) * _MovementRadius;
                    float2 offset = float2(cos(angle), sin(angle)) * radius;
                    float2 center = _Center.xy + offset;
                    center = clamp(center, _Radius, 1.0 - _Radius);
                    float dist = distance(i.uv, center);  // 여기서 dist 선언
                    float glow = pow(1.0 - smoothstep(0.0, _Radius * _GlowRange, dist), _GlowIntensity);
                    maxGlow = max(maxGlow, glow);
                }
                float4 flameColor = lerp(_Color, float4(1,1,1,1), maxGlow);
                flameColor.a = maxGlow;
                return flameColor;
            }
            ENDCG
        }

                // 세 번째 Pass: 빛 additive 추가
                Pass
                {
                    Blend SrcAlpha One
                    CGPROGRAM
                    #pragma vertex vert
                    #pragma fragment frag
                    #include "UnityCG.cginc"
                    int _FlameCount;
                    float4 _BackgroundCol;
                    float4 _Color;
                    float _MoveTime;
                    float _Smoothness;
                    float4 _Center;
                    float _MovementRadius;
                    float _Radius;
                    float _Seed;
                    float _GlowIntensity;
                    float _GlowRange;
                    float _AlphaFadeStrength;

                    float hash(float2 p) { return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453); }
                    float noise(float2 p) {
                        float2 i = floor(p); float2 f = frac(p);
                        float a = hash(i); float b = hash(i + float2(1,0)); float c = hash(i + float2(0,1)); float d = hash(i + float2(1,1));
                        float2 u = f * f * (3.0 - 2.0 * f);
                        return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
                    }
                    struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
                    struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };
                    v2f vert(appdata v) { v2f o; o.vertex = UnityObjectToClipPos(v.vertex); o.uv = v.uv; return o; }
                    fixed4 frag(v2f i) : SV_Target
                    {
                        float3 lightInfluence = float3(0,0,0);
                        float maxGlow = 0.0;
                        for (int flame = 0; flame < _FlameCount; flame++) {
                            float2 noiseInput = float2(_MoveTime * 0.1 * _Smoothness + flame * 10.0 + _Seed, _MoveTime * 0.1 * _Smoothness + flame * 20.0 + _Seed);
                            float angle = noise(noiseInput + float2(10.0, 0)) * 6.2831;
                            float radius = noise(noiseInput + float2(0.0, 10.0)) * _MovementRadius;
                            float2 offset = float2(cos(angle), sin(angle)) * radius;
                            float2 center = _Center.xy + offset;
                            center = clamp(center, _Radius, 1.0 - _Radius);
                            float dist = distance(i.uv, center);  // 여기서 dist 선언
                            float glow = pow(1.0 - smoothstep(0.0, _Radius * _GlowRange, dist), _GlowIntensity);
                            maxGlow = max(maxGlow, glow);
                            lightInfluence += _Color.rgb * glow * _GlowIntensity;
                        }
                        return float4(lightInfluence, maxGlow);
                    }
                    ENDCG
                }
    }
}