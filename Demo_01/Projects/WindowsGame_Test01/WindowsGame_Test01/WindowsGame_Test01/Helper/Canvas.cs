using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame_Test01.Helper
{
    public class Canvas : Sprite
    {
        public Texture2D textureImage;
               

        public int distX = 0;
        public int distY = 0;
        public int movingSpeedX = 2;
        public int movingSpeedY = 2;

        private readonly int mapSizeX;
        private readonly int mapSizeY;

        //each clip picture paint number
        private readonly int clipSizeX;
        private readonly int clipSizeY;

        //each paint pixals number
        private readonly int paintWidth;
        private readonly int paintHeight;

        private readonly int screenSizeX;
        private readonly int screenSizeY;

        private readonly int[,] map;

        private InputMontior inputMontior;

        public Canvas(Texture2D setTexture, int setClipSizeX, int setClipSizeY, int setPaintWidth, int setPaintHeight, int[,] setMap, int setScreenSizeX, int setScreenSizeY)
        {
            textureImage = setTexture;
            mapSizeX = setMap.GetLength(0);
            mapSizeY = setMap.GetLength(1); 
            map = setMap;

            paintWidth=setPaintWidth;
            paintHeight=setPaintHeight;

            screenSizeX = setScreenSizeX;
            screenSizeY = setScreenSizeY;

            clipSizeX=setClipSizeX;
            clipSizeY=setClipSizeY;
            inputMontior = InputMontior.Instance;
            inputMontior.keyEvent += new KeyHandler(this.KeyboardInputController);
        }

        public override void SpriteDraw(SpriteBatch spriteBatch)
        {
            //cut the clip out of screen
            int startIndexX = Math.Min(Math.Max(-distX / paintWidth,0),mapSizeX-1);
            int startIndexY = Math.Min(Math.Max(-distY / paintHeight,0),mapSizeY-1);
            int endIndexX = Math.Min(Math.Max((screenSizeX - distX) / paintWidth + 1, 0), mapSizeX);
            int endIndexY = Math.Min(Math.Max((screenSizeY - distY) / paintHeight + 1, 0), mapSizeY);

            for (int x = startIndexX; x < endIndexX; x++)
            {
                for (int y = startIndexY; y < endIndexY; y++)
                {
                    Rectangle DestRect = new Rectangle(x * paintWidth + distX, y * paintHeight + distY, paintWidth, paintHeight);
                    int paint = map[x, y];
                    int clipX = paint % clipSizeX;
                    int clipY = paint / clipSizeY;
                    Rectangle ClipRect = new Rectangle(clipX * paintWidth, clipY * paintHeight, paintWidth, paintHeight);
                    spriteBatch.Draw(textureImage, DestRect, ClipRect, Color.White);
                }
            }
        }
        public override void Update()
        {
            base.Update();

        }

        private void KeyboardInputController(Object s, KeyEventArgs e)
        {
            foreach (Keys key in e.KeyChars)
            {
                if (key == Keys.Left)
                    distX -= movingSpeedX;
                if (key == Keys.Right)
                    distX += movingSpeedX;
                if (key == Keys.Up)
                    distY -= movingSpeedY;
                if (key == Keys.Down)
                    distY += movingSpeedY;
                //LogHelper.Write("solving Keyboard Input");
            }
        
        
        }

    }
        
}
