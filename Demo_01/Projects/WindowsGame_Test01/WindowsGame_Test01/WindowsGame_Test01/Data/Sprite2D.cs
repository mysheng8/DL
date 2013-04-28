using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using WindowsGame_Test01.Helper;





namespace WindowsGame_Test01.Data
{
    public enum RenderPass
    {
        Background,
        Environment,
        Charactor,
        Effect,
        User
    }

    public class Sprite2D
    {
        public RenderPass renderPass;
        public int sortID = 0;
        public virtual void Update() { }
        public virtual void SpriteDraw(SpriteBatch spriteBatch) { }
    }


    #region Sprite class
    public class ImageSprite : Sprite2D
    {
        public Texture2D textureImage;

        public Rectangle rect;
        public Rectangle? sourceRect;


        public ImageSprite(Texture2D setTexture, Rectangle setRect, Rectangle? setSourceRect, RenderPass setRenderPass)
        {
            textureImage = setTexture;
            rect = setRect;
            sourceRect = setSourceRect;
            renderPass = setRenderPass;
        }
        public override void SpriteDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, rect, sourceRect, Color.White);
        }

    }
    #endregion

    #region Animated Sprite class
    public class AnimateSprite : ImageSprite
    {

        private Point currentFrame, frameSize;

        public AnimateSprite(Texture2D setTexture, Rectangle setRect, Point setFrameSize, RenderPass setRenderPass) :
            base(setTexture, setRect, null, setRenderPass)
        {
            frameSize = setFrameSize;
            currentFrame = new Point(0, 0);
            RefreshSourceRect();
        }


        public override void Update()
        {
            ++currentFrame.X;
            if (currentFrame.X >= frameSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y >= frameSize.Y)
                    currentFrame.Y = 0;
            }
            RefreshSourceRect();
        }

        private void RefreshSourceRect()
        {
            int width = textureImage.Width / frameSize.X;
            int height = textureImage.Height / frameSize.Y;
            sourceRect = new Rectangle(currentFrame.X * width, currentFrame.Y * height, width, height);
        }
    }
    #endregion




    public class Sprite2DManager
    {
        static SpriteBatch spriteBatch;
        static GraphicsDevice device;
        static ContentManager content;
        List<Sprite2D> SpriteList;

        private static Sprite2DManager sprite2DManager;
        private Sprite2DManager()
        {
            SpriteList = new List<Sprite2D>();
            device = GameServices.GetService<GraphicsDevice>();
            content = GameServices.GetService<ContentManager>();
            spriteBatch = new SpriteBatch(device);
            
        }

        public static Sprite2DManager Instance
        {
            get
            {
                if (sprite2DManager == null)
                    sprite2DManager = new Sprite2DManager();
                return sprite2DManager;
            }
        }

        public Sprite2D CreateSimpleSprite(string texFileName, Rectangle setRect, Rectangle? setSourceRect, RenderPass setRenderPass, int setSortID = 0)
        {
            Texture2D newTex = content.Load<Texture2D>(texFileName);
            ImageSprite sprite = new ImageSprite(newTex, setRect, setSourceRect, setRenderPass);
            SpriteList.Add(sprite);
            sprite.sortID = setSortID;
            LogHelper.Write("Create a Simple Sprite " + texFileName);
            return sprite;
        }

        public Sprite2D CreateAnimateSprite(string texFileName, Rectangle setRect, Point setFrameSize, RenderPass setRenderPass, int setSortID = 0)
        {
            Texture2D newTex = content.Load<Texture2D>(texFileName);
            AnimateSprite sprite = new AnimateSprite(newTex, setRect, setFrameSize, setRenderPass);
            SpriteList.Add(sprite);
            sprite.sortID = setSortID;

            LogHelper.Write("Create a Animated Sprite " + texFileName);
            return sprite;
        }

        public Sprite2D CreateSpriteText(string setText, Vector2 setPosition, string spriteFontFileName, RenderPass setRenderPass, int setSortID = 0)
        {
            SpriteFont spriteFont = content.Load<SpriteFont>(spriteFontFileName);
            SpriteText spriteText = new SpriteText(setText, setPosition, spriteFont);
            spriteText.renderPass = setRenderPass;
            spriteText.sortID = setSortID;
            SpriteList.Add(spriteText);
            LogHelper.Write("Create a Sprite Text " + spriteText);
            return spriteText;
        }
        public Sprite2D CreateBackground(string texFileName, int setPaintWidth, int setPaintHeight, RenderPass setRenderPass, int setSortID = 0)
        {
            Texture2D newTex = content.Load<Texture2D>(texFileName);
            int screenWidth = device.Viewport.Width;
            int screenHeight = device.Viewport.Height;
            Background background = new Background(newTex, setPaintWidth, setPaintHeight, screenWidth, screenHeight);
            background.renderPass = setRenderPass;
            background.sortID = setSortID;
            SpriteList.Add(background);
            LogHelper.Write("Create a Simple Sprite " + texFileName);
            return background;
        }
        public Sprite2D CreateCanvas(string texFileName, int setClipSizeX, int setClipSizeY, int setPaintWidth, int setPaintHeight, int[,] setMap, RenderPass setRenderPass, int setSortID = 0)
        {
            Texture2D newTex = content.Load<Texture2D>(texFileName);
            int screenWidth = device.Viewport.Width;
            int screenHeight = device.Viewport.Height;
            Canvas canvas = new Canvas(newTex, setClipSizeX, setClipSizeY, setPaintWidth, setPaintHeight, setMap, screenWidth, screenHeight);
            canvas.renderPass = setRenderPass;
            canvas.sortID = setSortID;

            SpriteList.Add(canvas);

            LogHelper.Write("Create a Canvas " + texFileName);
            return canvas;
        }


        public void RemoveSpirte(Sprite2D sprite)
        {
            SpriteList.Remove(sprite);

        }


        public void Update()
        {
            foreach (Sprite2D sprite in SpriteList)
            {
                sprite.Update();
            }
        }

        public void Draw()
        {
            foreach (RenderPass pass in Enum.GetValues(typeof(RenderPass)))
            {
                List<Sprite2D> renderPassList = SpriteList.FindAll(r => r.renderPass == pass);
                renderPassList.Sort((r1, r2) => r1.sortID.CompareTo(r2.sortID));
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, DepthStencilState.DepthRead, RasterizerState.CullCounterClockwise);
                //spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                foreach (Sprite2D s in renderPassList)
                {
                    s.SpriteDraw(spriteBatch);
                }
                spriteBatch.End();
            }
        }
    }




}
