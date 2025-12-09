Shader "3DS/Portal" 
{
    Properties 
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _OutlineTex ("Outline Texture (Alpha)", 2D) = "white" {}
        _MaskTex ("Mask Texture (Alpha)", 2D) = "white" {}
    }
    SubShader 
    {
        Tags 
        { 
            "Queue"="Transparent" "RenderType"="Transparent" 
        }

        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass 
        {
            SetTexture [_MainTex] 
            {
                combine texture
            }
            SetTexture [_OutlineTex] 
            {
                combine previous + texture, previous
            }
            SetTexture [_MaskTex] 
            {
                combine previous, texture
            }
        }
    }
}
