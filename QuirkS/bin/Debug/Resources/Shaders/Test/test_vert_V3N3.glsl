#version 140

uniform mat4 ModelViewProjection;
uniform mat4 Model;

uniform float Extrusion;

in vec3 inPosition;
in vec3 inNormal;

out vec3 fragPosition;
out vec3 fragNormal;

void main(void) {
	gl_Position = ModelViewProjection * vec4(inPosition, 1.0f);
	fragPosition = (Model * vec4(inPosition, 0.0f)).xyz;
	fragNormal = (Model * vec4(inNormal, 0.0f)).xyz;
}