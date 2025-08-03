Shader "Unlit/BossFight"
{
    Properties
    {
        _BackgroundCol("BackgroundColor", Color) = (0, 0, 0, 0)
        _Color("Color", Color) = (1, 0.3, 0.3, 1)
        _Radius("Glow Radius", Range(0.01, 1)) = 0.1
        _MoveTime("Time", Float) = 0.1
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100
        blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _BackgroundCol;
            float4 _Color;
            float _Radius;
            float _MoveTime;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p) {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

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
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 offset = float2(
                    noise(float2(_MoveTime, 0.0)),
                    noise(float2(0.0, _MoveTime))
                ) - 0.5;

                float2 center = float2(0.5, 0.5) + offset;
    
                float dist = distance(i.uv, center);
                float glow = smoothstep(_Radius, 0.0, dist);

                float4 col = (dist > _Radius) ? _BackgroundCol : (_Color * glow);
                col.a = 1.0;

                return col;
            }

            ENDCG
        }
    }
}
