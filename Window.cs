using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace MiniProyecto2
{
    // In this tutorial we take a look at how we can use textures to make the light settings we set up in the last episode
    // different per fragment instead of making them per object.
    // Remember to check out the shaders for how we converted to using textures there.
    public class Window : GameWindow
    {
        // Since we are going to use textures we of course have to include two new floats per vertex, the texture coords.
        private readonly float[] _vertices =
        {
            // Positions          Normals              Texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };
        private readonly float[] verticesBody = {
        // positions          // normals           // texture coords
        -0.35f, -0.35f, -0.35f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,
         0.35f, -0.35f, -0.35f,  0.0f,  0.0f, -1.0f,  1.0f,  0.0f,
         0.35f,  0.35f, -0.35f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
         0.35f,  0.35f, -0.35f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
        -0.35f,  0.35f, -0.35f,  0.0f,  0.0f, -1.0f,  0.0f,  1.0f,
        -0.35f, -0.35f, -0.35f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,

        -0.35f, -0.35f,  0.35f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,
         0.35f, -0.35f,  0.35f,  0.0f,  0.0f,  1.0f,  1.0f,  0.0f,
         0.35f,  0.35f,  0.35f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
         0.35f,  0.35f,  0.35f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
        -0.35f,  0.35f,  0.35f,  0.0f,  0.0f,  1.0f,  0.0f,  1.0f,
        -0.35f, -0.35f,  0.35f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,

        -0.35f,  0.35f,  0.35f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
        -0.35f,  0.35f, -0.35f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
        -0.35f, -0.35f, -0.35f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
        -0.35f, -0.35f, -0.35f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
        -0.35f, -0.35f,  0.35f, -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
        -0.35f,  0.35f,  0.35f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

         0.35f,  0.35f,  0.35f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
         0.35f,  0.35f, -0.35f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
         0.35f, -0.35f, -0.35f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
         0.35f, -0.35f, -0.35f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
         0.35f, -0.35f,  0.35f,  1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
         0.35f,  0.35f,  0.35f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

        -0.35f, -0.35f, -0.35f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,
         0.35f, -0.35f, -0.35f,  0.0f, -1.0f,  0.0f,  1.0f,  1.0f,
         0.35f, -0.35f,  0.35f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
         0.35f, -0.35f,  0.35f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
        -0.35f, -0.35f,  0.35f,  0.0f, -1.0f,  0.0f,  0.0f,  0.0f,
        -0.35f, -0.35f, -0.35f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,

        -0.35f,  0.35f, -0.35f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,
         0.35f,  0.35f, -0.35f,  0.0f,  1.0f,  0.0f,  1.0f,  1.0f,
         0.35f,  0.35f,  0.35f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
         0.35f,  0.35f,  0.35f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
        -0.35f,  0.35f,  0.35f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
        -0.35f,  0.35f, -0.35f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f
    };
    private readonly float[] verticesPatas = {
        // positions          // normals           // texture coords
        -0.20f, -0.20f, -0.20f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,
         0.20f, -0.20f, -0.20f,  0.0f,  0.0f, -1.0f,  1.0f,  0.0f,
         0.20f,  0.20f, -0.20f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
         0.20f,  0.20f, -0.20f,  0.0f,  0.0f, -1.0f,  1.0f,  1.0f,
        -0.20f,  0.20f, -0.20f,  0.0f,  0.0f, -1.0f,  0.0f,  1.0f,
        -0.20f, -0.20f, -0.20f,  0.0f,  0.0f, -1.0f,  0.0f,  0.0f,

        -0.20f, -0.20f,  0.20f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,
         0.20f, -0.20f,  0.20f,  0.0f,  0.0f,  1.0f,  1.0f,  0.0f,
         0.20f,  0.20f,  0.20f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
         0.20f,  0.20f,  0.20f,  0.0f,  0.0f,  1.0f,  1.0f,  1.0f,
        -0.20f,  0.20f,  0.20f,  0.0f,  0.0f,  1.0f,  0.0f,  1.0f,
        -0.20f, -0.20f,  0.20f,  0.0f,  0.0f,  1.0f,  0.0f,  0.0f,

        -0.20f,  0.20f,  0.20f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
        -0.20f,  0.20f, -0.20f, -1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
        -0.20f, -0.20f, -0.20f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
        -0.20f, -0.20f, -0.20f, -1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
        -0.20f, -0.20f,  0.20f, -1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
        -0.20f,  0.20f,  0.20f, -1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

         0.20f,  0.20f,  0.20f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,
         0.20f,  0.20f, -0.20f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f,
         0.20f, -0.20f, -0.20f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
         0.20f, -0.20f, -0.20f,  1.0f,  0.0f,  0.0f,  0.0f,  1.0f,
         0.20f, -0.20f,  0.20f,  1.0f,  0.0f,  0.0f,  0.0f,  0.0f,
         0.20f,  0.20f,  0.20f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f,

        -0.20f, -0.20f, -0.20f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,
         0.20f, -0.20f, -0.20f,  0.0f, -1.0f,  0.0f,  1.0f,  1.0f,
         0.20f, -0.20f,  0.20f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
         0.20f, -0.20f,  0.20f,  0.0f, -1.0f,  0.0f,  1.0f,  0.0f,
        -0.20f, -0.20f,  0.20f,  0.0f, -1.0f,  0.0f,  0.0f,  0.0f,
        -0.20f, -0.20f, -0.20f,  0.0f, -1.0f,  0.0f,  0.0f,  1.0f,

        -0.20f,  0.20f, -0.20f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f,
         0.20f,  0.20f, -0.20f,  0.0f,  1.0f,  0.0f,  1.0f,  1.0f,
         0.20f,  0.20f,  0.20f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
         0.20f,  0.20f,  0.20f,  0.0f,  1.0f,  0.0f,  1.0f,  0.0f,
        -0.20f,  0.20f,  0.20f,  0.0f,  1.0f,  0.0f,  0.0f,  0.0f,
        -0.20f,  0.20f, -0.20f,  0.0f,  1.0f,  0.0f,  0.0f,  1.0f
    };

        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);
        private readonly Vector3 _lightPos2 = new Vector3(0.0f,0.0f,0.0f);
        private int _vertexBufferObject;
        private int _vaoModel;
        private int _vaoLamp;
        private int _vertexBufferObject2;
        private int _vaoModel2;
        private int _vertexBufferObject3;
        private int _vaoModel3;
        private int _vertexBufferObject4;
        private int _vaoModel4;
        private int _vertexBufferObject5;
        private int _vaoModel5;
        private int _vertexBufferObject6;
        private int _vaoModel6;
        private int _vertexBufferObject7;
        private int _vaoModel7;
        private int _vertexBufferObject8;
        private int _vaoModel8;
        private Shader _lampShader;
        private Shader _lightingShader;
        private Texture _diffuseMap;
        private Texture _specularMap;
        private Shader _lightingShader2;
        private Texture _diffuseMap2;
        private Texture _specularMap2;
        private Shader _lightingShader3;
        private Texture _diffuseMap3;
        private Texture _specularMap3;
        private Shader _lightingShader4;
        private Texture _diffuseMap4;
        private Texture _specularMap4;
        private Shader _lightingShader5;
        private Texture _diffuseMap5;
        private Texture _specularMap5;

        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private bool flag;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            //Cara Creeper
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            _lightingShader = new Shader(".\\Shaders\\vert.glsl",".\\Shaders\\lightFrag.glsl");
            _diffuseMap = new Texture(".\\Textures\\CreeperFace.png");
            _specularMap = new Texture(".\\Textures\\CreeperFace.png");
            _vaoModel = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            var positionLocation = _lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation = _lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //cuerpo Creeper1
            _vertexBufferObject2 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject2);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesBody.Length * sizeof(float), verticesBody, BufferUsageHint.StaticDraw);
            _lightingShader2 = new Shader(".\\Shaders\\vert.glsl",".\\Shaders\\lightFrag.glsl");
            _diffuseMap2 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _specularMap2 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _vaoModel2 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject2);
            var positionLocation2 = _lightingShader2.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation2);
            GL.VertexAttribPointer(positionLocation2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation2 = _lightingShader2.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation2);
            GL.VertexAttribPointer(normalLocation2, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation2 = _lightingShader2.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation2);
            GL.VertexAttribPointer(texCoordLocation2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Cuerpo 2
            _vertexBufferObject3 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject3);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesBody.Length * sizeof(float), verticesBody , BufferUsageHint.StaticDraw);
            _lightingShader3 = new Shader(".\\Shaders\\vert.glsl",".\\Shaders\\lightFrag.glsl");
            _diffuseMap3 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _specularMap3 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _vaoModel3 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel3);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject3);
            var positionLocation3 = _lightingShader3.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation3);
            GL.VertexAttribPointer(positionLocation3, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation3 = _lightingShader3.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation3);
            GL.VertexAttribPointer(normalLocation3, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation3 = _lightingShader3.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation3);
            GL.VertexAttribPointer(texCoordLocation3, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Cuerpo 3
            _vertexBufferObject4 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject4);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesBody.Length * sizeof(float), verticesBody , BufferUsageHint.StaticDraw);
            _lightingShader4 = new Shader(".\\Shaders\\vert.glsl",".\\Shaders\\lightFrag.glsl");
            _diffuseMap4 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _specularMap4 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _vaoModel4 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel4);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject4);
            var positionLocation4 = _lightingShader4.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation4);
            GL.VertexAttribPointer(positionLocation4, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation4 = _lightingShader4.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation4);
            GL.VertexAttribPointer(normalLocation4, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation4 = _lightingShader4.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation4);
            GL.VertexAttribPointer(texCoordLocation4, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Pata 1
            _vertexBufferObject5 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject5);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesPatas.Length * sizeof(float), verticesPatas , BufferUsageHint.StaticDraw);
            _lightingShader5 = new Shader(".\\Shaders\\vert.glsl",".\\Shaders\\lightFrag.glsl");
            _diffuseMap5 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _specularMap5 = new Texture(".\\Textures\\CuerpoCreeper.png");
            _vaoModel5 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel5);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject5);
            var positionLocation5 = _lightingShader5.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation5);
            GL.VertexAttribPointer(positionLocation5, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation5 = _lightingShader5.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation5);
            GL.VertexAttribPointer(normalLocation5, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation5 = _lightingShader5.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation5);
            GL.VertexAttribPointer(texCoordLocation5, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Pata 2
            _vertexBufferObject6 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject6);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesPatas.Length * sizeof(float), verticesPatas , BufferUsageHint.StaticDraw);
            _vaoModel6 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel6);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject6);
            var positionLocation6 = _lightingShader5.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation6);
            GL.VertexAttribPointer(positionLocation6, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation6 = _lightingShader5.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation6);
            GL.VertexAttribPointer(normalLocation5, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation6 = _lightingShader5.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation6);
            GL.VertexAttribPointer(texCoordLocation6, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Pata 3
            _vertexBufferObject7 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject7);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesPatas.Length * sizeof(float), verticesPatas , BufferUsageHint.StaticDraw);
            _vaoModel7 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel7);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject7);
            var positionLocation7 = _lightingShader5.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation7);
            GL.VertexAttribPointer(positionLocation7, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation7 = _lightingShader5.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation7);
            GL.VertexAttribPointer(normalLocation7, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation7 = _lightingShader5.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation7);
            GL.VertexAttribPointer(texCoordLocation7, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Pata 4
            _vertexBufferObject8 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject8);
            GL.BufferData(BufferTarget.ArrayBuffer, verticesPatas.Length * sizeof(float), verticesPatas , BufferUsageHint.StaticDraw);
            _vaoModel8 = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel8);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject8);
            var positionLocation8 = _lightingShader5.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation8);
            GL.VertexAttribPointer(positionLocation8, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var normalLocation8 = _lightingShader5.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation8);
            GL.VertexAttribPointer(normalLocation8, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            var texCoordLocation8 = _lightingShader5.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation8);
            GL.VertexAttribPointer(texCoordLocation8, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //Lighting Cube
            _lampShader = new Shader(".\\Shaders\\vert.glsl",".\\Shaders\\frag.glsl");
            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            positionLocation = _lampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            CursorGrabbed = true;
            flag = false;

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 creeperHead = Matrix4.Identity;
            Matrix4 creeperBody1 = Matrix4.Identity;
            Matrix4 creeperBody2 = Matrix4.Identity;
            Matrix4 creeperBody3 = Matrix4.Identity;
            Matrix4 creeperPata1 = Matrix4.Identity;
            Matrix4 creeperPata2 = Matrix4.Identity;
            Matrix4 creeperPata3 = Matrix4.Identity;
            Matrix4 creeperPata4 = Matrix4.Identity;

            if(!flag) {
                creeperHead *= Matrix4.CreateTranslation(0.0f,-10.0f,0.0f);
                creeperBody1 *= Matrix4.CreateTranslation(0.0f,-10.0f,0.0f);
                creeperBody2 *= Matrix4.CreateTranslation(0.0f,-10.7f,0.0f);
                creeperBody3 *= Matrix4.CreateTranslation(0.0f,-9.075f,0.0f);
                creeperPata1 *= Matrix4.CreateTranslation(-7.65f,-2.375f, 0.35f);
                creeperPata2 *= Matrix4.CreateTranslation(-7.65f,-2.375f, -0.35f);
                creeperPata3 *= Matrix4.CreateTranslation(7.65f,-2.375f, 0.35f);
                creeperPata4 *= Matrix4.CreateTranslation(7.65f,-2.375f, -0.35f);
                flag = true;
            }
            else {
                if(GLFW.GetTime()<10) {
                    creeperHead *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-10.0f,0.0f);
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-10.0f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-10.7f,0.0f);
                    creeperBody3 *= Matrix4.CreateTranslation(0.0f,-9.075f,0.0f);
                    creeperPata1 *= Matrix4.CreateTranslation(-7.65f,-2.375f, 0.35f);
                    creeperPata2 *= Matrix4.CreateTranslation(-7.65f,-2.375f, -0.35f);
                    creeperPata3 *= Matrix4.CreateTranslation(7.65f,-2.375f, 0.35f);
                    creeperPata4 *= Matrix4.CreateTranslation(7.65f,-2.375f, -0.35f);
                }
                else if(GLFW.GetTime()<18.0) {
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-18.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-10.7f,0.0f);
                    creeperBody3 *= Matrix4.CreateTranslation(0.0f,-9.075f,0.0f);
                    creeperPata1 *= Matrix4.CreateTranslation((float)GLFW.GetTime()-17.65f,-2.375f, 0.35f);
                    creeperPata2 *= Matrix4.CreateTranslation(-7.65f,-2.375f, -0.35f);
                    creeperPata3 *= Matrix4.CreateTranslation(17.65f-(float)GLFW.GetTime(),-2.375f, 0.35f);
                    creeperPata4 *= Matrix4.CreateTranslation(7.65f,-2.375f, -0.35f);
                }
                else if(GLFW.GetTime()<23){
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-0.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-24.375f,0.0f);
                    creeperBody3 *= Matrix4.CreateTranslation(0.0f,-9.075f,0.0f);
                    creeperPata1 *= Matrix4.CreateTranslation(0.35f,-2.375f, 0.35f);
                    creeperPata2 *= Matrix4.CreateTranslation(-7.65f,-2.375f, -0.35f);
                    creeperPata3 *= Matrix4.CreateTranslation(-0.35f,-2.375f, 0.35f);
                    creeperPata4 *= Matrix4.CreateTranslation(7.65f,-2.375f, -0.35f);
                }
                else if(GLFW.GetTime()<31){ 
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-0.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-1.375f,0.0f);
                    creeperBody3 *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-33.075f,0.0f);
                    creeperPata1 *= Matrix4.CreateTranslation(0.35f,-2.375f, 0.35f);
                    creeperPata2 *= Matrix4.CreateTranslation((float)GLFW.GetTime()-30.65f,-2.375f, -0.35f);
                    creeperPata3 *= Matrix4.CreateTranslation(-0.35f,-2.375f, 0.35f);
                    creeperPata4 *= Matrix4.CreateTranslation(30.65f-(float)GLFW.GetTime(),-2.375f, -0.35f);
                }
                else {
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-0.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-1.375f,0.0f);
                    creeperBody3 *= Matrix4.CreateTranslation(0.0f,-2.075f,0.0f);
                    creeperPata1 *= Matrix4.CreateTranslation(0.35f,-2.375f, 0.35f);
                    creeperPata2 *= Matrix4.CreateTranslation(-0.35f,-2.375f, -0.35f);
                    creeperPata3 *= Matrix4.CreateTranslation(-0.35f,-2.375f, 0.35f);
                    creeperPata4 *= Matrix4.CreateTranslation(0.35f,-2.375f, -0.35f);
                }
            }
            int i=0;
            //Creeper Face
            GL.BindVertexArray(_vaoModel);
            _diffuseMap.Use();
            _specularMap.Use(TextureUnit.Texture1);
            _lightingShader.Use();
            _lightingShader.SetMatrix4("model", creeperHead);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader.SetVector3("viewPos", _camera.Position);
            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);
            _lightingShader.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader.SetVector3("spotLight.position", _camera.Position);
            _lightingShader.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
            _lightingShader.SetVector3("spotLight.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetFloat("spotLight.constant", 1.0f);
            _lightingShader.SetFloat("spotLight.linear", 0.09f);
            _lightingShader.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader.SetVector3("spotLight2.position", _lightPos);
            _lightingShader.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader.SetVector3("spotLight2.ambient", new Vector3(0.0f, 0.0f, 0.0f));
            _lightingShader.SetVector3("spotLight2.diffuse", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader.SetFloat("spotLight2.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            
            //Creeper body1
            GL.BindVertexArray(_vaoModel2);
            _diffuseMap2.Use();
            _specularMap2.Use(TextureUnit.Texture1);
            _lightingShader2.Use();
            _lightingShader2.SetMatrix4("model", creeperBody1);
            _lightingShader2.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader2.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader2.SetVector3("viewPos", _camera.Position);
            _lightingShader2.SetInt("material.diffuse", 0);
            _lightingShader2.SetInt("material.specular", 1);
            _lightingShader2.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader2.SetFloat("material.shininess", 32.0f);
            _lightingShader2.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader2.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader2.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader2.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader2.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader2.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader2.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader2.SetVector3("spotLight.position", _camera.Position);
            _lightingShader2.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader2.SetVector3("spotLight.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader2.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader2.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader2.SetFloat("spotLight.constant", 1.0f);
            _lightingShader2.SetFloat("spotLight.linear", 0.09f);
            _lightingShader2.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader2.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader2.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader2.SetVector3("spotLight2.position", _lightPos);
            _lightingShader2.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader2.SetVector3("spotLight2.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader2.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader2.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader2.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader2.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader2.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader2.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader2.SetFloat("spotLight2.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

             //Creeper body2
            GL.BindVertexArray(_vaoModel3);
            _diffuseMap3.Use();
            _specularMap3.Use(TextureUnit.Texture1);
            _lightingShader3.Use();
            _lightingShader3.SetMatrix4("model", creeperBody2);
            _lightingShader3.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader3.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader3.SetVector3("viewPos", _camera.Position);
            _lightingShader3.SetInt("material.diffuse", 0);
            _lightingShader3.SetInt("material.specular", 1);
            _lightingShader3.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader3.SetFloat("material.shininess", 32.0f);
            _lightingShader3.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader3.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader3.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader3.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader3.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader3.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader3.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader3.SetVector3("spotLight.position", _camera.Position);
            _lightingShader3.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader3.SetVector3("spotLight.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader3.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader3.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader3.SetFloat("spotLight.constant", 1.0f);
            _lightingShader3.SetFloat("spotLight.linear", 0.09f);
            _lightingShader3.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader3.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader3.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader3.SetVector3("spotLight2.position", _lightPos);
            _lightingShader3.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader3.SetVector3("spotLight2.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader3.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader3.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader3.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader3.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader3.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader3.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader3.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            //Creeper body3
            GL.BindVertexArray(_vaoModel4);
            _diffuseMap4.Use();
            _specularMap4.Use(TextureUnit.Texture1);
            _lightingShader4.Use();
            _lightingShader4.SetMatrix4("model", creeperBody3);
            _lightingShader4.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader4.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader4.SetVector3("viewPos", _camera.Position);
            _lightingShader4.SetInt("material.diffuse", 0);
            _lightingShader4.SetInt("material.specular", 1);
            _lightingShader4.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader4.SetFloat("material.shininess", 32.0f);
            _lightingShader4.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader4.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader4.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader4.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader4.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader4.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader4.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader4.SetVector3("spotLight.position", _camera.Position);
            _lightingShader4.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader4.SetVector3("spotLight.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader4.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader4.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader4.SetFloat("spotLight.constant", 1.0f);
            _lightingShader4.SetFloat("spotLight.linear", 0.09f);
            _lightingShader4.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader4.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader4.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader4.SetVector3("spotLight2.position", _lightPos);
            _lightingShader4.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader4.SetVector3("spotLight2.ambient", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader4.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader4.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader4.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader4.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader4.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader4.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader4.SetFloat("spotLight2.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            //Creeper pata1
            GL.BindVertexArray(_vaoModel5);
            _diffuseMap5.Use();
            _specularMap5.Use(TextureUnit.Texture1);
            _lightingShader5.Use();
            _lightingShader5.SetMatrix4("model", creeperPata1);
            _lightingShader5.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader5.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader5.SetVector3("viewPos", _camera.Position);
            _lightingShader5.SetInt("material.diffuse", 0);
            _lightingShader5.SetInt("material.specular", 1);
            _lightingShader5.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader5.SetFloat("material.shininess", 32.0f);
            _lightingShader5.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader5.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader5.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader5.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader5.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader5.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader5.SetVector3("spotLight.position", _camera.Position);
            _lightingShader5.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader5.SetVector3("spotLight.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetVector3("spotLight2.position", _lightPos);
            _lightingShader5.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader5.SetVector3("spotLight2.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            //Creeper pata2
            GL.BindVertexArray(_vaoModel6);
            _diffuseMap5.Use();
            _specularMap5.Use(TextureUnit.Texture1);
            _lightingShader5.Use();
            _lightingShader5.SetMatrix4("model", creeperPata2);
            _lightingShader5.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader5.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader5.SetVector3("viewPos", _camera.Position);
            _lightingShader5.SetInt("material.diffuse", 0);
            _lightingShader5.SetInt("material.specular", 1);
            _lightingShader5.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader5.SetFloat("material.shininess", 32.0f);
            _lightingShader5.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader5.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader5.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader5.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader5.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader5.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader5.SetVector3("spotLight.position", _camera.Position);
            _lightingShader5.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader5.SetVector3("spotLight.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetVector3("spotLight2.position", _lightPos);
            _lightingShader5.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader5.SetVector3("spotLight2.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight2.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            //Creeper pata3
            GL.BindVertexArray(_vaoModel7);
            _diffuseMap5.Use();
            _specularMap5.Use(TextureUnit.Texture1);
            _lightingShader5.Use();
            _lightingShader5.SetMatrix4("model", creeperPata3);
            _lightingShader5.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader5.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader5.SetVector3("viewPos", _camera.Position);
            _lightingShader5.SetInt("material.diffuse", 0);
            _lightingShader5.SetInt("material.specular", 1);
            _lightingShader5.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader5.SetFloat("material.shininess", 32.0f);
            _lightingShader5.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader5.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader5.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader5.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader5.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader5.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader5.SetVector3("spotLight.position", _camera.Position);
            _lightingShader5.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader5.SetVector3("spotLight.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetVector3("spotLight2.position", _lightPos);
            _lightingShader5.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader5.SetVector3("spotLight2.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight2.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            //Creeper pata4
            GL.BindVertexArray(_vaoModel8);
            _diffuseMap5.Use();
            _specularMap5.Use(TextureUnit.Texture1);
            _lightingShader5.Use();
            _lightingShader5.SetMatrix4("model", creeperPata4);
            _lightingShader5.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader5.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader5.SetVector3("viewPos", _camera.Position);
            _lightingShader5.SetInt("material.diffuse", 0);
            _lightingShader5.SetInt("material.specular", 1);
            _lightingShader5.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader5.SetFloat("material.shininess", 32.0f);
            _lightingShader5.SetVector3($"pointLights[{i}].position", _lightPos);
            _lightingShader5.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader5.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
            _lightingShader5.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat($"pointLights[{i}].constant", 1.0f);
            _lightingShader5.SetFloat($"pointLights[{i}].linear", 0.09f);
            _lightingShader5.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            _lightingShader5.SetVector3("spotLight.position", _camera.Position);
            _lightingShader5.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader5.SetVector3("spotLight.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetVector3("spotLight2.position", _lightPos);
            _lightingShader5.SetVector3("spotLight2.direction", _lightPos2);
            _lightingShader5.SetVector3("spotLight2.ambient", new Vector3(0.7f, 0.7f, 0.7f));
            _lightingShader5.SetVector3("spotLight2.diffuse", new Vector3(0.2f, 0.2f, 0.2f));
            _lightingShader5.SetVector3("spotLight2.specular", new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader5.SetFloat("spotLight2.constant", 1.0f);
            _lightingShader5.SetFloat("spotLight2.linear", 0.09f);
            _lightingShader5.SetFloat("spotLight2.quadratic", 0.032f);
            _lightingShader5.SetFloat("spotLight2.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader5.SetFloat("spotLight2.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            
            //Light Cube
            _lampShader.Use();
            Matrix4 lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f);
            lampMatrix *= Matrix4.CreateTranslation(_lightPos);
            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
          
            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }
            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _camera.Fov -= e.OffsetY;
            base.OnMouseWheel(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vaoModel);
            GL.DeleteVertexArray(_vaoLamp);

            GL.DeleteProgram(_lampShader.Handle);
            GL.DeleteProgram(_lightingShader.Handle);

            base.OnUnload();
        }
    }
}