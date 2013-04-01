using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace WindowsGame_Test01.Data
{
    public class Sprite
    {
        public RenderPass renderPass;
        public int sortID = 0;
        public virtual void Update() { }
        public virtual void SpriteDraw(SpriteBatch spriteBatch) { }
    }


    #region Sprite class
    public class ImageSprite : Sprite
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




}
