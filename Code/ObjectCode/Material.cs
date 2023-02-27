using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace ComputerGraphic.Code
{
    public class Material
    {
        private Shader shader;
        private Dictionary<string,object> uniforms = new Dictionary<string,object>();
        private Dictionary<int,Texture> textures = new Dictionary<int, Texture>();
        public Material(string vertPath, string fragPath, Dictionary<string,object> uniforms) {
            shader = new Shader(vertPath, fragPath);

            foreach(KeyValuePair<string,object> unifrom in uniforms) {

                SetUniform(unifrom.Key, unifrom.Value);
            }


        }

        public void UseShader() {
        
            foreach(KeyValuePair<int, Texture> TexWidthIndex in textures)
            {
                TexWidthIndex.Value.Use(TextureUnit.Texture0 + TexWidthIndex.Key);

            }

            shader.Use();
        }
        
        public void SetUniform(string name, object uniform)
        {
            shader.Use();

            if(uniform is int uniformInt)
            {
                shader.SetInt(name, uniformInt);

            } else if(uniform is float uniformFloat)
            {
                shader.SetFloat(name,uniformFloat);

            } else if(uniform is Matrix4 uniformMatrix)
            {
                shader.SetMatrix(name, uniformMatrix);

            } else if(uniform is Texture tex)
            {
                int addTextures = textures.Count;
                shader.SetInt(name, addTextures);
                textures.Add(addTextures, tex);

            } else
            {
                Console.WriteLine($"Unfupported shader uniform type");
                return;

            }

            uniforms[name] = uniform;
        }
    }
}
