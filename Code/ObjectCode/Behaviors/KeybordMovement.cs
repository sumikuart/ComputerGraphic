using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ComputerGraphic.Code
{
    public class KeybordMovement : Behavior
    {
        private float movementSpeed = 1.0f;

        public KeybordMovement(GameObject obj, Game window) : base(obj, window) { 
        
     

        }


        public override void Update(FrameEventArgs e)
        {
            KeyboardState input = window.KeyboardState;

            if (input.IsKeyDown(Keys.Up))
            {
                gameObject.transform.Position.Y += movementSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.Down))
            {
                gameObject.transform.Position.Y -= movementSpeed * (float)e.Time;
            }


            if (input.IsKeyDown(Keys.Left))
            {
                gameObject.transform.Position.X -= movementSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.Right))
            {
                gameObject.transform.Position.X += movementSpeed * (float)e.Time;
            }

            if (input.IsKeyDown(Keys.U))
            {
                gameObject.transform.Rotation.X += movementSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.J))
            {
                gameObject.transform.Rotation.X -= movementSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.H))
            {
                gameObject.transform.Rotation.Z += movementSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.K))
            {
                gameObject.transform.Rotation.Z -= movementSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.Y))
            {
                gameObject.transform.Rotation.Y += movementSpeed * (float)e.Time;
            }
            if (input.IsKeyDown(Keys.I))
            {
                gameObject.transform.Rotation.Y -= movementSpeed * (float)e.Time;
            }
        }
    }
}
