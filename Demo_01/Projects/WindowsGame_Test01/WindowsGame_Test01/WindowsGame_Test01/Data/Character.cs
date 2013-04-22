using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsGame_Test01.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame_Test01.Data
{
    #region Character Render layer


    #region Animation State
    public abstract class AnimState
    {
        public bool breakable;
        public virtual Clip GetClip(CharacterAnimSprite obj){return null;}
        public virtual void Change(CharacterAnimSprite obj, KeyEventArgs e) { }
        public virtual void Enter(CharacterAnimSprite obj) { }

        public virtual void Update(CharacterAnimSprite obj) 
        {
            Clip currentClip = GetClip(obj);
            currentClip.Update();
            //LogHelper.Write(obj.charName + " Update Idle Anim State.");
        }

        public virtual void Draw(CharacterAnimSprite obj, SpriteBatch spriteBatch) 
        {
            Clip currentClip = GetClip(obj);
            SpriteEffects effect = SpriteEffects.None;
            if (obj.dir == Direction.Left)
                effect = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(currentClip.textureImage, obj.rect, currentClip.SourceRect, Color.White,0,new Vector2(0,0),effect,0);
            //LogHelper.Write(obj.charName + " Draw Idle Anim State.");
        }

    }


    public class Idle : AnimState
    {
        private static Idle idle;
        public static Idle instance
        {
            get 
            {
                if (idle == null)
                {
                    idle = new Idle();
                }
                return idle;
            }
        }
        
        private Idle() 
        {
            breakable=true;
        }
        public override void Enter(CharacterAnimSprite s)
        {
            s.next=Idle.instance;
        }
        public override Clip GetClip(CharacterAnimSprite s)
        {
            return s.idle;
        }
        public override void Change(CharacterAnimSprite s, KeyEventArgs e)
        {
            LogHelper.Write("Idle Change State");
            OnRun(s, e);
            OnAttack(s, e);
        }
        private void OnRun(CharacterAnimSprite s, KeyEventArgs e)
        {
            if (e.KeyboardState==KeyStatus.OnPress||e.KeyboardState==KeyStatus.OnKeyDown)
            {
                foreach(Keys key in e.KeyChars)
                {
                    if (key == Keys.Left||key==Keys.Right)
                        s.next=Run.instance;
                        LogHelper.Write(s.charName + " Go to Run Anim State.");
                }
            }
        }
        private void OnAttack(CharacterAnimSprite s, KeyEventArgs e)
        {
            if (e.KeyboardState == KeyStatus.OnPress || e.KeyboardState == KeyStatus.OnKeyDown)
            {
                foreach (Keys key in e.KeyChars)
                {
                    if (key == Keys.Space)
                        s.next = Attack1.instance;
                }
            }
        }

    }
    public class Run : AnimState
    {
        private static Run run;
        public static Run instance
        {
            get 
            {
                if (run == null)
                {
                    run = new Run();
                }
                return run;
            }
        }
        
        private Run() 
        {
            breakable=true;
        }
        public override void Enter(CharacterAnimSprite s)
        {
            s.next = Run.instance;
        }
        public override Clip GetClip(CharacterAnimSprite s)
        {
            return s.run;
        }

        public override void Update(CharacterAnimSprite obj)
        {
            base.Update(obj);
        }

        public override void Change(CharacterAnimSprite s, KeyEventArgs e)
        {
            LogHelper.Write("Run Change State");
            OnStop(s, e);
            OnAttack(s, e);
        }
        private void OnStop(CharacterAnimSprite s, KeyEventArgs e)
        {
            LogHelper.Write(s.charName + " On Stop");
            if (e.KeyboardState==KeyStatus.OnRelease)
            {
                foreach(Keys key in e.KeyChars)
                {
                    if (key == Keys.Left||key==Keys.Right)
                        s.next=Idle.instance;
                        LogHelper.Write(s.charName + " Go back Idle Anim State.");
                }
            }
        }
        private void OnAttack(CharacterAnimSprite s, KeyEventArgs e)
        {
            if (e.KeyboardState == KeyStatus.OnPress || e.KeyboardState == KeyStatus.OnKeyDown)
            {
                foreach (Keys key in e.KeyChars)
                {
                    if (key == Keys.Space)
                        s.next = Attack1.instance;
                }
            }
        }

    }

    public class Attack1 : AnimState
    {
        private static Attack1 attack1;
        public static Attack1 instance
        {
            get
            {
                if (attack1 == null)
                {
                    attack1 = new Attack1();
                }
                return attack1;
            }
        }

        private Attack1()
        {
             breakable = false;
        }
        public override void Enter(CharacterAnimSprite obj)
        {
            obj.next = obj.prev;
        }
        public override Clip GetClip(CharacterAnimSprite obj)
        {
            return obj.attack1;
        }
    }
    #endregion

    #region Clip Sprite Renderer
    public enum Direction
    {
        Left,
        Right
    }

    public class Clip
    {
        public readonly Texture2D textureImage;
        private Point currentFrame, frameSize;
        private Rectangle sourceRect;
        public Rectangle SourceRect
        {
            get { return sourceRect; }
        }
        public int blockLength
        {
            get { return frameSize.X * frameSize.Y; }
        }

        public Clip(Texture2D setTexture, Point setFrameSize)
        {
            textureImage = setTexture;
            frameSize = setFrameSize;
            currentFrame = new Point(0, 0);
            RefreshSourceRect();
        }
        private void RefreshSourceRect()
        {
            int width = textureImage.Width / frameSize.X;
            int height = textureImage.Height / frameSize.Y;
            sourceRect = new Rectangle(currentFrame.X * width, currentFrame.Y * height, width, height);
        }
        public void Update()
        {
            if ((currentFrame.X + 1) * (currentFrame.Y + 1) <= blockLength)
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
        }
    }
    public class CharacterAnimSprite : Sprite2D
    {
        public readonly string charName;
        InputMontior inputMontior;

        public Rectangle rect;

        public AnimState prev;
        public AnimState current; 
        public AnimState next;
        
 
        public Clip idle;
        public Clip run;
        public Clip attack1;

        public  int currentFrame;
        public Direction dir;

        public CharacterAnimSprite(Rectangle setRect,Clip setIdle, Clip setRun,Clip setAttack1)
        {
            current=Idle.instance;
            prev = current;
            current.Enter(this);
            rect=setRect;
            idle = setIdle;
            currentFrame = 0;
            run = setRun;
            attack1 = setAttack1;
            dir = Direction.Right;
            inputMontior = InputMontior.Instance;
            inputMontior.keyEvent += new KeyHandler(this.onChange);

            
        }
        public override void Update()
        {
            if (currentFrame >= current.GetClip(this).blockLength || (current != next && current.breakable))
            {
                prev = current;
                current = next;
                current.Enter(this);
                currentFrame = 0;
            }
            else 
            {
                current.Update(this);
                currentFrame++;
            }
            
        }
        public override void SpriteDraw(SpriteBatch spriteBatch) 
        {
            current.Draw(this,spriteBatch);
        }
        public void onChange(Object s, KeyEventArgs e)
        {
            foreach (Keys key in e.KeyChars)
            {
                if (key==Keys.Left)
                    dir = Direction.Left;
                if (key == Keys.Right)
                    dir = Direction.Right;
            }
            current.Change(this, e);
        }
    }
    #endregion

    #endregion


    class MovingEntity : GameEntity
    {
        protected Vector2 velocity;
        protected Vector2 heading;
        protected Vector2 side;

        protected float mass;
        protected float maxSpeed;
        protected float maxForce;
        protected float maxTurnRate;

        public MovingEntity(string setName)
            : base(setName)
        { 
        
        
        }
    
    }


    class Character : GameEntity
    {
        CharacterAnimSprite sprite;
        Sprite2DManager Sprite2DManager;
        InputMontior inputMontior;
        public int posX;
        public int posY;
        public int speedX;
        public int speedY;

        public bool onGround;

        public Character(string setName, Rectangle setRect, Clip setIdle, Clip setRun, Clip setAttack1)
            : base(setName)
        {
            sprite = new CharacterAnimSprite(setRect, setIdle, setRun, setAttack1);
            Sprite2DManager = Sprite2DManager.Instance;
            Sprite2DManager.CreateCharacterAnim(sprite, RenderPass.Charactor, 1);
            posX = setRect.X;
            posY = setRect.Y;
            speedX = 2;
            speedY = 2;

            inputMontior = InputMontior.Instance;
            inputMontior.keyEvent += new KeyHandler(this.onChange);
        }
        public void onChange(Object s, KeyEventArgs e)
        {
            foreach (Keys key in e.KeyChars)
            {
                if (key == Keys.Left)
                    posX -= speedX;
                if (key == Keys.Right)
                    posX += speedX;
            }
            sprite.rect.X = posX;
            sprite.rect.Y = posY;
            
        }
        public void ForceOnGround() 
        {
        
        }



    }



}
