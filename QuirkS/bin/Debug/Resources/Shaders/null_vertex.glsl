﻿#version 140
// Null shader - Vertices pass straight through.

uniform Transformation {
	mat4 projection_matrix;
	mat4 modelview_matrix;
};

in vec3 vertex;

void main(void) {
	gl_Position = projection_matrix * modelview_matrix * vec4(vertex, 1.0);
}