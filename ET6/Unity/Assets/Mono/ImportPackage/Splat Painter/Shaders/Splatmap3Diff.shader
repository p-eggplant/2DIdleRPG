Shader "Splatmap/Splatmap3Diff"
{
	Properties
	{
		_SplatMap01("Texture", 2D) = "white" {}
	_Albedo01("Albedo 01", 2D) = "white" {}
	_Albedo02("Albedo 02", 2D) = "white" {}
	_Albedo03("Albedo 03", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _SplatMap01;
	float4 _SplatMap01_ST;
	sampler2D _Albedo01, _Albedo02, _Albedo03;
	float4 _Albedo01_ST, _Albedo02_ST, _Albedo03_ST;

	fixed4 frag(v2f i) : SV_Target
	{

		fixed4 splatMap01 = tex2D(_SplatMap01, TRANSFORM_TEX(i.uv, _SplatMap01)).rgba;

	half2 uv = TRANSFORM_TEX(i.uv, _Albedo01);
	fixed4 albedo01 = tex2D(_Albedo01, uv).rgba;
	fixed4 albedo02 = tex2D(_Albedo02, uv).rgba;
	fixed4 albedo03 = tex2D(_Albedo03, uv).rgba;

	fixed4 Color = splatMap01.r * albedo01.rgba
		+ splatMap01.g * albedo02.rgba
		+ splatMap01.b * albedo03.rgba;


	return  Color;
	}
		ENDCG
	}
	}
}