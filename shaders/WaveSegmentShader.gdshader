shader_type canvas_item;

uniform int Segments = 3;
uniform sampler2D Texture;  

uniform float BarHeight = 46.6;

//45.6

//46.6

//46.935

void fragment() 
{
	vec4 textureSum;	
	float lowestSegment = 45.6 / float(Segments + 1);
	
	//Top & Bottom
	textureSum += texture(Texture, UV + vec2(0.0, 0.5));
	textureSum += texture(Texture, UV - vec2(0.0, 47.1));
	
	//Middle
	for (int i = 0; i < Segments; i++) {
		float yPos = (lowestSegment * float(i + 1));
		textureSum += texture(Texture, UV - vec2(0.0, yPos));
	}

	COLOR = textureSum;
}