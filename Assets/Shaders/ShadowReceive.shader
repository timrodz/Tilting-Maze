Shader "timrodz/Unlit Shadow Receive" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
	}
	
 	SubShader 
	{
    	Tags 
		{ 
			"RenderType"="Opaque" 
		}
		
		LOD 100

		CGPROGRAM
		#pragma surface surf Lambert

		fixed4 _Color;

		struct Input 
		{
			float4 color : COLOR;
		};
	
		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = _Color.rgb;
		}
		ENDCG
	}
	
	Fallback "Transparent/Cutout/VertexLit"
}