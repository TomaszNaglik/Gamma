Shader "Custom/Map_HexShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Freq("Freq", float) = 1 
        _L("L", float) = 1
        _SeaLevel("Sea Level", float) = 1
        _MousePosition("MousePosition", Vector) = (0.5,0.5,0.5,0.5)
        _TargetCell("Cell", Vector) = (0.0,0.0,0.0,0.0)
        _NoiseFactor("NoiseFactor", float) = 0

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
        float _NoiseFactor;
        float2 _MousePosition;
        float2 _TargetCell;

        float N21(float2 p) {
            float n = frac(sin(p.x * 400. + p.y * 1756) * 534);

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
            smoothNoise += SmoothNoise(p * _L * 8.) * 0.225;
            smoothNoise += SmoothNoise(p * _L * 16.) * 0.1625;
            smoothNoise += SmoothNoise(p * _L * 30.) * 0.0825;
            return smoothNoise / 2.;
        }



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

            float n =  OctavedSmoothNoise(p-0.5) * _NoiseFactor;
            
            float2 a = (fmod(p+n*p,r)) - h;
            float2 b = (fmod(p+n*p + h, r)) - h;
            float2 gv = dot(a, a) < dot(b, b) ? a : b;

            float x = atan2(gv.x  ,gv.y);
            float y = 0.5 - HexDist(gv );


            float2 id = (p - gv)*float2(_Freq,_Freq*10./17.);
            return float4(y,y,id.x,id.y);
            
        }

        bool IsCell(float2 gv, float2 targetCell) {
            float threshold = 0.001;
            return abs(gv.x - targetCell.x) < threshold &&  abs(gv.y - targetCell.y) < threshold;
        }
            
        bool IsSameCell(float2 uv, float2 mp, float2 dimensions) {
            
            float threshold = 0.0001;
            mp = (mp * dimensions) / dimensions.y;
            float4 hc_uv = HexCoords(uv, _Freq) / _Freq;
            float4 hc_mp = HexCoords(mp, _Freq) / _Freq;

            return abs(hc_uv.z - hc_mp.z) < threshold && abs(hc_uv.w - hc_mp.w) < threshold;
        }
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 MapDimensions = float2(1000, 500);
            float2 uv = (IN.uv_MainTex.xy *MapDimensions) / MapDimensions.y;
            float3 col = float3(0, 0, 0);
            float3 highlight = float3(1, 0, 0);
            float4 hc = HexCoords(uv, _Freq) / _Freq;
            float2 gv = hc.xy;
            float2 id = hc.zw;
            float gridColor = smoothstep( 0.0000001, 0.0, hc.y *hc.x) * float3(1.,0.,0.);
            
            

            float smoothNoise = OctavedSmoothNoise(id/float2(_Freq, _Freq * 10. / 17.));
            smoothNoise = smoothstep(_SeaLevel, _SeaLevel + 0.0001, smoothNoise);
            float3 noise = float3(smoothNoise, smoothNoise, smoothNoise);
            
            float3 blue = float3(0, 0, 1);
            float3 green = float3(0, 1, 0);

            col = noise < 0.5 ? blue : green;
            //col = float3(id.x, id.y, 0);
            
            if (IsSameCell(uv, _MousePosition, MapDimensions)) {
                col = highlight;
            }

            if (IsCell(id, _TargetCell)) {
                col = float3(1, 1, 1);
            }
            
            //col += gridColor;
            o.Albedo =col;
  
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
