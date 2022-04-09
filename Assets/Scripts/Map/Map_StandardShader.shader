Shader "Custom/Map_StandardShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MousePosition("MousePosition", Vector) = (0.5,0.5,0.5,0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        float2 _MousePosition;
        fixed4 _Color;

        float dist(float2 a, float2 b) {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return sqrt(dx * dx + dy * dy);
        }

        float distanceToMouse(float2 uv, float2 mp, float circleSize) {
            
            uv.x *= 2;
            uv.x -= 0.5;
            
            mp.x *= 2;
            mp.x -= 0.5;

            float distance = dist(mp, uv);
            float smooth = smoothstep(circleSize, circleSize * 0.5, distance);
            return smooth;
        }

        float grid(float2 gv) {

            gv = frac(gv);
            gv -= 0.5;
            gv = abs(gv);
            gv = smoothstep(0.46, 0.49, gv);

            return gv.x + gv.y;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex.xy;
            float2 MapDimensions = float2(1000, 500);
            float2 gv = uv * MapDimensions;

            float DistanceToMouse = distanceToMouse(uv, _MousePosition, 0.03);
            float Grid = grid(gv);
            
            float3 finalColor=0;
            finalColor = min(Grid,DistanceToMouse);
            
            fixed4 c = float4(finalColor,1);
            o.Albedo = c.rgb;

            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
