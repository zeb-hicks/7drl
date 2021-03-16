#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state { Texture = <SpriteTexture>; MinFilter = Point; MagFilter = Point; AddressU = Wrap; AddressV = Wrap; };

Texture2D gdiff;
Texture2D gnorm;
Texture2D gdse;
Texture2D glight;

sampler2D gdiffs  = sampler_state { Texture = <gdiff>;   MinFilter = Point;  MagFilter = Point;  AddressU = Clamp; AddressV = Clamp; };
sampler2D gnorms  = sampler_state { Texture = <gnorm>;   MinFilter = Point;  MagFilter = Point;  AddressU = Clamp; AddressV = Clamp; };
sampler2D gdses   = sampler_state { Texture = <gdse>;    MinFilter = Point;  MagFilter = Point;  AddressU = Clamp; AddressV = Clamp; };
sampler2D glights = sampler_state { Texture = <glights>; MinFilter = Linear; MagFilter = Linear; AddressU = Clamp; AddressV = Clamp; };

float2 resolution;

struct VertexShaderOutput {
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float4 Depth : DEPTH0;
	float2 TextureCoordinates : TEXCOORD0;
};

const int sampleCount = 52;

const float aoSize = 3.0;
float depthAmbient(float2 uv) {
	float2 hpx = 0.5 / resolution;
	float o = 1.0;
	[loop]
	for (float y = -uv.y * aoSize; y < uv.y * aoSize; y += uv.y) {
		[loop]
		for (float x = -uv.x * aoSize; x < uv.x * aoSize; x += uv.x) {

		}
	}

	return o;
}

float4 MainPS(VertexShaderOutput input, float2 vPos : VPOS) : COLOR {
	float2 uv = input.TextureCoordinates;

	float4 diffuse = tex2D(gdiffs, uv);
	if (diffuse.a <= 0.05f) discard;
	float4 normal = tex2D(gnorms, uv);
	float4 dse = tex2D(gdses, uv);
	// float4 light = tex2D(glights * 0.5, uv);

	float a = diffuse.a;
	float3 c = diffuse.rgb;
	float3 n = normalize(normal.xyz * 2.0 - 1.0);
	float d = dse.r;
	float s = dse.g;
	float e = dse.b;
	// float3 l = light;

	// float3 ml = max(l, float3(e));
	float ao = depthAmbient(uv);
	
	return float4(ao, ao, ao, 1.0);
}

technique Compositing {
	pass BasePass {
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
