Shader "Custom/Map_HexShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Freq("Freq", float) = 1 
        _L("L", float) = 1
        _SeaLevel("Sea Level", float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        fixed4 _Color;
        sampler2D _MainTex;
        struct Input
        {
            float2 uv_MainTex;
        };

        float _Freq;
        float _L;
        float _SeaLevel;

        float HexDist(float2 p)
        {
            p = abs(p);
            float c = dot(p, normalize(float2(1, 1.73)));
            c = max(c, p.x);
            return c;
        }

        float4 HexCoords(float2 p, float f)
        {
            p *= f; 
            float2 r = float2(1., 1.7);
            float2 h = r*0.5;
            
            float2 a = (fmod(p,r)) - h;
            float2 b = (fmod(p + h, r)) - h;
            float2 gv = dot(a, a) < dot(b, b) ? a : b;

            float x = atan2(gv.x,gv.y);
            float y = 0.5 - HexDist(gv);


            float2 id = p - gv;
            return float4(y,y,id.x,id.y);
            
        }

        float N21(float2 p) {
            float n = frac(sin(p.x * 400. + p .y * 1756) * 534);

            return n;
        }

        float SmoothNoise(float2 p) {
            float2 lv = frac(p);
            float2 id = floor(p);

            lv = lv * lv * (3. - 2. * lv);
            float bl = N21(id);
            float br = N21(id + float2(1, 0));
            float tl = N21(id + float2(0, 1));
            float b = lerp(bl, br, lv.x);

            float tr = N21(id + float2(1, 1));
            float t = lerp(tl, tr, lv.x);

            return lerp(b, t, lv.y);
        }

        float OctavedSmoothNoise(float2 p) {
            float smoothNoise = SmoothNoise(p * _L);
            smoothNoise += SmoothNoise(p * _L * 2.) * 0.5;
            smoothNoise += SmoothNoise(p * _L * 4.) * 0.25;
            smoothNoise += SmoothNoise(p * _L * 8.) * 0.125;
            smoothNoise += SmoothNoise(p * _L * 16.) * 0.0625;
            smoothNoise += SmoothNoise(p * _L * 32.) * 0.0325;
            return smoothNoise / 2.;
        }
            

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 MapDimensions = float2(1000, 500);
            float2 uv = (IN.uv_MainTex.xy*MapDimensions)/MapDimensions.y;
            float3 col = float3(0, 0, 0);
            
            float4 hc = HexCoords(uv, _Freq) / _Freq;
            float2 gv = hc.xy;
            float2 id = hc.zw;
            float gridColor = smoothstep(0.0, 0.0000001, hc.y *hc.x) * float3(1.,0.,0.);
            col += gridColor;
            

            float smoothNoise = OctavedSmoothNoise(id);
            smoothNoise = smoothstep(_SeaLevel, _SeaLevel + 0.0001, smoothNoise);
            float3 noise = float3(smoothNoise, smoothNoise, smoothNoise);
            
            col = noise;
            col += gridColor;
            o.Albedo =col;
  
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
