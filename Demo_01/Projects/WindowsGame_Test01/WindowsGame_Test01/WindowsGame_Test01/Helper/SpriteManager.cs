using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if DEBUG
    using NUnit.Framework;
#endif


namespace WindowsGame_Test01.Helper
{
    public enum RenderPass
    {
        Background,
        Environment,
        Charactor,
        Effect,
        User
    }


    public class SpriteManager
    {
        private SpriteBatch spriteBatch;
        private Game game;

        private List<Sprite> SpriteList;
        private List<Texture2D> textures;

        private static SpriteManager spriteManager;
        private SpriteManager() 
        {
            SpriteList = new List<Sprite>();
            textures = new List<Texture2D>();
        }

        public static SpriteManager Instance
        {
            get 
            {
                if (spriteManager == null)
                    spriteManager = new SpriteManager();
                return spriteManager;
            }
        }

        public void Initialize(Game setGame)
        {
            game = setGame;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            
        }

        public Sprite CreateSimpleSprite(string texFileName, Rectangle setRect, Rectangle? setSourceRect,RenderPass setRenderPass)
        {
            Texture2D newTex=game.Content.Load<Texture2D>(texFileName);
            ImageSprite sprite = new ImageSprite(newTex, setRect, setSourceRect, setRenderPass);
            SpriteList.Add(sprite);
            textures.Add(newTex);
            LogHelper.Write("Create a Simple Sprite " + texFileName);
            return sprite;
        }

        public Sprite CreateAnimateSprite(string texFileName, Rectangle setRect, Point setFrameSize, RenderPass setRenderPass)
        {
            Texture2D newTex = game.Content.Load<Texture2D>(texFileName);
            AnimateSprite sprite = new AnimateSprite(newTex, setRect, setFrameSize, setRenderPass);
            SpriteList.Add(sprite);
            textures.Add(newTex);
            
            LogHelper.Write("Create a Animated Sprite " + texFileName);
            return sprite;
        }

        public SpriteText CreateSpriteText(string setText, Vector2 setPosition, string spriteFontFileName, RenderPass setRenderPass)
        {
            SpriteFont spriteFont = game.Content.Load<SpriteFont>(spriteFontFileName);
            SpriteText spriteText = new SpriteText(setText, setPosition, spriteFont);
            spriteText.renderPass = setRenderPass;
            SpriteList.Add(spriteText);
            LogHelper.Write("Create a Sprite Text " + spriteText);
            return spriteText;
        }

        public void Update() 
        {
            foreach (Sprite sprite in SpriteList) 
            {
                sprite.Update();
            }
        }

        public void Draw(BasicEffect effect)
        {
            foreach (RenderPass pass in Enum.GetValues(typeof(RenderPass)))
            {
                List<Sprite> renderPassList = SpriteList.FindAll(r => r.renderPass == pass);
                renderPassList.Sort((r1, r2) => r2.sortID.CompareTo(r1.sortID));
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise, effect);
                //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                foreach (Sprite s in renderPassList)
                {
                    s.SpriteDraw(spriteBatch);       
                }
                spriteBatch.End();
            }
        }
    }


#region unit test class
    [TestFixture]
    public class SpriteManagerTests : TestGame
    {
        
        SpriteManager testSpriteManager;
        SpriteFont spriteFont;
        protected override void Initialize()
        {
            testSpriteManager = SpriteManager.Instance;
            testSpriteManager.Initialize(this);
            
            base.Initialize();
            LogHelper.Write("Window Test Game Initialize...");
            

        }
        
        protected override void LoadContent()
        {
            LogHelper.Write("Window Test Game LoadContent...");
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(0, 0, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(80, 0, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(220, 0, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(0, 0, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(80, 90, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(220, 180, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(320, 280, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(120, 280, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateSimpleSprite(@"test/test02", new Rectangle(220, 280, 64, 64), null, RenderPass.Background);
            testSpriteManager.CreateAnimateSprite(@"Character01/test02", new Rectangle(320, 280, 150, 150), new Point(4, 5), RenderPass.Charactor);
            testSpriteManager.CreateAnimateSprite(@"Character01/test02", new Rectangle(260, 280, 150, 150), new Point(4, 5), RenderPass.Charactor);
            testSpriteManager.CreateAnimateSprite(@"Character01/test02", new Rectangle(120, 280, 150, 150), new Point(4, 5), RenderPass.Charactor);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(110, 110), @"Fonts\Texture", RenderPass.Background);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(210, 110), @"Fonts\Texture", RenderPass.Background);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(310, 110), @"Fonts\Texture", RenderPass.Background);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(310, 210), @"Fonts\Texture", RenderPass.Background);
            testSpriteManager.CreateSpriteText("Hello world", new Vector2(310, 310), @"Fonts\Texture", RenderPass.Background);
            testSpriteManager.CreateSpriteText("Good morning", new Vector2(210, 410), @"Fonts\sfText", RenderPass.Charactor);
            testSpriteManager.CreateSpriteText("Good Afternoon", new Vector2(510, 310), @"Fonts\sfText", RenderPass.Charactor);
            spriteFont = Content.Load<SpriteFont>(@"Fonts\sfText");
            base.LoadContent();
            
        }
        protected override void Update(GameTime gameTime)
        {
            testSpriteManager.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            testSpriteManager.Draw(base.effect);
            base.Draw(gameTime);
            SpriteBatch spriteBatch=new SpriteBatch(this.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise, base.effect);
            spriteBatch.DrawString(spriteFont, "hello, world!", Vector2.Zero, Color.White, 0, Vector2.Zero, 0.01f, 0, 0);

            spriteBatch.End();

            


            
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
