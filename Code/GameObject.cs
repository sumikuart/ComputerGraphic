using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphic.Code
{
    public class GameObject
    {

        public Transform transform;
        public Renderer render;

        private GameWindow gameWindow;

        List<Behavior> behaviors = new List<Behavior>();


        public GameObject(Renderer renderer, GameWindow gameWindow)
        {
            this.render = renderer;
            this.gameWindow = gameWindow;

            transform = new Transform();
        }

        public T GetComponent<T>() where T : Behavior
        {

            foreach (Behavior behavior in behaviors)
            {
                T componentAsT = behavior as T;
                if (componentAsT != null) return componentAsT;
            }

            return null;
        }

        public void AddComponent<T>(params object?[]? args) where T : Behavior
        {
            behaviors.Add(Activator.CreateInstance(typeof(T), this, gameWindow) as T);
        }


        public void Update(FrameEventArgs args)
        {
            foreach (Behavior behavior in behaviors)
            {
                behavior.Update(args);
            }
        }

        public void Draw(Matrix4 vp)
        {
            render.Draw(transform.CalculateModel() * vp);
        }

    }
}
