using ComputerGraphic.Code.ObjectCode;
using ComputerGraphic.Code.ObjectCode.Behaviors;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ComputerGraphic.Code
{
    public class Game : GameWindow
    {
        public int VertexBufferObject;
        public int VertexArrayObject;
        public int ElementBufferObject;

        public Texture wallTex;
        public Texture wallArg;

        public float RotationDegree = 0;

        private List<GameObject> gameObjects = new List<GameObject>();
        private Camera camera;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        //En kode der køre en gang når vinduet laves.
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            wallArg = new Texture(@"..\..\..\Assets/Images/AragonTexUdenBaggrund.png");
            wallTex = new Texture(@"..\..\..\Assets/Images/wall.jpg");

            Dictionary<string, object> uniforms = new Dictionary<string, object>();

            uniforms.Add("texture0", wallArg);
            uniforms.Add("texture1", wallTex);

            Material mat = new Material(@"..\..\..\Shaders/shader.vert", @"..\..\..\Shaders/shader.frag", uniforms);

            Renderer rendTri = new Renderer(mat, new TriangleMesh());
            Renderer rendCub = new Renderer(mat, new CubeMesh());
            Renderer rendSphere = new Renderer(mat, new SphereMesh(1, 2));

            GameObject triangle = new GameObject(rendTri, this);
            GameObject cube = new GameObject(rendCub, this);
            GameObject sphere = new GameObject(rendSphere, this);

            cube.transform.Position = new Vector3(1, 0, 0);

            GameObject cam = new GameObject(null, this);
            cam.AddComponent<Camera>(60.0f, (float)Size.X, (float)Size.Y, 0.3f, 1000.0f);
            camera = cam.GetComponent<Camera>();

            gameObjects.Add(cam);
            gameObjects.Add(triangle);
            gameObjects.Add(cube);
            gameObjects.Add(sphere);

            sphere.AddComponent<KeybordMovement>();
        }

        //En metode der kaldes når en frame renders
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60.0f), (float)Size.X / (float)Size.Y, 0.3f, 1000.0f);

            gameObjects.ForEach(x => x.Draw(camera.GetViewProjection()));

            Context.SwapBuffers();
        }

        //Denne kode køre hvert update frame.
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            gameObjects.ForEach(x => x.Update(args));
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
}