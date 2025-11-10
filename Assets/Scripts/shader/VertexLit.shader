Shader "3DS/VertexLit"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("_MainTex", 2D) = "white" {}
        _Emission ("Emission", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 80
        Pass
        {
            Material
            {
                Diffuse [_Color]
                Ambient [_Color]
            }
            Lighting On
            SetTexture [_Emission]
            {
                combine texture + primary
            }
            SetTexture [_MainTex]
            {
                combine texture * previous
            }
        }
    }
}