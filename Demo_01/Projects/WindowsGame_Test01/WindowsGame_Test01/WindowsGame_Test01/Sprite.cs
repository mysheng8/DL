using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame_Test01
{
    abstract class Sprite
    {
        Texture2D textureImage;
        protected Point frameSize; Point currentFrame;
        Point sheetSize;
        int timeSinceLastFrame = 0;
        public int millisecondsPerFrame = 40;
        int collisionOffset;
        const int defaultMillisecondsPerFrame = 40;
        public Vector2 speed;
        public Vector2 position;
        public SpriteEffects effects=SpriteEffects.None;

        protected int currentFrameCount = 0;
        public int FrameCounts
        {
            get
            {
                return sheetSize.X * sheetSize.Y;
            }
        }
        public bool isforwardPlay;
        public virtual bool isEnd
        {
            get 
            {
                return currentFrameCount == (sheetSize.X * sheetSize.Y) ? true : false;
            }
        }
                public virtual bool isStart
        {
            get 
            {
                return currentFrameCount==0?true:false;
            }
        }
        public int TimePeriod
        {
            get { return (millisecondsPerFrame * sheetSize.X * sheetSize.Y); }

        }

        public Sprite(Texture2D textureImage, Vector2 position,Point frameSize, int collisionOffset, Point currentFrame,Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position,Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.isforwardPlay = true;
        }

        public void Initialize()
        {
            this.currentFrame = Point.Zero;
            timeSinceLastFrame = 0;
            currentFrameCount = 0;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            this.Move(gameTime, clientBounds);
            this.Play(gameTime);
        }
        public virtual void Move(GameTime gameTime, Rectangle clientBounds)
        {

        }

        public virtual void Play(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                
                if (isforwardPlay)
                {
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        ++currentFrame.Y;
                        if (currentFrame.Y >= sheetSize.Y)
                            currentFrame.Y = 0;
                    }
                }
                else 
                {
                    --currentFrame.X;
                    if (currentFrame.X < 0)
                    {
                        currentFrame.X = sheetSize.X-1;
                        --currentFrame.Y;
                        if (currentFrame.Y < 0)
                            currentFrame.Y = sheetSize.Y-1;
                    }
                }
                ++currentFrameCount;
                if (currentFrameCount > sheetSize.X * sheetSize.Y)
                {
                    currentFrameCount = 0;
                }
            }
        }
        
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,position,new Rectangle(currentFrame.X * frameSize.X,currentFrame.Y * frameSize.Y,frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero,1f, effects, 0);
        }

        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + collisionOffset,
                (int)position.Y + collisionOffset,
                frameSize.X - (collisionOffset * 2),
                frameSize.Y - (collisionOffset * 2));
            }
        }

    }

    class UserControlledSprite : Sprite
    {
        public bool canBreak;
        Vector2 movement;
        public UserControlledSprite(Texture2D textureImage, Vector2 position,Point frameSize, int collisionOffset, Point currentFrame,Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset,currentFrame, sheetSize, speed)
        {
            this.canBreak = true;
        }
        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, bool canBreak)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            this.canBreak = canBreak;
            this.movement = Vector2.Zero;
        }

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    inputDirection.Y -= 1;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    inputDirection.Y += 1;
                }
                return inputDirection * speed;
            }
        }
        public SpriteEffects Geteffects()
        {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    return SpriteEffects.FlipHorizontally;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    return SpriteEffects.None;
                }
                else
                {
                    return effects;
                }

        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            base.Update(gameTime, clientBounds);

        }
        public override void Move(GameTime gameTime, Rectangle clientBounds) 
        {
            if (isStart)
            {
                movement = direction;
                effects=Geteffects();
            }
            position += movement;
            
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
            {
                position.X = clientBounds.Width - frameSize.X;
            }
            if (position.Y > clientBounds.Height - frameSize.Y)
            {
                position.Y = clientBounds.Height - frameSize.Y;
            }
        }
        

        public override bool isEnd
        {
            get
            {
                return (base.isEnd || canBreak);
            }
            
        }
    }


    class AutomatedSprite : Sprite
    {
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
        }
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }
        public override Vector2 direction
        {
            get { return speed; }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {

            base.Update(gameTime, clientBounds);

        }
        public override void Move(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;
            if (direction.X > 0)
                base.effects= SpriteEffects.None;
            else
                base.effects = SpriteEffects.FlipHorizontally;
            base.Update(gameTime, clientBounds);
        }

    }

}


