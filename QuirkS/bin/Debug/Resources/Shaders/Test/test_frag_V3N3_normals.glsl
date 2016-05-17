#version 330

in vec3 fragPosition;
in vec3 fragNormal;

// Directional lights
struct DirectionalLight
{
	vec3 Direction;
	vec3 Position;
	vec3 Color;
	float Range;
};

uniform int LightCount;

layout(std140) uniform BlockyBlock {
	DirectionalLight[10] Lights;
};

void main(void) {
	//vec3 defaultColor = vec3(1.0f, 0.65f, 0.1f);
	//vec3 lightDirection = vec3(0.0f, -1.0f, -1.0f);

	//float diffuseFactor = dot(normalize(fragNormal), -lightDirection);

	vec3 totalColor = vec3(0.0f, 0.0f, 0.0f);

	for (int i = 0; i < LightCount; i++)
	{
		float distanceFactor = clamp(1 - length(Lights[i].Position - fragPosition) / Lights[i].Range, 0.0f, 1.0f);
		totalColor = totalColor + Lights[i].Color * clamp(dot(fragNormal, -Lights[i].Direction), 0.0f, 1.0f) * distanceFactor;
	}

	//vec4 diffuseColor = vec4(defaultColor * diffuseFactor, 1.0f);

	gl_FragColor = vec4(fragNormal.x, fragNormal.y, fragNormal.z, 1.0f); //vec4(1.0, 0.0, 0.0, 1.0);
}