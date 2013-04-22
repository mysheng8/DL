using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame_Test01.Data
{
    public class Sprite3D
    {
        public Matrix transform;
        public Texture2D texture;
        VertexPositionTexture[] vpt;
        Vector3 pos, lookat, up;
        Matrix world, bbWorld, view, project;
        BasicEffect basicEffect;
        public Sprite3D(Texture2D setTexture,Game game)
        {
            vpt = new VertexPositionTexture[4];
            vpt[0] = new VertexPositionTexture(new Vector3(-25,-25, 0), new Vector2(0, 1));
            vpt[1] = new VertexPositionTexture(new Vector3(-25, 25, 0), new Vector2(0, 0));
            vpt[2] = new VertexPositionTexture(new Vector3(25,-25, 0), new Vector2(1, 1));
            vpt[3] = new VertexPositionTexture(new Vector3(25, 25, 0), new Vector2(1, 0));

            texture = setTexture;



            pos = new Vector3(0, 0, 200);
            lookat = Vector3.Zero;
            up = Vector3.Up;

            world = Matrix.Identity;
            bbWorld = Matrix.Identity;
            view = Matrix.CreateLookAt(pos, lookat, up);
            project = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f / 480f, 1, 1000);

            basicEffect = new BasicEffect(game.GraphicsDevice);
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = project;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = texture;
        }

        public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                pos.X += 1;
                lookat.X += 1;
                
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                pos.X -= 1;
                lookat.X -= 1;
                
            }
            view = Matrix.CreateLookAt(pos, lookat, up);
            basicEffect.View = view;
        }
        public void Draw(GraphicsDevice graphicsDevice) 
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
    
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vpt, 0, 2);
            }

        }

    }




}
