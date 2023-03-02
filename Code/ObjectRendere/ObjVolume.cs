using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Windowing.Desktop;

namespace ComputerGraphic.Code.ObjectRendere
{
    public class ObjVolume : Mesh
    {
        private Vector3[] vertices;
        private Vector3[] colors;
        private Vector2[] texCoords;
        protected override uint[] Indices => base.Indices;
        protected override float[] Vertices => base.Vertices;


        private List<Tuple<int,int,int>> faces = new List<Tuple<int,int,int>>();
 

        public  int VertCount { get { return vertices.Length; } }
        public  int IndiceCount { get { return faces.Count * 3; } }
        public  int ColorDataCount { get { return colors.Length; } }

  
        public static ObjVolume LoadModel(string path)
        {
            ObjVolume obj = new ObjVolume();
            
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
                }

            return obj;

        }
        
        public static ObjVolume LoadFromString(string path)
        {
            List<string> lines = new List<string>(path.Split('\n'));

            //Model Data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

            foreach (string line in lines)
            {

                if (line.StartsWith("v "))
                {
                    string temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2)
                    {
                        string[] vertParts = temp.Split(' ');

                        bool success = float.TryParse(vertParts[0], out vec.X);
                        success |= float.TryParse(vertParts[1], out vec.Y);
                        success |= float.TryParse(vertParts[2], out vec.Z);

                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }
                    verts.Add(vec);
                }
                else if (line.StartsWith("f "))
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<int, int, int> face = new Tuple<int, int, int>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success |= int.TryParse(faceparts[1], out i2);
                        success |= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new Tuple<int, int, int>(i1 - 1, i2 - 1, i3 - 1);
                            faces.Add(face);
                        }
                    }
                }

            }

            // Create the ObjVolume
            ObjVolume vol = new ObjVolume();
            vol.vertices = verts.ToArray();
            vol.faces = new List<Tuple<int, int, int>>(faces);
            vol.colors = colors.ToArray();
            vol.texCoords = texs.ToArray();

            List<float> VertList = new List<float>();
            for(int i = 0; i < vol.vertices.Length; i++)
            {
              
                    VertList.Add(verts[i].X);
                    VertList.Add(verts[i].Y);
                    VertList.Add(verts[i].Z);

            }

            //Vertices = VertList.ToArray();
            return vol;
        }




        public  Vector2[] GetTextureCoords()
        {
            return texCoords;
        }

        public  Vector3[] GetVerts()
        {
            return vertices;
        }

        public int[] GetIndices(int offset = 0)
        {
            List<int> temp = new List<int>();

            for(var i = 0; i < faces.Count; i++)
            {
                temp.Add((faces[i].Item1 + offset));
                temp.Add((faces[i].Item2 + offset));
                temp.Add((faces[i].Item3 + offset));
            }

            return temp.ToArray();
        }

        public  Vector3[] GetColorData()
        {
            return colors;
        }
    }
}
