using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
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
        InputMontior input;
        public Camera camera;

        public SpriteBatch spriteBatch;
        public Hud hudview;
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

  
        }
        protected override void Initialize()
        {
            GameServices.AddService<GraphicsDevice>(GraphicsDevice);
            GameServices.AddService<ContentManager>(Content);
            spriteBatch = new SpriteBatch(this.GraphicsDevice);
            input = InputMontior.Instance;
            Vector2 viewportSize = new Vector2(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);
            camera = new Camera(viewportSize);
            hudview = Hud.instance;
            hudview.Initilize(this, spriteBatch);
            base.Initialize();

        }
        protected override void LoadContent()
        {
            
        }
        protected override void UnloadContent()
        {

        }
        protected override void Update(GameTime gameTime)
        {
            camera.Update();
            hudview.Update();
            input.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            // Set the vertex buffer on the GraphicsDevice

            // TODO: Add your drawing code here
            hudview.Draw();
            base.Draw(gameTime);
        }


    }
    #region unit test class
    [TestFixture]
    public class UnitTests : TestGame
    {

        Sprite2DManager testSpriteManager;
        Sprite3DManager sprite3DManager;

        protected override void Initialize()
        {
            

            LogHelper.Write("Window Test Game Initialize...");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LogHelper.Write("Window Test Game LoadContent...");
            testSpriteManager = Sprite2DManager.Instance;
            sprite3DManager = new Sprite3DManager(camera);
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




            sprite3DManager.CreateSprite3D(@"Character01/test02", new Point(50, 50), new Point(4, 5));

            this.Components.Add(new FrameRateCounter(this, spriteBatch));
            base.LoadContent();

        }
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            testSpriteManager.Update();
            sprite3DManager.Update();
            //hudview.addHudItem("sprite3D.currentFrame", testSprite3D[i].currentFrame.ToString());

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sprite3DManager.Draw();
            testSpriteManager.Draw();
            base.Draw(gameTime);

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
