shader_type spatial;

const float borderWidth = 0.01;

varying vec2 size;

void vertex(){
	size.x = abs(VERTEX.x) * 2.0;
	size.y = abs(VERTEX.z) * 2.0;
}

vec2 grid(vec2 uv, float columns, float rows){
	return fract(vec2(uv.x * columns, uv.y * rows));
}

void fragment() {
	vec2 gridPos = grid(UV, size.x, size.y);

    float borderCondition = step(gridPos.x, borderWidth) + step(gridPos.y, borderWidth) + 
                            step(1.0 - borderWidth, gridPos.x) + step(1.0 - borderWidth, gridPos.y);
    
    vec3 colorBorder = vec3(0.08, 0.08, 0.08);
    vec3 colorInside = vec3(0.03, 0.03, 0.03);

    ALBEDO = mix(colorInside, colorBorder, step(0.5, borderCondition));
}