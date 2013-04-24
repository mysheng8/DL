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
        public Vector3 pos;
        public Texture2D texture;
        VertexPositionTexture[] vpt;
        GraphicsDevice device;
        Matrix world;
        public BasicEffect effect;
        public Sprite3D(Texture2D setTexture, GraphicsDevice setDevice)
        {
            vpt = new VertexPositionTexture[4];
            vpt[0] = new VertexPositionTexture(new Vector3(-25,-25, 0), new Vector2(0, 1));
            vpt[1] = new VertexPositionTexture(new Vector3(-25, 25, 0), new Vector2(0, 0));
            vpt[2] = new VertexPositionTexture(new Vector3(25,-25, 0), new Vector2(1, 1));
            vpt[3] = new VertexPositionTexture(new Vector3(25, 25, 0), new Vector2(1, 0));

            texture = setTexture;
            
            device = setDevice;
            world = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            effect = new BasicEffect(device);
            effect.World = world;
            effect.TextureEnabled = true;
            effect.Texture = texture;

        }
        public void setEffect(Vector3 setPos, Matrix setView, Matrix setProj)
        {
            pos = setPos;
            effect.View = setView;
            effect.Projection = setProj;
            world = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            effect.World = world;
        }
        public void Update()
        {

        }
        public void Draw() 
        {

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SamplerStates[0] = SamplerState.PointClamp;
                device.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vpt, 0, 2);
            }

        }

    }
    public class Renderer
    {
        public Renderer(Game game) 
        {
        
        
        }
    
    
    }



}
