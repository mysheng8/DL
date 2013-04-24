using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame_Test01.Helper
{
    public class Hud : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Queue<string> hudList;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public Hud(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            spriteFont = game.Content.Load<SpriteFont>(@"Fonts/sfText");
            this.spriteBatch = spriteBatch;
            hudList = new Queue<string>();
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
        public void addHudItem(string item, string value)
        {
            string consoleLine=string.Format((item+": {0}"), value);
            hudList.(consoleLine);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < hudList.Count;i++ )
            {
                spriteBatch.DrawString(spriteFont, hudList[i], new Vector2(20*(i+1), 20), Color.White);

            }
            spriteBatch.End();
        }
    }
}
