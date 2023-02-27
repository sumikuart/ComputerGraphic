using OpenTK;
using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ComputerGraphic.Code
{
    public abstract class Behavior
    {
        protected GameObject gameObject;
        protected Game window;

        public Behavior(GameObject obj, Game window) { 
        
            this .gameObject = obj;
            this .window = window;
        }

        public abstract void Update(FrameEventArgs e);
    }
}
