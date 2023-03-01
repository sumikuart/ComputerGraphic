using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ComputerGraphic.Code.ObjectCode
{
    internal class CylinderMesh : Mesh
    {

        protected override uint[] Indices => base.Indices;

        protected override float[] Vertices => base.Vertices;

        private float topRadius;
        private float bottomRadius;
        private float height;
        private int sectors;

        public CylinderMesh(float topRadius, float bottomRadius, float height, int sectors)
        {
            this.topRadius = topRadius;
            this.bottomRadius = bottomRadius;
            this.height = height;
            this.sectors = sectors < 3 ? 3 : sectors;

            BuildVertices();
            GenerateBuffers();
        }

        public override void Draw()
        {
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void BuildVertices()
        {
            List<float> unitVertices = getUnitCircleVertices();
            List<float> vertices = new List<float>();
            List<uint> indices = new List<uint>();

            for(int i = 0; i < 2; i++)
            {
                float h = -height / 2.0f + i * height;
                float t = 1.0f - i;
                float radius;
                if(h < 0)
                    radius = bottomRadius;
                else
                    radius = topRadius;

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
            uint baseCenterIndex = (uint)vertices.Count/5;
            uint topCenterindex = baseCenterIndex + 1;
            vertices.Add(0); vertices.Add(0); vertices.Add(-height/2.0f);
            vertices.Add(0.5f); vertices.Add(0.5f);

            vertices.Add(0); vertices.Add(0); vertices.Add(height/2.0f);
            vertices.Add(0.5f); vertices.Add(0.5f);

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

                //base and top circle
                //baseCenterIndex => k1 => k1+1 
                indices.Add(baseCenterIndex);
                indices.Add(k1);
                indices.Add(k1+1);

                //topCenterIndex => k2 => k2+1
                indices.Add(topCenterindex);
                indices.Add(k2);
                indices.Add(k2 + 1);
            }
            this.Vertices = vertices.ToArray();
            this.Indices = indices.ToArray();
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
