shader_type spatial;

uniform sampler2D depth_texture : source_color, hint_depth_texture;
uniform vec3 camera_world_position;

varying mat4 CAMERA;

void vertex() {
  CAMERA = INV_VIEW_MATRIX;
}

void fragment() {
	float depth = texture(depth_texture, SCREEN_UV).x;
	vec3 ndc = vec3(SCREEN_UV * 2.0 - 1.0, depth);

	vec4 view = INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	view.xyz /= view.w;
	float linear_depth = -view.z;
	
	vec4 world = CAMERA * INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
	vec3 world_position = world.xyz / world.w;
	
	vec3 cameraDir = normalize(world_position - camera_world_position);
	world_position += cameraDir * 0.1f;
	
	
}

//varying vec3 world_position;
//
//void vertex()
//{
    //world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
//}
//
//void fragment()
//{
    //// here you can use world_position
//}