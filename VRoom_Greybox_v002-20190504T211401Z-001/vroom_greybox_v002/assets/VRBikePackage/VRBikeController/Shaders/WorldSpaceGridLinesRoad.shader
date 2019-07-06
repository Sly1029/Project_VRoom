Shader "Custom/WorldSpaceGridLinesRoad" {
	Properties {
		_GridColor("Grid Color", Color) = (1,1,1,1)
		_BaseColor("Base Color", Color) = (0,0,0,0)
		_MainTex ("Albedo (RGB)", 2D) = "black" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Threshold ("Threshold", Range(0,1)) = .35
		_GridThickness ("Grid Thickness", float) = .03
		_GridSize ("Grid Size", float) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		Cull Off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 worldNormal;
		};

		float4 _GridColor;
		float4 _BaseColor;
		float _GridThickness;
		float _GridSize;
		half _Glossiness;
		half _Metallic;
		float _Threshold;
		float cellWidth = .2;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
//			float4 m = tex2D (_MainTex, IN.uv_MainTex);
//			float4 m1 = tex2D (_MainTex, float2(1,1) - IN.uv_MainTex);

			// calculate worldspace grids
			fixed3 cellularPosition = fmod(abs(IN.worldPos), _GridSize);
			fixed xGrid = step(cellularPosition.x,_GridThickness);
			fixed yGrid = step(cellularPosition.y,_GridThickness) * (1-IN.worldNormal.y);
			fixed zGrid = step(cellularPosition.z,_GridThickness);
			fixed grid = saturate(xGrid + yGrid + zGrid);
			fixed3 grid3 = fixed3 (grid, grid, grid) * _GridColor;


			// Interfering the input texture with itself to create random colored windows
			//   (given random noise scaled up some)
//			fixed interference = m * m1;
//			interference *= step(_Threshold, interference);
			fixed4 c = _BaseColor;
			o.Albedo = c;


			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			// make the gridlines, the lights, and the edge of the wave emissive
			o.Emission = grid3;
//			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
