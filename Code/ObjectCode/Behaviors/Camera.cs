using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphic.Code.ObjectCode.Behaviors
{
    internal class Camera : Behavior
    {
        Vector3 front = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        float speed = 1;
        private float FOV;
        private float aspectX;
        private float aspectY;
        private float near;
        private float far;

        public Camera(GameObject gameObject, Game window, float FOV, float aspectX, float aspectY, float near, float far) : base(gameObject, window)
        {
            gameObject.transform.Position = new Vector3(0.0f, 0.0f, 3.0f);
            this.FOV = FOV;
            this.aspectX = aspectX;
            this.aspectY = aspectY;
            this.near = near;
            this.far = far;
        }

        public override void Update(FrameEventArgs e)
        {
            KeyboardState input = window.KeyboardState;
            if (input.IsKeyDown(Keys.W))
            {
                gameObject.transform.Position += front * speed * (float)e.Time; //Forward 
            }
            if (input.IsKeyDown(Keys.S))
            {
                gameObject.transform.Position -= front * speed * (float)e.Time; //Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                gameObject.transform.Position -= Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                gameObject.transform.Position += Vector3.Normalize(Vector3.Cross(front, up)) * speed * (float)e.Time; //Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                gameObject.transform.Position += up * speed * (float)e.Time; //Up 
            }

            if (input.IsKeyDown(Keys.LeftShift))
            {
                gameObject.transform.Position -= up * speed * (float)e.Time; //Down
            }
        }

        public Matrix4 GetViewProjection()
        {
            Matrix4 view = Matrix4.LookAt(gameObject.transform.Position, gameObject.transform.Position + front, up);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), aspectX / aspectY, near, far);
            return view * projection;
        }

    }
}
