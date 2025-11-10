Shader "3DS/Fog"
{
    Properties
    {
        _Color ("Flat Color", Color) = (1,1,1,1)
        _DummyTex ("Dummy", 2D) = "white" {}

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 80
        Pass
        {
            ZWrite Off
            Cull Front
            Lighting Off
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture [_DummyTex]
            {
                constantColor [_Color]
                combine constant
            }
        }
    }
}
