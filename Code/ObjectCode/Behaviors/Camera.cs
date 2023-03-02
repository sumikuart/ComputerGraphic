using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private Vector2 RotationOffset = new Vector2(0.0f,0.0f);

        private GameObject gameObject;


        private Matrix4 view;

        //Mouse Input
        private float yaw;
        private float pitch;
        private bool firstRun = true;
 
        private Vector2 lastPos = new Vector2(0,0);
        private Vector2 delta = new Vector2(0, 0);
        private float sensitivity = 9;


        public Camera(GameObject gameObject, Game window, float FOV, float aspectX, float aspectY, float near, float far) : base(gameObject, window)
        {
            this.gameObject = gameObject;
            this.gameObject.transform.Position = new Vector3(0.0f, 0.0f, 3.0f);
            this.FOV = FOV;
            this.aspectX = aspectX;
            this.aspectY = aspectY;
            this.near = near;
            this.far = far;


            view = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            
        }

        public override void Update(FrameEventArgs e)
        {
          

            if (!window.IsFocused) // Check to see if the window is focused
            {
                return;
            }


            KeyboardState input = window.KeyboardState;
            MouseState mouse = window.MouseState;

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

            if (input.IsKeyDown(Keys.LeftShift))
            {
                gameObject.transform.Position -= up * speed * (float)e.Time; //Down
            }


            //Rotation With Keyboard
            if (input.IsKeyDown(Keys.T))
            {
                RotationOffset.Y += sensitivity * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.G))
            {
                RotationOffset.Y -= sensitivity * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.F))
            {
                RotationOffset.X -= sensitivity * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.H))
            {
                RotationOffset.X += sensitivity * (float)e.Time;
            }

            //Roattion With mouse
            if (firstRun)
            {
                lastPos = new Vector2 (mouse.X, mouse.Y);
                firstRun = false;
                yaw = 270;
            } else
            {
                delta.X = mouse.X - lastPos.X;
                delta.Y = mouse.Y - lastPos.Y;

                lastPos = new Vector2(mouse.X, mouse.Y);
                yaw += delta.X * sensitivity * (float)e.Time;

                if (pitch > 89.0f)
                {
                    pitch = 89.0f;
                }
                else if (pitch < -89.0f)
                {
                    pitch = -89.0f;
                }
                else
                {
                    pitch -= delta.Y * sensitivity * (float)e.Time;
                }
            }            



            front.X = (float)Math.Cos(MathHelper.DegreesToRadians(yaw))* (float)Math.Cos(MathHelper.DegreesToRadians(pitch)); 
            front.Y = (float)Math.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = (float)Math.Cos(MathHelper.DegreesToRadians(pitch)) * (float)Math.Sin(MathHelper.DegreesToRadians(yaw));
            front = Vector3.Normalize(front);

            view = Matrix4.LookAt(gameObject.transform.Position, gameObject.transform.Position+front, up);
            
        }

        public Matrix4 GetViewProjection()
        {
            Matrix4 view = Matrix4.LookAt(gameObject.transform.Position, gameObject.transform.Position + front, up);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), aspectX / aspectY, near, far);
            return view * projection;
        }

    }
}
