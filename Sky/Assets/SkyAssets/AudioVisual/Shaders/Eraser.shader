Shader "Eraser" 
{
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
/*
    SubShader 
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        ZTest Off
        Blend One One
        BlendOp Max
        
        CGPROGRAM
        sampler2D _MainTex;      
 
        struct Input 
        {
            float2 uv_MainTex;
        };
 
        fixed4 _Color;
        
        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
*/
    FallBack "Diffuse"
}
