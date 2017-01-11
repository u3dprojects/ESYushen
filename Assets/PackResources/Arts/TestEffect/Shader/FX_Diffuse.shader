Shader "YuShen/FX Diffuse"
 {
	Properties {
		_MainTex ("Base (RGBA)", 2D) = "white" {}
		_DiffuseColor("Diffuse Color",Color ) = (1,1,1,1)
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" }

		Fog {Mode off}
		//Blend SrcAlpha OneMinusSrcAlpha
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform float4 _DiffuseColor;
			
			struct appdata
			{
			    float4 vertex    : POSITION;
				float4 color	 : COLOR;
			    float2 texcoord  : TEXCOORD0;
			    
			};
	
			struct v2f
			{
			    float4 pos 		: SV_POSITION;
			    half2  tex1 	: TEXCOORD0;
				half4  color	: TEXCOORD1;
			};

			v2f vert(appdata v)
			{
			    v2f o;
			    o.pos       = mul( UNITY_MATRIX_MVP, v.vertex );
			    o.tex1.xy   = v.texcoord.xy;
			    o.color		= v.color;
			    return o;
			}
	
			sampler2D _MainTex;
			
			fixed4 frag( v2f ps ) : COLOR
			{
			   fixed4 cBase = tex2D( _MainTex , ps.tex1 );
			   fixed4 vColor;
		       vColor.rgba = cBase * _DiffuseColor  * ps.color;
	   	 	   return vColor;
			}
			
			ENDCG
		}
	  }
	  FallBack "Diffuse"
} 
