Shader "3DS/Emissive"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Emission", Float) = 1.0
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
                combine texture * previous
            }
            SetTexture [_MainTex]
            {
                constantColor ([_Color],[_Color],[_Color],1)
                combine constant * previous
            }
        }
    }
}