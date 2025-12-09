Shader "Custom/Open Portal"{
Properties{
    _MainTex ("Render Texture", 2D) = "white" {}
    _MaskTex ("Mask Texture", 2D) = "white" {}
}

SubShader {
    Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    Blend SrcAlpha OneMinusSrcAlpha
    ZWrite Off

    Pass {

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
    	#pragma target 2.0

    	#include "UnityCG.cginc"

        sampler2D _MainTex;
        sampler2D _MaskTex;
        float4 _MainTex_ST;
        float4 _MaskTex_ST;

        struct appdata_t
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct v2f
        {
            float4 screenPos : TEXCOORD1;
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
        };

        v2f vert (appdata_t v) {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.screenPos = ComputeGrabScreenPos(o.vertex);
            o.uv = v.uv;
            return o;
        }

        fixed4 frag (v2f i) : SV_Target {
            float2 screenUV = i.screenPos.xy / i.screenPos.w;
            screenUV.x -= 0.5f;
            screenUV.x *= 400;
            screenUV.x /= 240;
            screenUV.x += 0.5f;
            fixed3 renderColor = tex2D(_MainTex, screenUV);
            fixed4 maskColor = tex2D(_MaskTex, i.uv);
            return fixed4(renderColor, maskColor.a);
        }
        ENDCG
    }
}
}