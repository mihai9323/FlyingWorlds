
Shader "Custom/ColorChannelColoring" 
{
	Properties
	{
		
		
		
		[PerRendererData]_MainTex ("Base (RGB)", 2D) = "white" {}
			_Color ("Tint", Color) = (1,1,1,1)
		_Color1("Channel1", Color) = (1,1,1,1)
		_Color2("Channel2", Color) = (1,1,1,1)
		_Color3("Channel3", Color) = (1,1,1,1)
		_Color4("Channel4", Color) = (1,1,1,1)
		_Color5("Channel5", Color) = (1,1,1,1)
		_Color6("Channel6", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile DUMMY PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
			};
			
			uniform sampler2D _MainTex;
	
			
		uniform fixed4 _Color,_Color1, _Color2, _Color3, _Color4,_Color5,_Color6;
		
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
			
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 c = tex2D (_MainTex, IN.texcoord)* IN.color;
			
				half4 fC = c;
				//fC= lerp(fC,_Color1,((c.r) *c.a));
				//fC= lerp(fC,_Color2,((c.g) *c.a));
				//fC= lerp(fC,_Color3,((c.b) *c.a));
				
					if(c.r >0.5) fC= lerp(fC,_Color1,((c.r * 2 - 1) *c.a));
					else fC= lerp(fC,_Color2,((c.r * 2) *c.a));
					
					if(c.g >0.5) fC= lerp(fC,_Color3,((c.g * 2 - 1) *c.a));
					else fC= lerp(fC,_Color4,((c.g * 2) *c.a));
					
					if(c.b >0.5) fC= lerp(fC,_Color5,((c.b * 2 - 1) *c.a));
					else fC= lerp(fC,_Color6,((c.b * 2) *c.a));
					
				fC.rgb*=c.a;
				
				return fC;
			}
		ENDCG
		}
	}
}
