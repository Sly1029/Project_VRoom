Shader "Hidden/ScreenSpaceOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Threshold ("Threshold", Range(0,1.1)) = .5
		_OutlineColor ("OutlineColor", Color) = (1,1,1,1)
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _CameraGBufferTexture2;
			sampler2D _CameraDepthTexture;
			// This is a totally undocumented builtin that contains the texel dimensions for this texture!
			//  .xy = 1/size   .zw = size.
			fixed4 _MainTex_TexelSize;
			float _Threshold;
			float4 _OutlineColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 colOG = tex2D(_MainTex, i.uv);
				fixed4 col1 = tex2D(_CameraGBufferTexture2, i.uv + fixed2(-1 * _MainTex_TexelSize.x, 1 * _MainTex_TexelSize.y)); // left   column
				fixed4 col2 = tex2D(_CameraGBufferTexture2, i.uv + fixed2(-1 * _MainTex_TexelSize.x, 0 * _MainTex_TexelSize.y));
				fixed4 col3 = tex2D(_CameraGBufferTexture2, i.uv + fixed2(-1 * _MainTex_TexelSize.x,-1 * _MainTex_TexelSize.y));
				fixed4 col4 = tex2D(_CameraGBufferTexture2, i.uv + fixed2( 0 * _MainTex_TexelSize.x, 1 * _MainTex_TexelSize.y)); // middle column
				fixed4 col5 = tex2D(_CameraGBufferTexture2, i.uv + fixed2( 0 * _MainTex_TexelSize.x, 0 * _MainTex_TexelSize.y));
				fixed4 col6 = tex2D(_CameraGBufferTexture2, i.uv + fixed2( 0 * _MainTex_TexelSize.x,-1 * _MainTex_TexelSize.y));
				fixed4 col7 = tex2D(_CameraGBufferTexture2, i.uv + fixed2( 1 * _MainTex_TexelSize.x, 1 * _MainTex_TexelSize.y)); // right  column
				fixed4 col8 = tex2D(_CameraGBufferTexture2, i.uv + fixed2( 1 * _MainTex_TexelSize.x, 0 * _MainTex_TexelSize.y));
				fixed4 col9 = tex2D(_CameraGBufferTexture2, i.uv + fixed2( 1 * _MainTex_TexelSize.x,-1 * _MainTex_TexelSize.y));
				col1.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2(-1 * _MainTex_TexelSize.x, 1 * _MainTex_TexelSize.y)).r); // left   column
				col2.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2(-1 * _MainTex_TexelSize.x, 0 * _MainTex_TexelSize.y)).r);
				col3.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2(-1 * _MainTex_TexelSize.x,-1 * _MainTex_TexelSize.y)).r);
				col4.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2( 0 * _MainTex_TexelSize.x, 1 * _MainTex_TexelSize.y)).r); // middle column
				col5.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2( 0 * _MainTex_TexelSize.x, 0 * _MainTex_TexelSize.y)).r);
				col6.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2( 0 * _MainTex_TexelSize.x,-1 * _MainTex_TexelSize.y)).r);
				col7.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2( 1 * _MainTex_TexelSize.x, 1 * _MainTex_TexelSize.y)).r); // right  column
				col8.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2( 1 * _MainTex_TexelSize.x, 0 * _MainTex_TexelSize.y)).r);
				col9.a = log(tex2D(_CameraDepthTexture, i.uv + fixed2( 1 * _MainTex_TexelSize.x,-1 * _MainTex_TexelSize.y)).r);

				// Sobel Edge Detection Operator Kernel
				  // -1 0 1
				  // -2 0 2
				  // -1 0 1
				      
				// Grid Variables
				  // col1 col4 col7
				  // col2 col5 col8
				  // col3 col6 col9      

				fixed linearDepth = Linear01Depth(tex2Dproj(_CameraDepthTexture, float4(i.uv.x, i.uv.y, 1, 1) ));

				fixed4 verticalSobel   = col1 * -1 + col2 * -2 + col3 * -1 + col7 * 1 + col8 * 2 + col9 * 1;
				fixed4 horizontalSobel = col1 * -1 + col4 * -2 + col7 * -1 + col3 * 1 + col6 * 2 + col9 * 1;
				// I think the maximum value of the sobel operator output scalar is sqrt(32)
				fixed4 averageEdgeStrength = sqrt(verticalSobel * verticalSobel + horizontalSobel * horizontalSobel) / float4(sqrt(32),sqrt(32),sqrt(32),sqrt(32));
				fixed thresholdPass = step(_Threshold, averageEdgeStrength.x) + step(_Threshold, averageEdgeStrength.y) + step(_Threshold, averageEdgeStrength.z) + step(_Threshold, averageEdgeStrength.w);
				fixed4 col = _OutlineColor * thresholdPass * (1 - (linearDepth * linearDepth)) + colOG;
//				clip (col.b - .01);	
//				return tex2D(_CameraGBufferTexture2, i.uv);
				//return (1-linearDepth) * float4(1,1,1,1);
				return col;
			}
			ENDCG
		}
	}
}