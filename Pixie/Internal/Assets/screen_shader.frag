#version 450 core

in vec2 frag_texture_coords;

out vec4 color;

layout(binding = 0) uniform sampler2D texture_sampler;
layout(binding = 1) uniform sampler2D palette_sampler;

layout(location = 3) uniform int palette_size;
layout(location = 4) uniform int background_color_index;

const int MAX_COLORS = 256;

void main(void) {
    float color_offset = 1.0 / palette_size;

    float color_idx = texture2D(texture_sampler, frag_texture_coords).x;
    if (color_idx <= 0.0) {
        color_idx = background_color_index;
    } else {
        color_idx = color_idx * MAX_COLORS;
    }

    float palette_color = (color_idx - 1) * color_offset;
    color = texture(palette_sampler, vec2(palette_color, 0.0));
    //color = vec4(0.5, 0.3, 0.5, 1.0);
}