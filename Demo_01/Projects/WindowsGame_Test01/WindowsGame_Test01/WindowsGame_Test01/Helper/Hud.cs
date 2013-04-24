using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame_Test01.Helper
{
    public class Hud 
    {
        private static Hud writer = null;
        private  Hud() { }
        public static Hud instance
        {
            get
            {
                if (writer == null)
                {
                    writer = new Hud();
                }
                return writer;
            }
        }
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Queue<string> hudList;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public void Initilize(Game game, SpriteBatch spriteBatch)
        {
            spriteFont = game.Content.Load<SpriteFont>(@"Fonts/Hud");
            this.spriteBatch = spriteBatch;
            hudList = new Queue<string>();
        }

        public void Update()
        {
            if (hudList.Count > 10) 
            {
                hudList.Dequeue();
            }
        }
        public void addHudItem(string item, string value)
        {
            DateTime ct = DateTime.Now;
            string s = "[" + ct.Hour.ToString("00") + ":" +
              ct.Minute.ToString("00") + ":" +
              ct.Second.ToString("00") + "] ";
              ;
            string consoleLine=string.Format((s+item+": {0}"), value);
            hudList.Enqueue(consoleLine);
        }

        public void Draw()
        {
            spriteBatch.Begin();
            for (int i = 0; i < hudList.Count;i++ )
            {
                spriteBatch.DrawString(spriteFont, hudList.ElementAt(i), new Vector2(20,20+12*i), Color.White);

            }
            spriteBatch.End();
        }
    }
}
