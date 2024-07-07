shader_type spatial;

uniform sampler2D main_texture : source_color;
uniform vec4 color_stripe : source_color = vec4(0.2, 0.3, 1.0, 1.0);
uniform float divisions = 8.0; // increase for more stripe density
uniform float stripe_bias = 1.2; // 1.0 means no stripes; 2.0 means stripes and gaps are equal size
uniform float speed = 0.05;
uniform float angle = 0.7854; // in radians

void fragment() {
    float w = cos(angle) * UV.x + sin(angle) * UV.y - speed * TIME;
    float stripe_condition = step(0.0001, floor(mod(w * divisions, stripe_bias)));
    
	vec3 texture_color = mix(
		COLOR.rgb, 
		texture(main_texture, UV.xy).rgb, 
		0.4
	);
    
	ALBEDO = mix(texture_color, color_stripe.rgb, stripe_condition);
}
