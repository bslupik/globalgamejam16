Shader "Custom/DissolveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ImageTex ("Texture", 2D) = "white" {}
		_Cutoff("Cutoff", float) = 0.5
		_CutoffColorRange("Cutoff Color Range", Range(0, 0.1)) = 0.05
		_EffectColor("Dissolve Color", Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent+1"  "PreviewType"="Plane"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		Lighting Off

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

			sampler2D _MainTex;
			sampler2D _ImageTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			fixed _CutoffColorRange;
			fixed4 _EffectColor;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);		
				fixed4 img = tex2D(_ImageTex, i.uv);
				col.rgb = lerp(_EffectColor.rgb, col.rgb, step(img.r, _Cutoff - _CutoffColorRange));
				col.a *= step(img.r, _Cutoff);

				return col;
			}
			ENDCG
		}
	}
}