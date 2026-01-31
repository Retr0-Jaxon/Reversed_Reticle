Shader "Custom/TileHoverShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 0.5)) = 0.05
        _MainColor ("Main Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            fixed4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 检测 UV 是否在边缘范围内
                bool isOutline = i.uv.x < _OutlineWidth || i.uv.x > (1.0 - _OutlineWidth) ||
                                 i.uv.y < _OutlineWidth || i.uv.y > (1.0 - _OutlineWidth);

                if (isOutline)
                {
                    return _OutlineColor;
                }

                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
                return col;
            }
            ENDCG
        }
    }
}