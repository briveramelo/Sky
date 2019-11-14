Shader "MaskConstruction" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _EraserTex ("Eraser Texture", 2D) = "white" {}
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        ZTest Off
        Blend One One
        BlendOp Max
        //Cull Off
        Pass {
            Lighting Off
            SetTexture[_EraserTex] {
                ConstantColor [_Color]
                combine texture * constant
            }
        }
    }
}