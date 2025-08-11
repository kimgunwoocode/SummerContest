Shader "Custom/CenterCircleMask"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BackgroundColor("BackgroundColor", Color) = (0, 0, 0, 0)
        
        _Radius("Radius", Float) = 0.5 
        _Center("Center", Vector) = (0.5, 0.5, 0, 0) 
        _IsReversedAlpha("IsReversedAlpha", Float) = 0
        _IsReversedColor("IsReversedColor", Float) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 _MainTex_ST;
                float _Radius;
                float4 _Center;
                float4 _BackgroundColor;
                float _IsReversedAlpha;
                float _IsReversedColor;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    float4 reversedColor = 1.0 - _BackgroundColor;
                    col.xyz = _BackgroundColor.xyz; 
                    if (_IsReversedColor == 1) {
                        col.xyz = reversedColor.xyz;
                    }

                    float2 relative = i.uv - _Center.xy;
                    relative.x *= _ScreenParams.x / _ScreenParams.y;

                    float dist = length(relative);

                    col.a = 1 - smoothstep(_Radius , _Radius * 1.3, dist);
                    if (_IsReversedAlpha == 1) {
                        col.a = 1 - col.a;
                    }

                    return col;
                }
                ENDCG
            }
        }
}