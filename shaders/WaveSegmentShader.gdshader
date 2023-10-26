shader_type canvas_item;

uniform int Segments = 3;
uniform sampler2D Texture; 

uniform float BarHeight = 1;

void fragment() 
{
	vec4 textureSum = texture(Texture, UV);	
	float lowestSegment = BarHeight / float(Segments + 1);
	
	//Init texture clear
	textureSum.a = 0.0;
	
	for (int i = 0; i < (Segments + 2); i++) {
		float yPos = (lowestSegment * float(i));
		textureSum += texture(Texture, UV + vec2(0.0, (0.5 - yPos)));
	}
	
	textureSum.rgb = COLOR.rgb;
	textureSum.a = 1.0 - textureSum.a;
	
	COLOR = textureSum;
}