Shader "Custom/TileHoverOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineThickness ("Outline Thickness", Range(0.5, 3)) = 3
        _OutlineThreshold ("Outline Threshold", Range(0.01, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineThickness;
            float _OutlineThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 计算屏幕空间中法线的变化
                // 使用ddx和ddy计算法线的梯度
                float3 normalDdx = ddx(i.worldNormal);
                float3 normalDdy = ddy(i.worldNormal);

                // 计算法线变化的幅度
                float normalChange = length(normalDdx) + length(normalDdy);

                // 同时计算世界坐标的变化（作为辅助）
                float3 worldPosDdx = ddx(i.worldPos);
                float3 worldPosDdy = ddy(i.worldPos);
                float worldPosChange = length(worldPosDdx) + length(worldPosDdy);

                // 结合两者作为边缘强度
                float edgeStrength = normalChange * _OutlineThickness * 10.0;

                // 对边缘强度进行平滑处理
                float outlineFactor = smoothstep(_OutlineThreshold, _OutlineThreshold + 0.3, edgeStrength);

                // 采样纹理颜色
                fixed4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // 混合描边颜色
                fixed4 finalColor = lerp(texColor, _OutlineColor, outlineFactor);

                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
