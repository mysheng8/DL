using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WindowsGame_Test01.Helper;

namespace WindowsGame_Test01.Data
{
    public class Background : Sprite2D
    {
        public Texture2D textureImage;

        public int distX = 0;
        public int distY = 0;

        private int movingSpeedX = 2;
        private int movingSpeedY = 1;

        private readonly int paintWidth;
        private readonly int paintHeight;

        private readonly int screenSizeX;
        private readonly int screenSizeY;

        private InputMontior inputMontior;

        public void setMovingSpeed(int x, int y)
        {
            movingSpeedX = x;
            movingSpeedY = y;
        }
        public Background(Texture2D setTexture, int setPaintWidth, int setPaintHeight, int setScreenSizeX, int setScreenSizeY)
        {
            textureImage = setTexture;
            paintWidth = setPaintWidth;
            paintHeight = setPaintHeight;

            screenSizeX = setScreenSizeX;
            screenSizeY = setScreenSizeY;
            inputMontior = InputMontior.Instance;
            inputMontior.keyEvent += new KeyHandler(this.KeyboardInputController);
        
        }
        public override void SpriteDraw(SpriteBatch spriteBatch)
        {
            int y = distY;
            if (distY > 0) { y = 0; }
            if ((distY + paintHeight) < screenSizeY && paintHeight > screenSizeY) { y = screenSizeY - paintHeight; }

            int numClip = screenSizeX / paintWidth +1;
            for (int i = 0; i <= numClip; i++) 
            {
                int x = (i - 1) * paintWidth + distX % paintWidth;
                if (distX < 0) 
                {
                    x = i * paintWidth + distX % paintWidth;
                }
                Rectangle Rect = new Rectangle(x, y, paintWidth, paintHeight);
                spriteBatch.Draw(textureImage, Rect, null, Color.White);
            }

        }
        private void KeyboardInputController(Object s, KeyEventArgs e)
        {
            foreach (Keys key in e.KeyChars)
            {
                if (key == Keys.Left)
                    distX += movingSpeedX;
                if (key == Keys.Right)
                    distX -= movingSpeedX;
                if (key == Keys.Up)
                    distY += movingSpeedY;
                if (key == Keys.Down)
                    distY -= movingSpeedY;
                //LogHelper.Write("solving Keyboard Input");
                
            }
        }
    
    }

    public class Canvas: Sprite2D
    {
        public Texture2D textureImage;
               

        public int distX = 0;
        public int distY = 0;
        private int movingSpeedX = 3;
        private int movingSpeedY = 3;

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

        private void KeyboardInputController(Object s, KeyEventArgs e)
        {
            foreach (Keys key in e.KeyChars)
            {
                if (key == Keys.Left)
                    distX += movingSpeedX;
                if (key == Keys.Right)
                    distX -= movingSpeedX;
                if (key == Keys.Up)
                    distY += movingSpeedY;
                if (key == Keys.Down)
                    distY -= movingSpeedY;
                //LogHelper.Write("solving Keyboard Input");
            }
        }
        public void setMovingSpeed(int x, int y)
        {
            movingSpeedX = x;
            movingSpeedY = y;
        }

    }



    public class Ground
    {
        public readonly float height;
        public Ground(float setHeight) 
        {
            height = setHeight;
        }
    
    }


    public class CollisionWorld
    {
        private CollisionWorld collisionWorld;
        private CollisionWorld() { }
        public CollisionWorld instance
        {
            get 
            {
                if (collisionWorld == null)
                {
                    collisionWorld = new CollisionWorld();
                }
                return collisionWorld;
            }
        }

        public void CreateGround(float setHeight) 
        {
            Ground ground = new Ground(setHeight);

        }



    
    }




    public class Map 
    {
        Sprite2DManager sprite2DManager;

        public Map()
        {
            sprite2DManager = Sprite2DManager.Instance;
            AddRenderLayer();
            AddCollisionLayer();
        }
        public void AddRenderLayer()
        {
            sprite2DManager.CreateBackground(@"test/background", 300, 500, RenderPass.Background, 0);
            int[,] mg_map = new int[20, 20]{
                                            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 },
                                            { 2, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 3, 3, 3, 3, 3, 3, 3, 3, 3, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 3, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 3, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 9, 2 },
                                            { 2, 10, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 12, 2 },
                                            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }
                                            };

            sprite2DManager.CreateCanvas(@"test/things1", 4, 4, 32, 32, mg_map, RenderPass.Background, 2);
        
        }
        public void AddCollisionLayer() 
        {
        
        
        }

    }
        
}
