Shader "Custom/ScrollingStripes"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1, 1, 1, 1)
        _ColorB ("Color B", Color) = (0, 0, 0, 1)
        _Density ("Stripe Density", Float) = 10.0
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100

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

            fixed4 _ColorA;
            fixed4 _ColorB;
            float _Density;
            float _ScrollSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. 实现 45 度旋转
                // 在 UV 空间中，(u + v) 会产生斜线效果
                // _Time.y 是内置的匀速增长时间
                float pos = (i.uv.x + i.uv.y) * _Density + (_Time.y * _ScrollSpeed);

                // 2. 使用 frac (取小数部分) 配合 step 产生硬边条纹
                // step(0.5, x) 会让一半为 0，一半为 1
                float stripe = step(0.5, frac(pos));

                // 3. 根据条纹值在两个颜色间插值
                fixed4 col = lerp(_ColorA, _ColorB, stripe);
                
                return col;
            }
            ENDCG
        }
    }
}