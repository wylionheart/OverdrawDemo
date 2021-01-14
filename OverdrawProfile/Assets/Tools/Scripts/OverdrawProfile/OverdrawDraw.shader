Shader "OverdrawProfile/OverdrawDraw"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
		
        Blend One One
        ZClip False
		ZTest Always
		ZWrite On
		Cull Back
        //Fog { Color (0,0,0,0) }
		//Fog{ Mode Off }
		Pass
		{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			

			struct appdata_t 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
		
			struct v2f
			{
				float4 pos : SV_POSITION;
				//fixed4 color : COLOR;
				//half2 uv1 : TEXCOORD0;
			};
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				//o.uv1 = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}
		   

			half4 frag(v2f  i ) : COLOR
			{	
				
				return  half4(2/255.0f,2/255.0f,2/255.0f,1);
			}	

			ENDCG
		}
    }

    Fallback "Diffuse"
}
