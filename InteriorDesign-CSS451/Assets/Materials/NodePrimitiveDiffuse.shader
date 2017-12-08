Shader "Lit/NodePrimitiveDiffuse"
{
	Properties
	{
		// we have removed support for texture tiling/offset,
		// so make them not be displayed in material inspector
		_MainTex("Texture", 2D) = "white" {}
		MyColor("MyColor", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		//Tgs{ "RenderType" = "Opaque" }
		Tags{ "LightMode" = "ForwardBase" }
		LOD 100

		Pass
		{
		CGPROGRAM
		// use "vert" function as the vertex shader
		#pragma vertex vert
				// use "frag" function as the pixel (fragment) shader
		#pragma fragment frag
		#include "UnityCG.cginc" // for UnityObjectToWorldNormal
		#include "UnityLightingCommon.cginc" // for _LightColor0

		// vertex shader inputs
		struct appdata
		{
			float4 vertex : POSITION; // vertex position
			float2 uv : TEXCOORD0; // texture coordinate
		};

		// vertex shader outputs ("vertex to fragment")
		struct v2f
		{
			float2 uv : TEXCOORD0; // texture coordinate
			fixed4 diff : COLOR0; // diffuse lighting color
			float4 vertex : SV_POSITION; // clip space position
		};

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4x4 MyXformMat;
		fixed4 MyColor;

		// vertex shader
		v2f vert(appdata_base v)
		{
			v2f o;
			UNITY_INITIALIZE_OUTPUT(v2f, o);

			v2f interm;			//intermediate - we want to transform the vertex first
								//o.vertex = UnityObjectToClipPos(v.vertex);
			o.vertex = mul(MyXformMat, v.vertex);
			o.vertex = mul(UNITY_MATRIX_VP, o.vertex);

			// get vertex normal in world space
			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			// dot product between normal and light direction for
			// standard diffuse (Lambert) lighting
			half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
			// factor in the light color
			o.diff = nl * _LightColor0;

			//o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;


		}

		// pixel shader; returns low precision ("fixed4" type)
		// color ("SV_Target" semantic)

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = MyColor;
			col *= i.diff;
			return col;
		}
		ENDCG
	}
	}
}