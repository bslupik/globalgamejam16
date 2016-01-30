Shader "ImageEffects/ScreenTearing"
{
//v = max(0,sign(distance - _Value))
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_EffectTex("Cutoff Map, Greyscale", 2D) = "white" {}
		_AbberationTex("Abberation Map, Greyscale", 2D) = "white" {}
		_Cutoff ("Cutoff", Range(0,0.25)) = 0.125
		_NumTears ("Tear Count", Float) = 0.05
		_NoiseSpeed ("Noise Speed", Range(0,1)) = 0.05
		_MinTearing ("Min Tearing", Range(0,1)) = 0.05
		_ChromaticAbberation ("Chromatic Abberation", Range(0,10)) = 0.05
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "IgnoreProjector"="True" "Queue"="Transparent"  "PreviewType"="Plane"}
		ZWrite Off
		Lighting Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _EffectTex;
			sampler2D _AbberationTex;
			half _Cutoff;
			fixed _NumTears;
			fixed _NoiseSpeed;
			fixed _MinTearing;
			fixed _ChromaticAbberation;

			
			fixed4 frag (v2f_img i) : SV_Target
			{
				half2 effectUV = i.uv;
				effectUV.x = _NoiseSpeed * _Time.gg;
				half distortionNoise = tex2D(_EffectTex, effectUV ).r;
				half distortionSign = sign(fmod(i.uv.y * _NumTears, 1) - 0.5); //returns a square wave, which we'll use for tear direction
				fixed tearingMagnitude =  _Cutoff * distortionSign * lerp(_MinTearing, 1, distortionNoise);
				fixed chromaticMagnitudeRed = _Cutoff * _ChromaticAbberation * tex2D(_AbberationTex, effectUV).r;
				effectUV.x += 0.5;
				fixed chromaticMagnitudeBlue = _Cutoff * _ChromaticAbberation * tex2D(_AbberationTex, effectUV).r;

				fixed chromaticMagnitude = tex2D(_EffectTex, effectUV).r;



				effectUV.x = i.uv.x + tearingMagnitude;
				fixed4 col = tex2D(_MainTex, effectUV);

				effectUV.x -= chromaticMagnitudeRed * chromaticMagnitude;
				col.r = tex2D(_MainTex, effectUV).r;

				effectUV.x += chromaticMagnitude * (chromaticMagnitudeRed + chromaticMagnitudeBlue);
				col.b = tex2D(_MainTex, effectUV).b;
							
				return col;
			}
			ENDCG
		}
	}
}