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
    vec2 grid = grid(UV, size.x, size.y);
	
	if (grid.x < borderWidth || grid.x > 1.0 - borderWidth || grid.y < borderWidth || grid.y > 1.0 - borderWidth){
    	ALBEDO = vec3(0, 0, 0.0);
	}
}