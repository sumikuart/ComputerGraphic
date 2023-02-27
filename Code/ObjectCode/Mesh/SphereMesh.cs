using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static ComputerGraphic.Code.ObjectCode.SphereMesh;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ComputerGraphic.Code.ObjectCode
{
    internal class SphereMesh : Mesh
    {

        protected override uint[] Indices => base.Indices;

        protected override float[] Vertices => base.Vertices;

        private float radius;
        private int subs;

        public SphereMesh(float radius, int subs) 
        { 
            this.radius = radius;
            this.subs = subs;

            BuildVertices();
            GenerateBuffers();
        }

        public override void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void printSelf()
        {
            Console.Write("===== Sphere =====\n"
              + "        Radius: " + radius + "\n"
              + "  Sector Count: " + subs + "\n");
        }

        //public void printSelf()
        //{
        //    Console.Write("===== Sphere =====\n"
        //      + "        Radius: " + radius + "\n"
        //      + "  Sector Count: " + sectors + "\n"
        //      + "   Stack Count: " + stacks + "\n"
        //      + "Triangle Count: " + getTriangleCount() + "\n"
        //      + "   Index Count: " + getIndexCount() + "\n"
        //      + "  Vertex Count: " + getVertexCount() + "\n"
        //      + "TexCoord Count: " + getTexCoordCount());
        //}
        public struct textCord
        {
            public float x;
            public float y;
            public textCord(float x, float y)
            {
                this.x = x; this.y = y;
            }

            public float[] unfold()
            {
                return new float[] { this.x, this.y};
            }
        }

        public struct vertex
        {
            public float x;
            public float y;
            public float z;
            public vertex(float x, float y, float z)
            {
                this.x = x; this.y = y; this.z = z;
            }

            public float[] unfold()
            {
                return new float[] { this.x, this.y, this.z };
            }
        }

        
        public void BuildVertices()
        {
            const float S_Step = 186 / 2048.0f;
            const float T_Step = 322 / 1024.0f;

            List<float> tmpVertices = new List<float>();
            tmpVertices.AddRange(baseIcosahedron());

            List<vertex> vertices = new List<vertex>();
            List<textCord> textcoords = new List<textCord>();
            List<uint> indices = new List<uint>();

            vertex v0, v1, v2, v3, v4, v11;
            textCord t0, t1, t2, t3, t4, t11;
            uint index = 0;

            v0 = new vertex(tmpVertices[0], tmpVertices[1], tmpVertices[2]);
            v11 = new vertex(tmpVertices[11*3], tmpVertices[11*3+1], tmpVertices[11*3+2]);
            for (int i = 1; i <= 5; i++)
            {
                v1 = new vertex(tmpVertices[i * 3], tmpVertices[i * 3 + 1], tmpVertices[i * 3 + 2]);
                if (i < 5)
                    v2 = new vertex(tmpVertices[(i + 1) * 3], tmpVertices[(i + 1) * 3 + 1], tmpVertices[(i + 1) * 3 + 2]);
                else
                    v2 = new vertex(tmpVertices[3], tmpVertices[3 + 1], tmpVertices[3 + 2]);

                v3 = new vertex(tmpVertices[(i + 5) * 3], tmpVertices[(i + 5) * 3 + 1], tmpVertices[(i + 5) * 3 + 2]);
                if ((i + 5) < 10)
                    v4 = new vertex(tmpVertices[(i + 6) * 3], tmpVertices[(i + 6) * 3 + 1], tmpVertices[(i + 6) * 3 + 2]);
                else
                    v4 = new vertex(tmpVertices[6 * 3], tmpVertices[6 * 3 + 1], tmpVertices[6 * 3 + 2]);

                // texture coords
                t0 = new textCord((2 * i - 1) * S_Step, 0);
                t1 = new textCord((2 * i - 2) * S_Step, T_Step);
                t2 = new textCord((2 * i) * S_Step, T_Step);
                t3 = new textCord((2 * i - 1) * S_Step, T_Step*2);
                t4 = new textCord((2 * i + 1) * S_Step, T_Step*2);
                t11 = new textCord((2 * i) * S_Step, T_Step*3);

                // add a triangle in 1st row
                vertices.Add(v0); vertices.Add(v1); vertices.Add(v2);
                textcoords.Add(t0); textcoords.Add(t1); textcoords.Add(t2);
                indices.Add(index); indices.Add(index+1); indices.Add(index+2);

                // add 2 triangles in 2nd row
                vertices.Add(v1); vertices.Add(v3); vertices.Add(v2);
                textcoords.Add(t1); textcoords.Add(t3); textcoords.Add(t2);
                indices.Add(index+3); indices.Add(index + 4); indices.Add(index + 5);

                vertices.Add(v2); vertices.Add(v3); vertices.Add(v4);
                textcoords.Add(t2); textcoords.Add(t3); textcoords.Add(t4);
                indices.Add(index+6); indices.Add(index + 7); indices.Add(index + 8);

                // add a triangle in 3rd row
                vertices.Add(v3); vertices.Add(v11); vertices.Add(v4);
                textcoords.Add(t3); textcoords.Add(t11); textcoords.Add(t4);
                indices.Add(index + 9); indices.Add(index + 10); indices.Add(index + 11);

                // next index
                index += 12;
            }

            // subdivide icosahedron
            subdivideVerticesFlat(vertices, textcoords, indices);
        }

        public void subdivideVerticesFlat(List<vertex> vertices, List<textCord> textcoords, List<uint> indices)
        {
            List<vertex> tmpVertices = new List<vertex>();
            List<textCord> tmpTextcoords = new List<textCord>();
            List<uint> tmpIndices = new List<uint>();
            int indexCount;

            vertex v1, v2, v3;
            textCord t1, t2, t3;

            vertex newV1, newV2, newV3;
            textCord newT1, newT2, newT3;

            uint index = 0;
            int i, j;
            for(i = 1; i <= subs; i++)
            {
                tmpVertices = new List<vertex>(vertices);
                tmpTextcoords = new List<textCord>(textcoords);
                tmpIndices = new List<uint>(indices);

                vertices.Clear();
                textcoords.Clear();
                indices.Clear();

                index = 0;
                indexCount = tmpIndices.Count();
                for(j = 0; j < indexCount; j += 3) 
                {
                    // get 3 vertice and texcoords of a triangle
                    v1 = tmpVertices[(int)tmpIndices[j]];
                    v2 = tmpVertices[(int)tmpIndices[j+1]];
                    v3 = tmpVertices[(int)tmpIndices[j+2]];
                    t1 = tmpTextcoords[(int)tmpIndices[j]];
                    t2 = tmpTextcoords[(int)tmpIndices[j + 1]];
                    t3 = tmpTextcoords[(int)tmpIndices[j + 2]];

                    // get 3 new vertices by spliting half on each edge
                    newV1 = computeHalfVertex(v1, v2, radius);
                    newV2 = computeHalfVertex(v2, v3, radius);
                    newV3 = computeHalfVertex(v1, v3, radius);
                    newT1 = computeHalfTexCoord(t1, t2);
                    newT2 = computeHalfTexCoord(t2, t3);
                    newT3 = computeHalfTexCoord(t1, t3);

                    // add 4 new triangles
                    vertices.Add(v1); vertices.Add(newV1); vertices.Add(newV3);
                    textcoords.Add(t1); textcoords.Add(newT1); textcoords.Add(newT3);
                    indices.Add(index); indices.Add(index+1); indices.Add(index+2);

                    vertices.Add(newV1); vertices.Add(v2); vertices.Add(newV2);
                    textcoords.Add(newT1); textcoords.Add(t2); textcoords.Add(newT2);
                    indices.Add(index+3); indices.Add(index + 4); indices.Add(index + 5);

                    vertices.Add(newV1); vertices.Add(newV2); vertices.Add(newV3);
                    textcoords.Add(newT1); textcoords.Add(newT2); textcoords.Add(newT3);
                    indices.Add(index+6); indices.Add(index + 7); indices.Add(index + 8);

                    vertices.Add(newV3); vertices.Add(newV2); vertices.Add(v3);
                    textcoords.Add(newT3); textcoords.Add(newT2); textcoords.Add(t3);
                    indices.Add(index+9); indices.Add(index + 10); indices.Add(index + 11);

                    // next index
                    index += 12;
                }
            }

            buildInterleavedVertices(vertices, textcoords);
            this.Indices = indices.ToArray();
        }

        public void buildInterleavedVertices(List<vertex> vertices, List<textCord> textcords)
        {
            List<float> final = new List<float>();
            int count = vertices.Count;
            for(int i = 0; i < count; i++) 
            {
                final.AddRange(vertices[i].unfold());
                final.AddRange(textcords[i].unfold());
            }

            this.Vertices = final.ToArray();
        }
        public float[] baseIcosahedron()
        {
            float[] vertices = new float[12*3];
            float HAngle = MathF.PI / 180 * 72;
            float VAngle = MathF.Atan(1.0f / 2);

            int i1, i2;
            float z, xy;
            float hAngle1 = -MathF.PI / 2 - HAngle / 2;
            float hAngle2 = -MathF.PI / 2;

            vertices[0] = 0;
            vertices[1] = 0;
            vertices[2] = radius;

            for (int i = 1; i <= 5; i++)
            {
                i1 = i * 3;
                i2 = (i + 5) * 3;

                z = radius * MathF.Sin(VAngle);
                xy = radius * MathF.Cos(VAngle);

                vertices[i1] = xy * MathF.Cos(hAngle1);
                vertices[i2] = xy * MathF.Cos(hAngle2);
                vertices[i1 + 1] = xy * MathF.Sin(hAngle1);
                vertices[i2 + 1] = xy * MathF.Sin(hAngle2);
                vertices[i1 + 2] = z;       
                vertices[i2 + 2] = -z;
                

                // next horizontal angles
                hAngle1 += HAngle;
                hAngle2 += HAngle;
            }

            i1 = 11 * 3;
            vertices[i1] = 0;
            vertices[i1 + 1] = 0;
            vertices[i1 + 2] = -radius;

            return vertices;
        }
        public float computeScaleForLength(vertex vertex, float length)
        {
            return length / MathF.Sqrt(vertex.x*vertex.x + vertex.y*vertex.y + vertex.z*vertex.z);
        }

        public vertex computeHalfVertex(vertex v1, vertex v2, float length) 
        {
            vertex newV = new vertex(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
            float scale = computeScaleForLength(newV, length);
            newV = new vertex(newV.x*scale, newV.y * scale, newV.z * scale);
            return newV;
        }

        public textCord computeHalfTexCoord(textCord t1, textCord t2) 
        {
            return new textCord((t1.x + t2.x) * 0.5f, (t1.y + t2.y) * 0.5f);
        }
    }
}
