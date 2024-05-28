shader_type canvas_item;

uniform vec4 color : source_color;

void fragment() {
    vec4 tex_color = texture(TEXTURE, UV);
    
    if (tex_color.a > 0.0) {
        COLOR = vec4(color.rgb, tex_color.a);
    } else {
        discard;
    }
}