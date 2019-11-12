Shader "MaskConstruction" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        ZTest Off
        Blend One One
        BlendOp Max
        Pass {
            Lighting Off
            SetTexture[_MainTex] {
                matrix [_UVMatrix]//todo: fix broken line. Eraser appears as full, 100% opaque square. Replace this deprecated+removed "fixed function" with something still available in Unity
                ConstantColor [_Color]
                combine texture * constant
            }
        }
    }
}