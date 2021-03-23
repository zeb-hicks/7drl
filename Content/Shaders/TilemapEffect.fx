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
Texture2D gdse;
Texture2D gnorm;

Texture2D diffuse;
Texture2D normal;
Texture2D specular;
Texture2D emissive;
Texture2D depth;

sampler2D diffuseSampler  = sampler_state { Texture = <diffuse>;  MinFilter = Point; MagFilter = Point; AddressU = Wrap; AddressV = Wrap; };
sampler2D normalSampler   = sampler_state { Texture = <normal>;   MinFilter = Point; MagFilter = Point; AddressU = Wrap; AddressV = Wrap; };
sampler2D specularSampler = sampler_state { Texture = <specular>; MinFilter = Point; MagFilter = Point; AddressU = Wrap; AddressV = Wrap; };
sampler2D emissiveSampler = sampler_state { Texture = <emissive>; MinFilter = Point; MagFilter = Point; AddressU = Wrap; AddressV = Wrap; };
sampler2D depthSampler    = sampler_state { Texture = <depth>;    MinFilter = Point; MagFilter = Point; AddressU = Wrap; AddressV = Wrap; };

int Tiles[256];
int TileSize;
float2 MapSize;

struct VertexShaderOutput {
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float4 Depth : DEPTH0;
	float2 TextureCoordinates : TEXCOORD0;
};

struct PixelShaderOutput {
	float4 Diffuse: COLOR0;
	float4 Normal: COLOR1;
	float4 DSE: COLOR2;
};

int TileIDFromUV(float2 uv) {
	float2 tuv = uv * MapSize;
	return (int)(tuv.x + tuv.y * MapSize.x);
}

PixelShaderOutput MainPS(VertexShaderOutput input, float2 vPos : VPOS) {
	PixelShaderOutput output;

	float2 uv = input.TextureCoordinates;

	float4 diffuse = tex2D(diffuseSampler, uv);

	if (diffuse.a <= 0.05f) discard;

	float4 normal = tex2D(normalSampler, uv);
	normal.rgb = normalize(normal.rgb - float3(0.5, 0.5, 0.5)) + float3(0.5, 0.5, 0.5);
	float specular = tex2D(specularSampler, uv).r;
	float depth = tex2D(depthSampler, uv).r;
	float emissive = tex2D(emissiveSampler, uv).r;

	output.Diffuse = diffuse;
	output.Normal = normal;
	output.DSE = float4(depth, specular, emissive, 1.0);
	return output;
}

technique TilemapDrawing {
	pass BasePass {
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
