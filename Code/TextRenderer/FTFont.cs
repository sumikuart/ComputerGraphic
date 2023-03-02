
using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
//using SharpFont; 
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;


namespace ComputerGraphic
{
    public struct Character
    {
        public int TextureID { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Bearing { get; set; }
        public int Advance { get; set; }
    }

    internal class FTFont
    {
        private Dictionary<uint,Character> _characters = new Dictionary<uint,Character>();
        private int vertexArray;
        private int vertexBuffer;

        public FTFont(uint pixelHeight) {

            //Library lib = new Libarary();
        
        }
    }
}
