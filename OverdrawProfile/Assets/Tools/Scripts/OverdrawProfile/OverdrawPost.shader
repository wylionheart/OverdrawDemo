Shader "OverdrawProfile//OverdrawPost"
{
	Properties
    {
        _OverDrawTexBlit("Source", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        //Blend One One
        ZClip False
		ZTest LEqual
		ZWrite On
		Cull Back
        //Fog { Color (0,0,0,0) }
		//Fog{ Mode Off }
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _OverDrawTex;
			float4 _OverDrawTex_ST;

			struct appdata_t 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
		
			struct v2f
			{
				float4 pos : SV_POSITION;
				//fixed4 color : COLOR;
				half2 uv1 : TEXCOORD0;
			};
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (half3(v.vertex.x,v.vertex.y,v.vertex.z));
				o.pos.y = o.pos.y;
				o.pos.x = o.pos.x;
				o.uv1 = TRANSFORM_TEX (v.texcoord, _OverDrawTex);
				return o;
			}
		   

			half4 frag(v2f  i ) : COLOR
			{	
				//return  half4(0,1,0,1);
				half4 col = tex2D(_OverDrawTex,i.uv1);
				if(col.r > 100/255.0f)
				{
					if(col.r > 150/255.0f)
						col = half4(1,0,0,1);
					else
						col = half4(0,0,1,1);
					
				}
				
				return  col;
			}	

			ENDCG
		}

    }

    Fallback "Diffuse"
}
