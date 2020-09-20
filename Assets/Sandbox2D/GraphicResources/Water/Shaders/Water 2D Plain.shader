// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Water/Water 2D Plain"
{
	Properties 
	{
		_Color ("Color", Color) = (1, 1, 1, 0.5)
		[Normal] _Refraction ("Refraction", 2D) = "bump" {}
		_Intensity ("Refraction Intensity", Float) = 0.02
		_Current ("Current Speed", Float) = -0.25
	}
	SubShader 
	{
		Tags 
		{
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		GrabPass { }
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile __ DISABLE_REFRACTION
			
			#include "UnityCG.cginc"
			#include "Water2D.cginc"
			
			struct appdata_t 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR;
			};
			
			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float4 screenPos : TEXCOORD5;
				fixed4 color : COLOR;
			};
			
			v2f vert (appdata_t IN)
			{
				v2f OUT;
				OUT.texcoord = IN.texcoord;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.screenPos = OUT.vertex;
				
				OUT.color = IN.color;
				
				return OUT;
			}
			
			fixed4 frag(v2f IN) : SV_Target
			{
				IN.texcoord.x += _Current * _Time.y;
			
				fixed4 texColor = IN.color;
			
			#if DISABLE_REFRACTION
				fixed4 c = texColor * _Color;
			#else
				fixed4 sceneColor = refraction(_Refraction, _Refraction_ST, IN.texcoord, IN.screenPos, _Intensity);
				fixed4 c = fixed4(lerp(sceneColor.rgb, texColor.rgb * _Color.rgb, _Color.a), texColor.a);
			#endif
				
				c.rgb *= c.a;
				
				return c;
			}
			ENDCG
		}
	}
}
