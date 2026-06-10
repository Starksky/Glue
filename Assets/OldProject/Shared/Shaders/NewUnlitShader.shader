Shader "Custom/SoftBodyMatrixDeform"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // Режим деформации: 0=SimpleScale, 1=Affine, 2=PerVertex
        _DeformMode ("Deform Mode", Float) = 0
        
        // Для SimpleScale
        _DeformScale ("Scale (XY)", Vector) = (1,1,0,0)
        
        // Для PerVertex
        _Vertex0 ("Vertex 0 Offset", Vector) = (0,0,0,0)
        _Vertex1 ("Vertex 1 Offset", Vector) = (0,0,0,0)
        _Vertex2 ("Vertex 2 Offset", Vector) = (0,0,0,0)
        _Vertex3 ("Vertex 3 Offset", Vector) = (0,0,0,0)
        
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        
        Pass
        {
            Name "DeformPass"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_local PIXELSNAP_ON
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _RendererColor;
            
            // Параметры деформации
            float _DeformMode;
            float2 _DeformScale;
            float4x4 _DeformMatrix;
            float4 _Vertex0;
            float4 _Vertex1;
            float4 _Vertex2;
            float4 _Vertex3;
            
            // Исходные позиции вершин в локальном пространстве для PerVertex режима
            // Порядок: нижний левый, нижний правый, верхний левый, верхний правый
            static const float3 originalCorners[4] = {
                float3(-0.5, -0.5, 0),
                float3( 0.5, -0.5, 0),
                float3(-0.5,  0.5, 0),
                float3( 0.5,  0.5, 0)
            };
            
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                
                float4 deformedVertex = IN.vertex;
                
                // Режим 0: Simple Scale
                if (_DeformMode < 0.5f)
                {
                    deformedVertex.xy *= _DeformScale;
                }
                // Режим 1: Affine Matrix
                else if (_DeformMode < 1.5f)
                {
                    // Применяем матрицу 3x3 к позиции (X,Y,1)
                    float3 pos = float3(deformedVertex.x, deformedVertex.y, 1);
                    float3 transformed;
                    transformed.x = dot(pos, float3(_DeformMatrix[0].x, _DeformMatrix[1].x, _DeformMatrix[2].x));
                    transformed.y = dot(pos, float3(_DeformMatrix[0].y, _DeformMatrix[1].y, _DeformMatrix[2].y));
                    transformed.z = dot(pos, float3(_DeformMatrix[0].z, _DeformMatrix[1].z, _DeformMatrix[2].z));
                    deformedVertex.xy = transformed.xy;
                }
                // Режим 2: Per Vertex
                else
                {
                    // Определяем индекс вершины по UV координатам
                    int vertexIndex = 0;
                    float u = IN.texcoord.x;
                    float v = IN.texcoord.y;
                    
                    if (u > 0.5f && v < 0.5f) vertexIndex = 1;      // нижний правый
                    else if (u < 0.5f && v > 0.5f) vertexIndex = 2;  // верхний левый
                    else if (u > 0.5f && v > 0.5f) vertexIndex = 3;  // верхний правый
                    // else vertexIndex = 0 (нижний левый)
                    
                    // Получаем смещение для этой вершины
                    float3 offset = float3(0,0,0);
                    if (vertexIndex == 0) offset = _Vertex0.xyz;
                    else if (vertexIndex == 1) offset = _Vertex1.xyz;
                    else if (vertexIndex == 2) offset = _Vertex2.xyz;
                    else offset = _Vertex3.xyz;
                    
                    // Применяем смещение относительно исходной позиции
                    deformedVertex.xyz = originalCorners[vertexIndex] + offset;
                }
                
                OUT.vertex = UnityObjectToClipPos(deformedVertex);
                
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif
                
                OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color * _RendererColor;
                
                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, IN.texcoord) * IN.color;
                color.rgb *= color.a;
                return color;
            }
            ENDCG
        }
    }
}