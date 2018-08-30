Shader "Tutorials/Shader_Tutorials"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		/*_Center("Center",Vector) = (0,0,0,0)
		_Radius("Radius",Float) = 0.5*/

		//_Amount("Extrusion Amount",Range(0,1)) = 0
	}

	SubShader
	{
		Tags{"RenderType" = "Opaque"}

		CGPROGRAM
		//#pragma surface surf Lambert vertex:vert

		#pragma surface surf SimpleLambert

		struct Input
		{
			float2 uv_MainTex;

			//float3 worldPos;
		};

		sampler2D _MainTex;

		/*float3 _Center;
		float _Radius;*/

		//float _Amount;

		/*void vert(inout appdata_full v)
		{
			v.vertex.xyz += v.normal*_Amount;
		}*/

		void surf(Input IN, inout SurfaceOutput o)
		{
			/*float d = distance(_Center, IN.worldPos);
			float dN = 1 - saturate(d / _Radius);

			if ((dN > 0.25) && (dN < 0.3))o.Albedo = half3(1, 1, 1);
			else o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;*/

			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDirec, half attenuat)
		{
			half NdotL = dot(s.Normal, lightDirec);
			half4 c;
			c.rgb = s.Albedo*_LightColor0.rgb*(NdotL*attenuat);
			c.a = s.Alpha;

			return c;
		}

		ENDCG
	}

	Fallback "Diffuse"
	//SubShader
	//{
	//	Tags { "RenderType"="Opaque" }
	//	LOD 100

	//	Pass
	//	{
	//		CGPROGRAM
	//		#pragma vertex vert
	//		#pragma fragment frag
	//		// make fog work
	//		#pragma multi_compile_fog
	//		
	//		#include "UnityCG.cginc"

	//		struct appdata
	//		{
	//			float4 vertex : POSITION;
	//			float2 uv : TEXCOORD0;
	//		};

	//		struct v2f
	//		{
	//			float2 uv : TEXCOORD0;
	//			UNITY_FOG_COORDS(1)
	//			float4 vertex : SV_POSITION;
	//		};

	//		sampler2D _MainTex;
	//		float4 _MainTex_ST;
	//		
	//		v2f vert (appdata v)
	//		{
	//			v2f o;
	//			o.vertex = UnityObjectToClipPos(v.vertex);
	//			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	//			UNITY_TRANSFER_FOG(o,o.vertex);
	//			return o;
	//		}
	//		
	//		fixed4 frag (v2f i) : SV_Target
	//		{
	//			// sample the texture
	//			fixed4 col = tex2D(_MainTex, i.uv);
	//			// apply fog
	//			UNITY_APPLY_FOG(i.fogCoord, col);
	//			return col;
	//		}
	//		ENDCG
	//	}
	//}
}
