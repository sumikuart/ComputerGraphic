using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ComputerGraphic.Code.ObjectCode
{
    internal class CapsuleMesh : Mesh
    {

        protected override uint[] Indices => base.Indices;

        protected override float[] Vertices => base.Vertices;

        private float radius;
        private float height;
        private int sectors;
        private int subs;

        public CapsuleMesh(float radius, float height, int subs)
        {
            this.radius = radius;
            this.height = height;
            this.subs = subs < 1 ? 1 : subs;

            BuildVertices();
            GenerateBuffers();
        }

        public override void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public struct vertex
        {
            public float x;
            public float y;
            public float z;
            public float s;
            public float t;
            public vertex(float x, float y, float z, float s, float t)
            {
                this.x = x; this.y = y; this.z = z; this.s = s; this.t = t;
            }

            public float[] unfold()
            {
                return new float[] { this.x, this.y, this.z, this.s, this.t };
            }
        }
        public void BuildVertices()
        {
            SphereMesh sphere = new SphereMesh(radius, subs, false);
            uint[] sphereIndices = sphere.getIndices();
            float[] sphereVertices = sphere.getVertices();

            List<vertex> vertices = new List<vertex>();

            List<uint> topSphereBase = new List<uint>();
            List<uint> bottomSphereBase = new List<uint>();

            uint index = 0;
            int nearZeros = 0;
            int nearZeros1 = 0;
            int nearZeros2 = 0;
            int nearZeros3 = 0;
            for (int i = 0; i < sphereIndices.Length; i++, index++)
            { 
                float x = sphereVertices[i * 5];
                float y = sphereVertices[i * 5 + 1];
                float z = sphereVertices[i * 5 + 2];
                float s = sphereVertices[i * 5 + 3];
                float t = sphereVertices[i * 5 + 4];
                if(nearZero(z))
                {
                    nearZeros++;
                    vertex newV;
                    if (i % 3 == 0)
                    {
                        Console.WriteLine("its the first");
                        nearZeros1++;
                        if (sphereVertices[(i+1) * 5 + 2] > 0.001f || sphereVertices[(i + 2) * 5 + 2] > 0.001f)
                            newV = new vertex(x, y, height / 2.0f, s, t);
                        else
                            newV = new vertex(x, y, -height / 2.0f, s, t);
                    }
                    else if ((i+2) % 3 == 0)
                    {
                        Console.WriteLine("its the second");
                        nearZeros3++;
                        if (sphereVertices[(i - 1) * 5 + 2] > 0.001f || sphereVertices[(i + 1) * 5 + 2] > 0.001f)
                            newV = new vertex(x, y, height / 2.0f, s, t);
                        else
                            newV = new vertex(x, y, -height / 2.0f, s, t);

                    } 
                    else
                    {
                        Console.WriteLine("its the third");
                        nearZeros2++;
                        if (sphereVertices[(i - 1) * 5 + 2] > 0.001f || sphereVertices[(i - 2) * 5 + 2] > 0.001f)
                            newV = new vertex(x, y, height / 2.0f, s, t);
                        else
                            newV = new vertex(x, y, -height / 2.0f, s, t);
                    } 
                    vertices.Add(newV);
                    if(newV.z > 0 && !topSphereBase.Any(j => vertices[(int)j].Equals(newV)))
                        topSphereBase.Add((uint)i);
                    else if (newV.z < 0 && !bottomSphereBase.Any(j => vertices[(int)j].Equals(newV)))
                        bottomSphereBase.Add((uint)i);

                }
                else if(z > 0)
                {
                    vertices.Add(new vertex(x, y, z + height / 2.0f, s, t));
                }
                else if(z < 0)
                {
                    vertices.Add(new vertex(x, y, z - height / 2.0f, s, t));
                }
            }
            Console.WriteLine("nearZeros: "+nearZeros);
            Console.WriteLine("nearZeros first: " + nearZeros1);
            Console.WriteLine("nearZeros second: " + nearZeros2);
            Console.WriteLine("nearZeros thrid: " + nearZeros3);
            List<float> finalVertices = new List<float>();
            foreach(vertex v in vertices)
            {
                finalVertices.AddRange(v.unfold());
            }

            List<uint> finalIndices = new List<uint>();
            finalIndices.AddRange(sphereIndices);
            sectors = nearZeros / 6;
            for (int i = 0; i < sectors; i++)
            {
                // 2 triangles per sector
                // k1 => k1+1 => k2
                finalIndices.Add(topSphereBase[i]);
                finalIndices.Add(topSphereBase[i+1]);
                finalIndices.Add(bottomSphereBase[i]);

                // k2 => k1+1 => k2+1
                finalIndices.Add(bottomSphereBase[i]);
                finalIndices.Add(topSphereBase[i + 1]);
                finalIndices.Add(bottomSphereBase[i+1]);
            }
            this.Vertices = finalVertices.ToArray();
            this.Indices = finalIndices.ToArray();
        }

        private bool nearZero(float value)
        {
            return (value > -0.001f) && (value < 0.001f);
        }
        public void BuildCylinderVertices()
        {
            List<float> unitVertices = getUnitCircleVertices();
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            for(int i = 0; i < 2; i++)
            {
                float h = -height / 2.0f + i * height;
                float t = 1.0f - i;

                for(int j = 0, k = 0; j <= sectors; j++, k += 3)
                {
                    float ux = unitVertices[k];
                    float uy = unitVertices[k+1];
                    float uz = unitVertices[k+2];

                    vertices.Add(ux * radius);
                    vertices.Add(uy * radius);
                    vertices.Add(h);
                    vertices.Add((float)j/sectors);
                    vertices.Add(t);
                }
            }

            uint k2 = (uint)sectors + 1;
            for(uint k1 = 0; k1 < sectors; k1++, k2++)
            {
                // 2 triangles per sector
                // k1 => k1+1 => k2
                indices.Add(k1);
                indices.Add(k1 + 1);
                indices.Add(k2);

                // k2 => k1+1 => k2+1
                indices.Add(k2);
                indices.Add(k1+1);
                indices.Add(k2+1);
            }
        }

        public List<float> getUnitCircleVertices()
        {
            float sectorStep = 2 * MathF.PI / sectors;
            float sectorAngle;

            List<float> unitCircleVertices = new List<float>();
            for(int i = 0; i <= sectors; i++)
            {
                sectorAngle = (float)i * sectorStep;
                unitCircleVertices.Add(MathF.Cos(sectorAngle));
                unitCircleVertices.Add(MathF.Sin(sectorAngle));
                unitCircleVertices.Add(0.0f);
            }
            return unitCircleVertices;
        }
    }
}
