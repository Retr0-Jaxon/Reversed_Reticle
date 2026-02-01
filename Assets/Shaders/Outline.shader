Shader "Custom/OuterOutline"
{
    Properties
    {
        _MainTex ("Render Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _Thickness ("Thickness", Range(1, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Unity自动填充：x=1/width, y=1/height
            fixed4 _OutlineColor;
            float _Thickness;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 如果当前像素已经有颜色（是正方形内部），则不绘制描边（或者绘制原色）
                // 这里我们假设只画描边，不画内部
                if (col.a > 0.1) {
                    return fixed4(0,0,0,0); 
                }

                // 采样周边 8 个方向
                float2 offsets[8] = {
                    float2(-1,-1), float2(0,-1), float2(1,-1),
                    float2(-1, 0),               float2(1, 0),
                    float2(-1, 1), float2(0, 1), float2(1, 1)
                };

                float maxAlpha = 0;
                for(int n = 0; n < 8; n++) {
                    // 根据厚度偏移采样点
                    float2 sampleUV = i.uv + offsets[n] * _MainTex_TexelSize.xy * _Thickness;
                    maxAlpha = max(maxAlpha, tex2D(_MainTex, sampleUV).a);
                }

                // 如果周围有正方形（alpha > 0），则当前像素绘制为描边色
                return _OutlineColor * maxAlpha;
            }
            ENDCG
        }
    }
}