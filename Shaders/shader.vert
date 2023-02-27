#version 330 core
layout (location = 0) in vec3 aPosition; 
layout (location = 1) in vec2 aTexCord;


out vec2 texCord;

uniform mat4 mvp;
void main()
{
    vec3 newPosition = vec3(aPosition.x,aPosition.y,aPosition.z);
    gl_Position = vec4(newPosition, 1.0) * mvp;
    texCord = aTexCord;
    
}