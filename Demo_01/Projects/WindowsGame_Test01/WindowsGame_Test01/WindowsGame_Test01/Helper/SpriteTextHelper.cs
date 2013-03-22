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


    public class SpriteTextHelper
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private List<SpriteText> spriteTexts;
        private static SpriteTextHelper spriteTextHelper;

        private SpriteTextHelper()
        {
            spriteTexts = new List<SpriteText>();
        }
        public static SpriteTextHelper Instance
        {
            get
            {
                if (spriteTextHelper == null)
                    spriteTextHelper = new SpriteTextHelper();
                return spriteTextHelper;
            }
        }
        public void Initialize(Game setGame)
        {
            game = setGame;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public SpriteText CreateSpriteText(string setText, Vector2 setPosition, string spriteFontFileName)
        {
            SpriteFont spriteFont=game.Content.Load<SpriteFont>(spriteFontFileName);
            SpriteText spriteText = new SpriteText(setText, setPosition, spriteFont);
            spriteTexts.Add(spriteText);
            return spriteText;
        }
        public void Draw()
        {
            spriteTexts.Sort((s1, s2) => s1.sortID.CompareTo(s2.sortID));
            spriteBatch.Begin(SpriteSortMode.FrontToBack,BlendState.AlphaBlend);
            foreach (SpriteText st in spriteTexts)
            {
                spriteBatch.DrawString(st.spriteFont, st.text, st.position, Color.White);
            }
            spriteBatch.End();  
        }
    }


    #region unit test class
    [TestFixture]
    public class SpriteTextHelperTests : TestGame
    {
        SpriteTextHelper testSpriteTextHelper;
        protected override void Initialize()
        {
            testSpriteTextHelper = SpriteTextHelper.Instance;
            testSpriteTextHelper.Initialize(this);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            testSpriteTextHelper.CreateSpriteText("Hello world", new Vector2(110, 110), @"Fonts\Texture");
            testSpriteTextHelper.CreateSpriteText("good morning", new Vector2(110, 310), @"Fonts\Texture");
        }
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);
            testSpriteTextHelper.Draw();


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
