﻿Shader "Custom/SceneChangeShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Cutoff", Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  "IgnoreProjector"="True" "Queue"="Transparent"  "PreviewType"="Plane"}
		ZWrite Off
		Lighting Off
		LOD 100

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
			float4 _MainTex_ST;
			fixed _Cutoff;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				col = lerp(col, fixed4(0,0,0,0), _Cutoff);

				return col;
			}
			ENDCG
		}
	}
}
