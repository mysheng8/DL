using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame_Test01.Helper;

namespace WindowsGame_Test01
{
    public class TestGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        InputMontior inputMontior;
        public  BasicEffect effect;
        Camera camera;

        VertexPositionColor[] verts;
        VertexBuffer vertexBuffer;
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            camera = new Camera(this);
            this.Components.Add(camera);
        }
        protected override void Initialize()
        {
            effect = new BasicEffect(GraphicsDevice);
            inputMontior = InputMontior.Instance;
            base.Initialize();

        }
        protected override void LoadContent()
        {

            
            verts = new VertexPositionColor[4];
            verts[0] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Blue);
            verts[1] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Red);
            verts[2] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Green);


            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
            vertexBuffer.SetData(verts);

            
            
            
        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            inputMontior.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            
            


            // Set the vertex buffer on the GraphicsDevice
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = true;
            // TODO: Add your drawing code here
            effect.World = Matrix.Identity;
            effect.View = camera.viewMatrix;
            effect.Projection = camera.projection;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
               // GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, verts, 0, 2);
            }
            base.Draw(gameTime);
        }


    }
}
