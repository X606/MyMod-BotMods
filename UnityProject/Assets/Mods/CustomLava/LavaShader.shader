// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/LavaShader"
{
    Properties
    {
        
        _Texture ("Lava Texture", 2D) = "white" {}
        [NoScaleOffset] _LavaColors ("Lava Colors", 2D) = "white" {}
        _Transparancy ("Transparancy", Range(0, 10)) = 1
        _LavaSpeed ("Lava Speed", Range(0,5)) = 1
        _LavaDownSpeed ("Lava Down Speed", Range(0,5)) = 1
        _Slope ("Lava Slope", Float) = 1
        _GrowStrength ("Glow Strength", Range(0,5)) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="AlphaTest" }

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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float4 screenuv : TEXCOORD2;
                float screenDepth : TEXCOORD3;
                float3 worldNormal : TEXCOORD4;
            };

            //sampler2D _Background;
            uniform sampler2D _CameraDepthTexture;
            sampler2D _Texture;
            float4 _Texture_ST;

            sampler2D _LavaColors;

            float _Transparancy;
            float _LavaSpeed;
            float _LavaDownSpeed;
            float _Slope;
            float _GrowStrength;

            float4 lerp3way(float4 col1, float val1, float4 col2, float val2, float4 col3, float val3) {
                return (col1*val1 + col2*val2 + col3*val3) / (val1 + val2 + val3);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                float yOffset = sin(worldPos.z/10 + _Time.y) * 0.1;

                float4 worldSpaceYOffset = mul( unity_WorldToObject, float4(0,yOffset,0,0));

                o.vertex = UnityObjectToClipPos(v.vertex + worldSpaceYOffset.xyz);

                o.worldNormal = mul( unity_ObjectToWorld, float4( v.normal, 0) );
                
                o.worldPos = worldPos + float4(0, yOffset, 0, 0);
                o.uv = v.uv;
                
                o.screenuv = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenDepth);

                
                //o.vertex = UnityObjectToClipPos(v.vertex + float3(0,yOffset,0));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float diff = 0;

                if (unity_OrthoParams.w == 0) // if camera is not orthographic
                {
                    float screenDepth = i.screenDepth;

                    float depth = DECODE_EYEDEPTH(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.screenuv.xy / i.screenuv.w));
                    
                    diff = depth - screenDepth;

                    bool isvalid = diff > -0.1;

                    diff = saturate(diff / _Transparancy);
                    diff = pow(diff, _Slope);
                    diff = 1 - diff;
                    
                    diff = diff * isvalid;

                    //diff = 1-(diff*0.5 + 0.5);

                }

                float downOffset = _Time.y/3 * _LavaDownSpeed;

                float2 uvXZ = i.worldPos.xz + float2(_Time.y/10 * _LavaSpeed,_Time.y/25 * _LavaSpeed);
                float2 uvZY = i.worldPos.xy + float2(0, downOffset);
                float2 uvYZ = i.worldPos.yz + float2(downOffset, 0);

                uvXZ = TRANSFORM_TEX(uvXZ, _Texture);
                uvZY = TRANSFORM_TEX(uvZY, _Texture);
                uvYZ = TRANSFORM_TEX(uvYZ, _Texture);

                float r = abs(dot(i.worldNormal, float3(0,1,0)));
                float g = abs(dot(i.worldNormal, float3(0,0,1)));
                float b = abs(dot(i.worldNormal, float3(1,0,0)));

                float lavaShapeXZ = tex2D(_Texture,  uvXZ).x;
                float lavaShapeZY = tex2D(_Texture,  uvZY).x;
                float lavaShapeYZ = tex2D(_Texture,  uvYZ).x;

                //return 

                float lavaShape = lerp3way(lavaShapeXZ, r, lavaShapeZY, g, lavaShapeYZ, b);
                
                //float mask = lavaShape;
                float mask = lavaShape - diff;

                //return diff;

                float4 lavaColor = tex2Dlod(_LavaColors, float4(lavaShape, 0, 0, 0));
                
                float3 outColor = (lavaColor * mask).rgb * _GrowStrength;

                return float4(outColor,1);

            }
            ENDCG
        }
    }
}
