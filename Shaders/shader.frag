#version 330 core
out vec4 FragColor;
in vec2 texCord;

uniform sampler2D texture1;
uniform sampler2D texture2;

uniform vec2 moveVectorVigo;
uniform vec2 moveVectorWall;
uniform float rotation;
vec2 rotationOffset = vec2(0.5,0.5);

void main()
{
    

    //vec4 src = texture(texture2,vec2(0.5*(1+cos(rotation))* texCord.x,0.5*(1+sin(rotation))+texCord.y));
    float xCoordinate = cos(rotation) * (texCord.x - rotationOffset.x)  - sin(rotation) * (texCord.y - rotationOffset.y) + rotationOffset.x;
    float yCoordinate = sin(rotation) * (texCord.x -  rotationOffset.x) + cos(rotation) * (texCord.y - rotationOffset.y) +  rotationOffset.y;

    vec4 src = texture(texture2,vec2(moveVectorWall.x+xCoordinate, moveVectorWall.y+yCoordinate));
    vec4 dst = texture(texture1,texCord+moveVectorVigo);

    FragColor = vec4((src.rgb * src.a + dst.rgb*dst.a*(1.0 - src.a))/(src.a+dst.a*(1.0f-src.a)),(src.a+dst.a*(1.0-src.a)));

    //FragColor = vec4(1,0,0,1);
}