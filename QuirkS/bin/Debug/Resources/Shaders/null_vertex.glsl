#version 140
// Null shader - Vertices pass straight through.

uniform Transformation {
	mat4 Projection;
	mat4 ModelView;
};

in vec3 vertex;

void main(void) {
	gl_Position = Projection * ModelView * vec4(vertex, 1.0);
}