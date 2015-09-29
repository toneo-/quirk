#version 140

uniform mat4 ModelViewProjection;

in vec3 inPosition;
in vec4 inColor;
out vec4 fragColor;

void main(void) {
	gl_Position = ModelViewProjection * vec4(inPosition, 1.0f);
	fragColor = inColor;
}