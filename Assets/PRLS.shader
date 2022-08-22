Shader "Custom/PRLS"{
    Properties{
        _Color("_Color",Color) = (1,1,1,1)//The color of the object itself
        _SpecularColor("_SpecularColor",Color)=(1,1,1,1)//Highlight color
        _Gloss("Gloss",Range(8,200)) = 10//Control the size of the highlight aperture
    }
    SubShader{
        Pass{
            //Define LightMode to get unity's built-in lighting variables
            Tags{"LightMode" = "ForwardBase"}

            CGPROGRAM
            //Unity built-in files are introduced, and unity's built-in variables should be used
            #include "Lighting.cginc"
            #pragma vertex vert
            #pragma fragment frag

            fixed4 _Color;
            half _Gloss;
            fixed4 _SpecularColor;

            struct a2v {
                float4 vertex: POSITION;
                float3 normal: NORMAL;
            };
            struct v2f {
                float4 position: SV_POSITION;
                fixed3 color: COLOR;
            };
            v2f vert(a2v v) {
                v2f f;
                //UNITY_MATRIX_MVP coordinates are converted from model space to clipping space
                f.position = UnityObjectToClipPos(v.vertex);
                //The unit vector of the normal: _World2Object turns a direction from world space to model space
                fixed3 normalDir = normalize(mul(v.normal, (float3x3) unity_WorldToObject));
                //Light unit vector: For each vertex, the position of the parallel light is the direction 
                //of the light_WorldSpaceLightPos0 to obtain the position of the parallel light
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                //Get the diffuse color
                fixed3 diffuse = _LightColor0.rgb * max(dot(normalDir, lightDir), 0) * _Color.rgb;
                //The direction from the current point to the camera
                fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz-mul(v.vertex, unity_WorldToObject).xyz);
                //High light reflection

                fixed3 halfDir = normalize(viewDir+lightDir);

                fixed3 specular = _LightColor0.rgb * pow(max(dot(normalDir, halfDir), 0), _Gloss) *_SpecularColor;
                //Ambient light
                fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
                //Color overlay
                f.color = diffuse + specular + ambient;
                return f;
            }
            fixed4 frag(v2f f): SV_Target{
                return fixed4(f.color,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}