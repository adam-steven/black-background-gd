shader_type canvas_item;

uniform int Segments = 3;
uniform sampler2D Texture;  

uniform float BarHeight = 47.6;

void fragment() 
{
	vec4 textureSum;	
	float lowestSegment = BarHeight / float(Segments + 1);
	
	for (int i = 0; i < (Segments + 2); i++) {
		float yPos = (lowestSegment * float(i));
		textureSum += texture(Texture, UV + vec2(0.0, (0.5 - yPos)));
	}

	COLOR = textureSum;
}