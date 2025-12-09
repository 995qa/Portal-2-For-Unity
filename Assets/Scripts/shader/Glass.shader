Shader "3DS/Glass"
{
    Properties
    {
        _Emm ("Alpha", Float) = 1.0
        _MainTex ("Main", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 80
        Pass
        {
            ZWrite Off
            Cull Off
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture [_MainTex]
            {
                constantColor (1,1,1,[_Emm])
                combine texture*constant
            }
        }
    }
}
