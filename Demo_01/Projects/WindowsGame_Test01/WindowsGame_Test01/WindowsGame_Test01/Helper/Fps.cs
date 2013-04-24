using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace WindowsGame_Test01.Helper
{
    public class FrameRateCounter : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            spriteFont = game.Content.Load<SpriteFont>(@"Fonts/sfText");
            this.spriteBatch = spriteBatch;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("FPS: {0}", frameRate);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, new Vector2(20, 0), Color.White);    // 坐标根据需要自行修改

            spriteBatch.End();
        }
    }
}
