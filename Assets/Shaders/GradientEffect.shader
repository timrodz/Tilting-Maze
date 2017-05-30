Shader "FX/Gradient Effect"
{
	Properties
	{
		_ColorTop("Top color", Color) = (1,1,1,1)
		_ColorBot("Bottom color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags 
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
		// No culling or depth
		// Cull Off ZWrite Off ZTest Always

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _ColorTop;
			fixed4 _ColorBot;
			half _Value;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = lerp(_ColorBot, _ColorTop, v.uv.y);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float4 col;
				col = tex2D(_MainTex, i.uv);
				col.rgb *= i.color.rgb;
				// col.a = i.color.a;
				// fixed4 col = tex2D(_MainTex, i.uv);
				// col *= lerp(_ColorBot, _ColorTop, _Time.x);
				// col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
