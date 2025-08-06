Shader "Custom/CenterCircleMask"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {} // 화면 텍스처 (포스트 프로세싱용)
        _BackgroundColor("BackgroundColor", Color) = (0, 0, 0, 0)
        _Radius("Radius", Float) = 0.5 // 원의 반경 (0~1 범위, Inspector에서 조절)
        _Center("Center", Vector) = (0.5, 0.5, 0, 0) // 중앙 위치 (UV 기준)
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 활성화

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

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv); // 원본 화면 색상
                    col.xyz = _BackgroundColor.xyz; 

                    // 중앙부터 거리 계산 (UV 기준)
                    float2 relative = i.uv - _Center.xy;
                    // 화면 비율 보정 (optional: 원형 유지, aspect ratio 고려)
                    relative.x *= _ScreenParams.x / _ScreenParams.y;

                    float dist = length(relative); // 중앙부터 거리

                    // 거리가 Radius 초과하면 알파 0 (투명)
                    col.a = 1 - smoothstep(_Radius , _Radius * 1.3, dist);

                    return col;
                }
                ENDCG
            }
        }
}