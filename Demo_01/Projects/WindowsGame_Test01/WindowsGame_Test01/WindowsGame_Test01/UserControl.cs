using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame_Test01
{
    class AutomateStatus
    {
        public string StatusID;
        UserControlledSprite AnimateClip;
        List<string> FromStatus;
        List<AutomatedSprite> TransitionInClip;
        public Vector2 position{get;set;}
        public SpriteEffects filp{get;set;}
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 40;
        int currentFrameCount = 0;

        public bool isEnd
        {
            get
            {
                return AnimateClip.isEnd;
            }
            
        }



        public AutomateStatus(string id,UserControlledSprite AnimateClip)
        {
            this.StatusID = id;
            this.AnimateClip = AnimateClip;
            this.FromStatus = new List<string>();
            this.TransitionInClip = new List<AutomatedSprite>();
            
            this.position = Vector2.Zero;
            this.filp = SpriteEffects.None;

        }

        public AutomateStatus(string id,UserControlledSprite AnimateClip, string FromStatusID, AutomatedSprite TransitionInClip)
            
        {
            this.StatusID = id;
            this.AnimateClip = AnimateClip;
            this.FromStatus = new List<string>();
            this.TransitionInClip = new List<AutomatedSprite>();
            
            this.position = Vector2.Zero;
            this.filp = SpriteEffects.None;

            this.FromStatus.Add(FromStatusID);
            this.TransitionInClip.Add(TransitionInClip);


        }

        public void Initialize()
        {
            currentFrameCount = 0;
            AnimateClip.Initialize();
            foreach(AutomatedSprite clip in TransitionInClip)
            {
                clip.Initialize();
            }
        }

        public void AddInterface(string FromStatusID, AutomatedSprite TransitionClip)
        {
            if (this.FromStatus.IndexOf(FromStatusID) < 0)
            {
                this.FromStatus.Add(FromStatusID);
                this.TransitionInClip.Add(TransitionClip);
            }

        }
        public void Update(GameTime gameTime, Rectangle clientBounds, string  FromStatusID)
        {
            
            int InClipID = this.FromStatus.IndexOf(FromStatusID);
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (InClipID >= 0)
            {
                AutomatedSprite InClip = TransitionInClip[InClipID];
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    ++currentFrameCount;
                    if (currentFrameCount > (AnimateClip.FrameCounts + InClip.FrameCounts))
                    {
                        currentFrameCount = AnimateClip.FrameCounts + InClip.FrameCounts;
                        timeSinceLastFrame = 0;
                    }
                }
                
                this.Move(gameTime, clientBounds, InClipID);
                this.Play(gameTime, InClipID);
            }
            else
            {
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    ++currentFrameCount;
                    if (currentFrameCount > AnimateClip.FrameCounts)
                    {
                        currentFrameCount = AnimateClip.FrameCounts;
                        timeSinceLastFrame = 0;
                    }
                }
                this.Move(gameTime, clientBounds);
                this.Play(gameTime);
            }
        }

        public void Move(GameTime gameTime, Rectangle clientBounds, int InClipID)
        {

            AutomatedSprite InClip = TransitionInClip[InClipID];
            AnimateClip.effects = filp;
            AnimateClip.position = position;
            if (currentFrameCount > InClip.FrameCounts)
            {
                AnimateClip.Move(gameTime, clientBounds);
                position = AnimateClip.position;
                filp = AnimateClip.effects;
            }
            else 
            {
                filp = AnimateClip.Geteffects();
            }
            InClip.effects = filp;
            InClip.position = position;
            
        }
        public void Move(GameTime gameTime, Rectangle clientBounds)
        {
            AnimateClip.position = position;
            AnimateClip.Move(gameTime, clientBounds);
            position = AnimateClip.position;
            filp = AnimateClip.effects;
        }


        public void Play(GameTime gameTime, int InClipID)
        {
            AutomatedSprite InClip = this.TransitionInClip[InClipID];
            if (currentFrameCount <= InClip.FrameCounts)
            {
                InClip.Play(gameTime);
            }
            else
            {
                AnimateClip.Play(gameTime);
            }
        }

        public void Play(GameTime gameTime)
        {
            AnimateClip.Play(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, string FromStatusID)
        {
            int InClipID = this.FromStatus.IndexOf(FromStatusID);
            if (InClipID >= 0)
            {
                AutomatedSprite InClip = this.TransitionInClip[InClipID];
                if (currentFrameCount <= InClip.FrameCounts)
                {
                    InClip.Draw(gameTime, spriteBatch);
                }
                else
                {
                    AnimateClip.Draw(gameTime, spriteBatch);
                }
            }
            else
            {
                AnimateClip.Draw(gameTime, spriteBatch);
            }
        }


    }


    class UserControl 
    {
        Texture2D idleImage;
        Texture2D walkImage;
        Texture2D idle2walkImage;
        UserControlledSprite walkSprite;
        UserControlledSprite idleSprite;
        AutomatedSprite idle2walk;
        AutomatedSprite walk2idle;
        AutomateStatus currentStatus;
        AutomateStatus lastStatus;
        AutomateStatus nextStatus;
        AutomateStatus walkStatus;
        AutomateStatus idleStatus;
        
        public UserControl(Texture2D idleImage, Texture2D walkImage,Texture2D idle2walkImage)
        {
            this.idleImage = idleImage;
            this.walkImage = walkImage;
            this.idle2walkImage = idle2walkImage;
            this.walkSprite = new UserControlledSprite(this.walkImage, Vector2.Zero, new Point(150, 150), 10, new Point(0, 0), new Point(4, 5), new Vector2(1.2f, 1.2f), 40, false);
            this.idleSprite = new UserControlledSprite(this.idleImage, Vector2.Zero, new Point(150, 150), 10, new Point(0, 0), new Point(5, 3), Vector2.Zero);

            this.idle2walk = new AutomatedSprite(this.idle2walkImage, Vector2.Zero, new Point(150, 150), 10, new Point(0, 0), new Point(4, 1), Vector2.Zero);
            this.walk2idle = new AutomatedSprite(this.idle2walkImage, Vector2.Zero, new Point(150, 150), 10, new Point(3, 0), new Point(4, 1), Vector2.Zero);
            this.walk2idle.isforwardPlay = false;
            this.walkStatus = new AutomateStatus("walk", walkSprite);
            this.walkStatus.AddInterface("idle", idle2walk);


            this.idleStatus = new AutomateStatus("idle", idleSprite);
            this.idleStatus.AddInterface("walk", walk2idle);

            this.currentStatus = idleStatus;
            this.lastStatus = idleStatus;

        }


        public  void Update(GameTime gameTime, Rectangle clientBounds)
        {


            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                nextStatus = walkStatus;
            }
            else
            {
                nextStatus = idleStatus;
            }
            if (currentStatus.isEnd)
            {
                lastStatus = currentStatus;
                currentStatus = nextStatus;
                currentStatus.Initialize();
            }
            currentStatus.position = lastStatus.position;
            currentStatus.filp = lastStatus.filp;
            currentStatus.Update(gameTime, clientBounds, lastStatus.StatusID);

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            currentStatus.Draw(gameTime, spriteBatch, lastStatus.StatusID);
        }

    }

}

