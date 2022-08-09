shader_type canvas_item;

uniform int Segments = 3;
uniform sampler2D Texture; 

uniform float BarHeight = 1;

void fragment() 
{
	vec4 textureSum;	
	float lowestSegment = BarHeight / float(Segments + 1);
	
	for (int i = 0; i < (Segments + 2); i++) {
		float yPos = (lowestSegment * float(i));
		textureSum += texture(Texture, UV + vec2(0.0, (0.5 - yPos)));
	}
	
	//If transparent pixel set it to barColour
	if(textureSum.a == 0.0) {
		textureSum = MODULATE;
	}
	
	//If black pixel set it to transparent
	if(textureSum.r == 0.0 && textureSum.g == 0.0 && textureSum.b == 0.0) {
		textureSum.a = 0.0;
	}
	
	COLOR = textureSum;
}