using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace WindowsGame_Test01.Helper
{
    class Camera : GameComponent
    {
        #region Variables
        float x = 0, y = 0;
        float zHeight = 500.0f;
        public Matrix viewMatrix;
        public Matrix projection;
        InputMontior inputMontior;

        #endregion

        #region Constructor
        public Camera(Game game)
            : base(game)
        {

        } // SimpleCamera(game)
        #endregion

        #region Initialize
        public override void Initialize()
        {
            inputMontior = InputMontior.Instance;
            inputMontior.keyEvent += new KeyHandler(this.KeyboardInputController);
            base.Initialize();
        } // Initialize
        #endregion

        public void KeyboardInputController (Object s , KeyEventArgs e)
        {
            foreach(Keys key in e.KeyChars)
            {
                if (key == Keys.Left)
                    x -= 1;
                if(key==Keys.Right)
                    x += 1;
                if(key==Keys.Up)
                    y -= 1;
                if (key == Keys.Down)
                    y += 1;
                //LogHelper.Write("solving Keyboard Input");
            }

        }


        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            viewMatrix = Matrix.CreateLookAt(
              new Vector3(x, y, -zHeight), new Vector3(x, y, 0), Vector3.Down);
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height, 1, 500);
        
        } // Update(gameTime)
        #endregion
    } // class SimpleCamera

}
