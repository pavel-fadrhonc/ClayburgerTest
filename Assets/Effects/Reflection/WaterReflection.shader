Shader "Custom/WaterReflection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BumpMap ("BumpMap", 2D) = "bump" {}
        _BumpAmount("BumpAmount", Range(0,0.1)) = 0.05
    }
    SubShader
    {
    
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent" 
            
        }
        GrabPass { "_UnderWaterTex" }
        Blend One OneMinusSrcAlpha

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
                float2 uvbump : TEXCOORD1;
	            float4 uvgrab : TEXCOORD2;                    
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _BumpMap;
            sampler2D _UnderWaterTex;
            fixed _BumpAmount;
            float4 _MainTex_ST;
            float4 _BumpMap_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uvbump = TRANSFORM_TEX(v.uv, _BumpMap);
            #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
            #else
                float scale = 1.0;
            #endif
                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {     
                half4 bump = tex2D(_BumpMap, i.uvbump);
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed alpha = col.a;
                
                half2 distortion = UnpackNormal(bump).rg;
                i.uvgrab.xy += distortion * _BumpAmount;
                i.uv.xy += distortion * _BumpAmount;
                col = tex2D(_MainTex, i.uv);
                
                half4 colGrab = tex2Dproj(_UnderWaterTex, UNITY_PROJ_COORD(i.uvgrab));
                
                col *= colGrab;
                //col *= UNITY_LIGHTMODEL_AMBIENT.rgb;
                col.a = alpha;
                
                return col;
            }
            ENDCG
        }
    }
}