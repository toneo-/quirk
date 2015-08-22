#version 140

uniform mat4 Projection;
uniform mat4 ModelView;

in vec2 inPosition;
in vec4 inColor;
out vec4 fragColor;

void main(void) {
	gl_Position = Projection * ModelView * vec4(inPosition, 0.0, 1.0);
	fragColor = inColor;
}