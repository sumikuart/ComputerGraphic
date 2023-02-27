using ComputerGraphic.Code;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ComputerGraphic
{
    public class Program
    {
        static void Main(string[] args)
        {

            GameWindowSettings settings = new GameWindowSettings()
            {
                RenderFrequency = 60.0,
                UpdateFrequency = 60.0
            };

            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "GameWindow"
            };
            Game game = new Game(settings, nativeWindowSettings);
            game.Run();
        }
    }
}
