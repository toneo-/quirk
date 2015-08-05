#version 140

uniform Transformation {
	mat4 Projection;
	mat4 ModelView;
};

in vec2 vertex;
in vec4 inColor;
out vec4 outColor;

void main(void) {
	gl_Position = Projection * ModelView * vec4(vertex, 0.0, 1.0);
}