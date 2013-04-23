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

        public BasicEffect effect;
        public Sprite3D(Texture2D setTexture, Vector3 setPos, Matrix setView, Matrix setProj, GraphicsDevice setDevice)
        {
            vpt = new VertexPositionTexture[4];
            vpt[0] = new VertexPositionTexture(new Vector3(-25,-25, 0), new Vector2(0, 1));
            vpt[1] = new VertexPositionTexture(new Vector3(-25, 25, 0), new Vector2(0, 0));
            vpt[2] = new VertexPositionTexture(new Vector3(25,-25, 0), new Vector2(1, 1));
            vpt[3] = new VertexPositionTexture(new Vector3(25, 25, 0), new Vector2(1, 0));

            texture = setTexture;

            device = setDevice;
            Matrix world = Matrix.CreateWorld(pos, new Vector3(0, 0, 1), Vector3.Up);
            effect = new BasicEffect(device);
            effect.World = world;
            effect.View = setView;
            effect.Projection = setProj;
            effect.TextureEnabled = true;
            effect.Texture = texture;

        }

        public void Update()
        {

        }
        public void Draw() 
        {

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
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
