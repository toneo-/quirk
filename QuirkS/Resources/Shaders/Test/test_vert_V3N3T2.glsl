#version 140

uniform mat4 ModelViewProjection;
uniform mat4 Model;

uniform sampler2D tex1;

in vec3 inPosition;
in vec3 inNormal;
in vec2 inUV;

out vec3 fragPosition;
out vec3 fragNormal;
out vec2 fragUV;

void main(void) {
	gl_Position = ModelViewProjection * vec4(inPosition, 1.0f);
	fragPosition = (Model * vec4(inPosition, 0.0f)).xyz;
	fragNormal = (Model * vec4(inNormal, 0.0f)).xyz;
	fragUV = inUV;
}