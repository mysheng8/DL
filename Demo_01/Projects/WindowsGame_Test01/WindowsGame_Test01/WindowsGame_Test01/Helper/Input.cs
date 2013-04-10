using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace WindowsGame_Test01.Helper
{
    public enum KeyStatus
    {
        OnPress = 0,//just press 
        OnRelease = 1,
        OnKeyDown = 2//keep press
    }

    public enum MouseButton
    {
        None = -1,
        LButton = 0,
        MButton = 1,
        RButton = 2,
        LRButton = 3//press both Left and Right Button
    
    }

    public class KeyEventArgs : EventArgs
    {
        private Keys[] keyChars;
        private KeyStatus keyboardState;
        public KeyEventArgs(Keys[] setKeyChars, KeyStatus setKeyboardState)
            : base()
        {
            this.keyChars = setKeyChars;
            this.keyboardState = setKeyboardState;
        }

        public Keys[] KeyChars
        {
            get
            {
                return keyChars;
            }
        }
        public KeyStatus KeyboardState
        {
            get
            {
                return keyboardState;
            }
        }
    }

    public class MouseEventArgs : EventArgs
    {
        private Vector2 pos;
        private MouseButton button;
        private Vector2 deltaPos;
        private int deltaScrollWheel;
        public MouseEventArgs( Vector2 setPos, MouseButton setButton,Vector2 setDeltaPos,int setDeltaScrollWheel) : base()
        {
            this.pos = setPos;
            this.button = setButton;
            this.deltaPos = setDeltaPos;
            this.deltaScrollWheel=setDeltaScrollWheel;
        }

        public Vector2 Pos
        {
            get
            {
                return pos;
            }
        }
        public MouseButton Button
        {
            get
            {
                return button;
            }
        }
        public Vector2 DeltaPos
        {
            get
            {
                return deltaPos;
            }
        }
        public int DeltaScrollWheel
        {
            get
            {
                return deltaScrollWheel;
            }
        }
    }


    public delegate void KeyHandler(object sender, KeyEventArgs e);

    public delegate void MouseHandler(object sender, MouseEventArgs e);

    public class InputMontior
    {
        private static InputMontior instance;
        private InputMontior() 
        {
            KeyboardState previousKeyboardState = Keyboard.GetState();
            MouseState previousMouseState = Mouse.GetState();


        }
        public static InputMontior Instance
        {
            get 
            {
                if (instance == null)
                    instance = new InputMontior();
                return instance;
            }
        }

        
        public event KeyHandler keyEvent;
        
        public event MouseHandler mouseEvent;

        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;
        private KeyStatus keystate;
        private MouseButton Button;

        public void Update()
        {
            currentKeyboardState= Keyboard.GetState();
            Keys[] keys = currentKeyboardState.GetPressedKeys();
            Keys[] previousKeys = previousKeyboardState.GetPressedKeys();

            if (keys.Length > 0)
            {
                if (keys.Equals(previousKeys))
                    keystate = KeyStatus.OnKeyDown;
                if (previousKeys.Length == 0)
                    keystate = KeyStatus.OnPress;
                KeyEventArgs keyEventArgs = new KeyEventArgs(keys, keystate);
                //rise a keyboard event
                if (keyEvent != null)
                    keyEvent(this, keyEventArgs);
            }
            if (previousKeys.Length > 0)
            {
                Keys[] outkeys = new Keys[previousKeys.Length];
                foreach (Keys k in previousKeys)
                {
                    if (currentKeyboardState.IsKeyUp(k))
                        outkeys[(outkeys.Length - 1)] = k;
                }
                if (outkeys[0] != Keys.None)//outkey is not empty
                {
                    keystate = KeyStatus.OnRelease;
                    KeyEventArgs keyEventArgs = new KeyEventArgs(outkeys, keystate);
                    //rise a keyboard event
                    if (keyEvent != null)
                        keyEvent(this, keyEventArgs);
                }
                
            }
            previousKeyboardState=currentKeyboardState;

            
            currentMouseState= Mouse.GetState();
            if(currentMouseState.LeftButton==ButtonState.Pressed)
                Button=MouseButton.LButton;
            if (currentMouseState.RightButton == ButtonState.Pressed)
                Button = MouseButton.RButton;
            if (currentMouseState.MiddleButton == ButtonState.Pressed)
                Button = MouseButton.MButton;
            if (currentMouseState.LeftButton == ButtonState.Pressed&&currentMouseState.RightButton == ButtonState.Pressed)
                Button = MouseButton.LRButton;
            if (Button != MouseButton.None)
            {
                Vector2 pos = new Vector2(currentMouseState.X, currentMouseState.Y);
                Vector2 deltaPos = new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
                int deltaScrollWheel = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
                MouseEventArgs mouseEventArgs = new MouseEventArgs(pos, Button, deltaPos, deltaScrollWheel);
                //rise a mouse event
                if (mouseEvent != null)
                    mouseEvent(this, mouseEventArgs);
            }
            previousMouseState = currentMouseState;

        }
    }
    
}
