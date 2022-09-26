shader_type canvas_item;

uniform int Segments = 3;
uniform sampler2D Texture; 

uniform float BarWidth = 1;

void fragment() 
{
	vec4 textureSum;	
	float lowestSegment = BarWidth / float(Segments + 1);
	
	for (int i = 0; i < Segments; i++) {
		float xPos = (lowestSegment * float(i + 1));
		textureSum += texture(Texture, UV + vec2((0.5 - xPos), -0.25));
	}
	
	textureSum.rgb = MODULATE.rgb;
	textureSum.a = 1.0 - textureSum.a;
	
	COLOR = textureSum;
}