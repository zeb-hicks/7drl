#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state {
	Texture = <SpriteTexture>;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Wrap;
	AddressV = Wrap;
};
Texture2D ColorLUT;
sampler2D ColorLUTSampler = sampler_state {
	Texture = <ColorLUT>;
	MinFilter = Point;
	MagFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderOutput {
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR {
	float size = 64;
	float sizeroot = 8;

	float4 c = tex2D(SpriteTextureSampler, input.TextureCoordinates);

	float ri = trunc(c.r * (size - 1)); // R 0-63
	float gi = trunc(c.g * (size - 1)); // G 0-63
	float bi = trunc(c.b * (size - 1)); // B 0-63

	float r = frac(ri / size / sizeroot); // R 0-0.125 (Red sector offset.)
	float g = frac(gi / size / sizeroot); // G 0-0.125 (Green sector offset.)

	float bx = trunc(bi % sizeroot) / sizeroot; // B 0-0.875 (Blue sector X offset.)
	float by = trunc(bi / sizeroot) / sizeroot; // B 0-0.875 (Blue sector Y offset.)

	float2 p = float2(r + bx, g + by);

	float4 lut = tex2D(ColorLUTSampler, p);

	return lut;
}

technique SpriteDrawing {
	pass P0 {
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
