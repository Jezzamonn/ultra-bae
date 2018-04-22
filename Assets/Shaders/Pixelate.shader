// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Pixelate" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Sizes ("Sizes", Vector) = (1, 4, 1024, 768)

        _Color_0 ("Color 0", Color) = (.094, .188, .188, 1)
        _Color_1 ("Color 1", Color) = (.314, .471, .408, 1)
        _Color_2 ("Color 2", Color) = (.659, .753, .690, 1)
        _Color_3 ("Color 3", Color) = (.878, .941, .910, 1)
    }
    SubShader {
        Pass {
        
            CGPROGRAM
            
            // target shader model 3.0, to increase the number of available instructions, registers, etc.
            #pragma target 3.0
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            // input uniforms
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Sizes;
            float4 _Color_0;
            float4 _Color_1;
            float4 _Color_2;
            float4 _Color_3;
            
            // vertex program input
            struct a2v
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
            }; 
            
            // vertex program output, fragment program input
            struct v2f
            {
                float4 pos : POSITION;
                float2  uv : TEXCOORD0;
            };
            
            // vertex program
            v2f vert (a2v  v)
            {
                v2f o;
                
                o.pos = UnityObjectToClipPos (v.vertex);
                o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);  
                return o;
            }
            
            float4 frag (v2f i) : COLOR
            {
                int x = i.uv.x * _Sizes[2];
                int y = i.uv.y * _Sizes[3];
                
                bool pixel_border = (_Sizes[0] > 0.5);
                int zoomLevel = int(_Sizes[1]);
                
                int offsetx = (x % zoomLevel);
                int offsety = (y % zoomLevel);

                int gridx = x / zoomLevel;
                int gridy = x / zoomLevel;
                
                int newx = x - offsetx;
                int newy = y - offsety;
                
                float2 sizes = float2(
                    newx / _Sizes[2],
                    newy / _Sizes[3]
                );
                
                float4 border_offset = float4(.03, .03, .03, 0);
                
                float4 fragcolor = tex2D(_MainTex, float2((float(newx) + 0.5 * zoomLevel) / _Sizes[2], (float(newy) + 0.5 * zoomLevel) / _Sizes[3]));

                if (fragcolor.r > 0.7 && fragcolor.g < 0.3) {
                    // you're free to pass through
                }
                else {
                    float gray = dot(fragcolor.rgb, float3(0.299, 0.587, 0.114));
                    float grayRounded = round(10 * gray + 0.3 * (gridx % 2)) / 10;
                    fragcolor.r = 0.8 * grayRounded;
                    fragcolor.g = 1 * grayRounded;
                    fragcolor.b = 0.6 * grayRounded + 0.1;
               }

                return fragcolor;
            }
        
            ENDCG
        }
    } 
    FallBack Off
}