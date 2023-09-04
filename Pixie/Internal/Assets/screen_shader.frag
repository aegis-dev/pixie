#version 450 core

in vec2 frag_texture_coords;

out vec4 color;

layout(binding = 0) uniform sampler2D texture_sampler;
layout(binding = 1) uniform sampler2D palette_sampler;

layout(location = 3) uniform int palette_size;
layout(location = 4) uniform int background_color_index;

const float MAX_COLORS = 256.0;
const float COLOR_MULTIPLIER = 1.0 / MAX_COLORS;

void main(void) {
    float color_sample = texture2D(texture_sampler, frag_texture_coords).x;

    float color_idx = background_color_index;
    if (color_sample > 0.0) {
        color_idx = color_sample * MAX_COLORS;
    }

    color_idx = mod(color_idx, float(palette_size) + 1.0);

    float palette_color_offset = (color_idx - 1.0) * COLOR_MULTIPLIER;
    color = texture(palette_sampler, vec2(palette_color_offset, 0.0));
}