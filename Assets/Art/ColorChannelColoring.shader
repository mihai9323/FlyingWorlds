Shader "Custom/ColorChannelColoring" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		
		_Color1("Channel1", Color) = (1,1,1,1)
		_Color2("Channel2", Color) = (1,1,1,1)
		_Color3("Channel3", Color) = (1,1,1,1)
		_Color4("Channel4", Color) = (1,1,1,1)
		_Color5("Channel5", Color) = (1,1,1,1)
		_Color6("Channel6", Color) = (1,1,1,1)
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		uniform sampler2D _MainTex;
	
			
		uniform fixed4 _Color1, _Color2, _Color3, _Color4,_Color5,_Color6;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			half4 fC = half4(0,0,0,0);
			if(c.r >0.5) fC+= _Color1 * (c.r * 2 - 1);
			else fC+= _Color2 * (c.r * 2);
			
			if(c.g >0.5) fC+= _Color3 * (c.g * 2 - 1);
			else fC+= _Color4 * (c.g * 2);
			
			if(c.b >0.5) fC+= _Color5 * (c.b * 2 - 1);
			else fC+= _Color6 * (c.b * 2);
			
			o.Albedo = fC.rgb;
			o.Alpha = fC.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
