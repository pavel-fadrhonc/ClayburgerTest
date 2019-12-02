Shader "Custom/Wavy"
{
    Properties
    {
        _Color("Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Range(0.0, 0.2)) = 0.05
        _Speed ("Speed", Range(0.0, 300.0)) = 100
        _Frequency ("Frequency", Range(0.0, 100)) = 1
        _CutoffXMin ("_CutoffXMin", Range (0,1)) = 0
        _CutoffXMax ("_CutoffXMax", Range (0,1)) = 1
        _Fade ("_Fade", Range(0,1)) = 0
        _WaveOffset ("WaveOffset", Range(0,1)) = 0
        
        _Chaos ("Chaos", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "PreviewType"="Plane"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amplitude;
            float _Speed;
            float _Frequency;
            float _CutoffXMin;
            float _CutoffXMax;
            float _Fade;
            float _WaveOffset;
            float _Chaos;
            
            ///  keijiro/prng.cginc
            float nrand(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                o.vertex.x += o.uv.y * ( + sin((_Time.x* _Frequency + (_WaveOffset) + (step(0,_Chaos) * nrand(o.uv) * _Chaos))) *_Amplitude); 
                //o.vertex.x += o.uv.y * ( + sin(_Time.x + (_WaveOffset) * _Frequency + (* _Frequency + + (step(0,_Chaos) * nrand(o.uv) * _Chaos)) ) *_Amplitude); 
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= step(_CutoffXMin, i.uv.x) * (1 - step(_CutoffXMax, i.uv.x));
                col.a *= clamp((1 - lerp(0,1, (_Fade - i.uv.y) / _Fade)), 0, 1);
                return col * _Color;
            }
            ENDCG
        }
    }
}