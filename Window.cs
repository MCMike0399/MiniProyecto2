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

        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private int _vertexBufferObject;
        private int _vaoModel;
        private int _vaoLamp;
        private int _vertexBufferObject2;
        private int _vaoModel2;
        private int _vertexBufferObject3;
        private int _vaoModel3;
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
            _lightingShader = new Shader("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\vert.glsl","C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\lightFrag.glsl");
            _diffuseMap = new Texture("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Textures\\CreeperFace.png");
            _specularMap = new Texture("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Textures\\CreeperFace.png");
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
            _lightingShader2 = new Shader("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\vert.glsl","C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\lightFrag.glsl");
            _diffuseMap2 = new Texture("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Textures\\CuerpoCreeper.png");
            _specularMap2 = new Texture("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Textures\\CuerpoCreeper.png");
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
            _lightingShader3 = new Shader("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\vert.glsl","C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\lightFrag.glsl");
            _diffuseMap3 = new Texture("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Textures\\CuerpoCreeper.png");
            _specularMap3 = new Texture("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Textures\\CuerpoCreeper.png");
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

            //Lighting Cube
            _lampShader = new Shader("C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\vert.glsl","C:\\Users\\maqui\\Documents\\CreeperModelaje\\Shaders\\frag.glsl");
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
            if(!flag) {
                creeperHead *= Matrix4.CreateTranslation(0.0f,-10.0f,0.0f);
                creeperBody1 *= Matrix4.CreateTranslation(0.0f,-10.0f,0.0f);
                creeperBody2 *= Matrix4.CreateTranslation(0.0f,-10.7f,0.0f);
                flag = true;
            }
            else {
                if(GLFW.GetTime()<10) {
                    creeperHead *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-10.0f,0.0f);
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-10.0f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-10.7f,0.0f);
                }
                else if(GLFW.GetTime()<18.0) {
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-18.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-10.7f,0.0f);
                }
                else if(GLFW.GetTime()<23){
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-0.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,(float)GLFW.GetTime()-24.375f,0.0f);
                }
                else {
                    creeperBody1 *= Matrix4.CreateTranslation(0.0f,-0.675f,0.0f);
                    creeperBody2 *= Matrix4.CreateTranslation(0.0f,-1.375f,0.0f);
                }
            }
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
            _lightingShader.SetVector3("light.position", _lightPos);
            _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
            _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            
            //Creeper body1
            GL.BindVertexArray(_vaoModel2);
            _diffuseMap2.Use();
            _specularMap2.Use(TextureUnit.Texture1);
            _lightingShader.Use();
            _lightingShader.SetMatrix4("model", creeperBody1);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _lightingShader.SetVector3("viewPos", _camera.Position);
            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);
            _lightingShader.SetVector3("light.position", _lightPos);
            _lightingShader.SetVector3("light.ambient", new Vector3(0.2f));
            _lightingShader.SetVector3("light.diffuse", new Vector3(0.5f));
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));
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
            _lightingShader3.SetVector3("light.position", _lightPos);
            _lightingShader3.SetVector3("light.ambient", new Vector3(0.2f));
            _lightingShader3.SetVector3("light.diffuse", new Vector3(0.5f));
            _lightingShader3.SetVector3("light.specular", new Vector3(1.0f));
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