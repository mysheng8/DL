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
    public class Camera
    {
        #region Variables
        float x, y ;
        float zHeight = 200.0f;
        public Matrix viewMatrix;
        public Matrix projection;
        InputMontior inputMontior;
        Vector2 viewportSize;
        #endregion

        #region Constructor
        public Camera(Vector2 setViewportSize)
        {
            x = 0;
            y = 0;
            viewportSize = setViewportSize;
            viewMatrix = Matrix.CreateLookAt(new Vector3(x, y, zHeight), new Vector3(x, y, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)viewportSize.X / (float)viewportSize.Y, 1, 1000);
            inputMontior = InputMontior.Instance;
            inputMontior.keyEvent += new KeyHandler(this.KeyboardInputController);
        } // SimpleCamera(game)
        #endregion

        private void KeyboardInputController (Object s , KeyEventArgs e)
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
        public void Update()
        {
            viewMatrix = Matrix.CreateLookAt(
              new Vector3(x, y, zHeight), new Vector3(x, y, 0), Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)viewportSize.X / (float)viewportSize.Y, 1, 1000);
        } // Update(gameTime)
        #endregion
    } // class SimpleCamera

}
