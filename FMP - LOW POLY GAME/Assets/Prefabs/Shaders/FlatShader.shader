Shader "Custom/FlatShader"{
	Properties{
		_MainTex("Texture",       2D) = "green" {}
		_BlendNormal("Blend Normals", Range(0,1)) = 0.5
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
			LOD 100

			Pass{
			CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"

			struct appdata {
			float4 vertex : POSITION;
			float2 uv     : TEXCOORD0;
			float3 normal : NORMAL;
			float3 color  : COLOR0;
		};

		struct v2f {
			float4 vertex   : SV_POSITION;
			float2 uv       : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
			float3 normal   : NORMAL;
			float3 color    : COLOR0;
		};

		sampler2D _MainTex;
		float4    _MainTex_ST;
		float     _BlendNormal;

		v2f vert(appdata v) {
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.normal = UnityObjectToWorldNormal(v.normal);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			o.color = v.color;
			return o;
		}

		float3 GetFaceNormal(float3 position) {
			float3 dx = ddx(position);
			float3 dy = ddy(position);
			return normalize(cross(dy, dx));
		}

		float3 GetLighting(float3 normal) {
			float3 light = saturate(dot(normal, _WorldSpaceLightPos0.xyz)) * _LightColor0;
			light += ShadeSH9(float4(normal, 0.25)).rgb;
			return light;
		}

		fixed4 frag(v2f i) : SV_Target{
			fixed4 color = tex2D(_MainTex, i.uv);

		float3 faceNormal = GetFaceNormal(i.worldPos);
		float3 finalNormal = lerp(i.normal, faceNormal, _BlendNormal * i.color.r);

		color.rgb *= GetLighting(finalNormal);
		return color;
		}
		ENDCG
		}
	}
}
