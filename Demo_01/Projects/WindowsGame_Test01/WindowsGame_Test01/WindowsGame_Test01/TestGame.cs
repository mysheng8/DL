using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame_Test01.Helper;
using WindowsGame_Test01.Data;

#if DEBUG
using NUnit.Framework;
#endif

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
    #region unit test class
    [TestFixture]
    public class UnitTests : TestGame
    {

        Sprite2DManager testSpriteManager;
        Sprite3D[] testSprite3D;
        int n = 1;
        protected override void Initialize()
        {
            testSpriteManager = Sprite2DManager.Instance;
            testSpriteManager.Initialize(this);

            base.Initialize();
            LogHelper.Write("Window Test Game Initialize...");
            

        }

        protected override void LoadContent()
        {
            LogHelper.Write("Window Test Game LoadContent...");


            //Map map = new Map();

            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(0, 0, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(80, 0, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(220, 0, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(0, 0, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(80, 90, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(220, 180, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(320, 280, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(120, 280, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(220, 280, 64, 64), null, RenderPass.Environment);
            testSpriteManager.CreateAnimateSprite(@"Character01/test01", new Rectangle(320, 280, 150, 150), new Point(5, 3), RenderPass.Charactor);
            testSpriteManager.CreateAnimateSprite(@"Character01/test02", new Rectangle(260, 280, 150, 150), new Point(4, 5), RenderPass.Charactor);
            testSpriteManager.CreateAnimateSprite(@"Character01/test02", new Rectangle(120, 280, 150, 150), new Point(4, 5), RenderPass.Charactor);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(110, 110), @"Fonts\Texture", RenderPass.User);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(210, 110), @"Fonts\Texture", RenderPass.User);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(310, 110), @"Fonts\Texture", RenderPass.User);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(310, 210), @"Fonts\Texture", RenderPass.User);

            testSpriteManager.CreateSpriteText("Good morning", new Vector2(210, 410), @"Fonts\sfText", RenderPass.Charactor);
            
            Sprite2D test = testSpriteManager.CreateSpriteText("Good Afternoon", new Vector2(510, 310), @"Fonts\sfText", RenderPass.Charactor);
            testSpriteManager.RemoveSpirte(test);



            Texture2D idleTex = Content.Load<Texture2D>(@"Character01/test01");
            Clip idle = new Clip(idleTex, new Point(5, 3));
            Texture2D runTex = Content.Load<Texture2D>(@"Character01/test02");
            Clip run = new Clip(runTex, new Point(4, 5));
            Texture2D attTex = Content.Load<Texture2D>(@"Character01/test03");
            Clip attack1 = new Clip(attTex, new Point(5, 10));
            Character a = new Character("a", new Rectangle(420, 280, 150, 150), idle, run, attack1);



            Texture2D testTex = Content.Load<Texture2D>(@"test/test01");


            testSprite3D = new Sprite3D[n];
            for (int i = 0; i < n; i++)
                testSprite3D[i] = new Sprite3D(testTex, this.GraphicsDevice);

            SpriteBatch spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Components.Add(new FrameRateCounter(this, spriteBatch));
            base.LoadContent();

        }
        protected override void Update(GameTime gameTime)
        {
            testSpriteManager.Update();
            for (int i = 0; i < n; i++)
                testSprite3D[i].Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            for (int i = 0; i < n; i++)
                testSprite3D[i].Draw();
            testSpriteManager.Draw();
            
            base.Draw(gameTime);
            //SpriteBatch spriteBatch=new SpriteBatch(this.GraphicsDevice);
            //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise);
            //spriteBatch.DrawString(spriteFont, "hello, world!", Vector2.Zero, Color.White, 0, Vector2.Zero, 0.01f, 0, 0);

            // spriteBatch.End();





        }
        /*
              [Test]
              public void ()
              {

              } 
        */
    }
    #endregion

}
