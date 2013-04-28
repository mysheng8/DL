using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using WindowsGame_Test01.Helper;

namespace WindowsGame_Test01.Data
{
    public class Sprite3D
    {
        public Vector3 pos;

        VertexPositionTexture[] vpt;
        GraphicsDevice device;
        Matrix world;
        public BasicEffect effect;
        Point frameSize;
        public Point currentFrame;
        public Sprite3D( GraphicsDevice setDevice)
        {
            vpt = new VertexPositionTexture[4];
            device = setDevice;
            world = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            effect = new BasicEffect(device);
            effect.World = world;
            effect.TextureEnabled = true;
        }
        public void createSprite(Point setSpriteSize, Point setFrameSize)
        {
            int x = setSpriteSize.X;
            int y = setSpriteSize.Y;
            float u = 1.0f / setFrameSize.X;
            float v = 1.0f / setFrameSize.Y;
            frameSize = setFrameSize;
            currentFrame = new Point(0, 0);
            vpt[0] = new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0, v));
            vpt[1] = new VertexPositionTexture(new Vector3(0, y, 0), new Vector2(0, 0));
            vpt[2] = new VertexPositionTexture(new Vector3(x, 0, 0), new Vector2(u, v));
            vpt[3] = new VertexPositionTexture(new Vector3(x, y, 0), new Vector2(u, 0));
        }
        public void setPos(Vector3 setPos)
        {
            pos = setPos;
            world = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            effect.World = world;
        }
        public void setTexture(Texture2D setTexture) 
        {
            effect.Texture = setTexture;
        }
        public void setView(Matrix setView, Matrix setProj)
        {
            effect.View = setView;
            effect.Projection = setProj;
        }
        public void Update()
        {
            ++currentFrame.X;
            if (currentFrame.X >= frameSize.X)
            {
                currentFrame.X = 0;
                ++currentFrame.Y;
                if (currentFrame.Y >= frameSize.Y)
                    currentFrame.Y = 0;
            }
            float deltaU = 1.0f / frameSize.X;
            float deltaV = 1.0f / frameSize.Y;
            vpt[0].TextureCoordinate = new Vector2(deltaU * currentFrame.X, deltaV * (currentFrame.Y + 1));
            vpt[1].TextureCoordinate = new Vector2(deltaU * currentFrame.X, deltaV * currentFrame.Y);
            vpt[2].TextureCoordinate = new Vector2(deltaU * (currentFrame.X + 1), deltaV * (currentFrame.Y + 1));
            vpt[3].TextureCoordinate = new Vector2(deltaU * (currentFrame.X + 1), deltaV * currentFrame.Y);
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
   
    public class Sprite3DManager
    {
        GraphicsDevice device;
        ContentManager content;
        Camera camera;

        List<Sprite3D> spList;
        public Sprite3DManager(Camera setCamera) 
        {
            device = GameServices.GetService<GraphicsDevice>();
            content = GameServices.GetService<ContentManager>();
            camera = setCamera;
            spList = new List<Sprite3D>();
        }
        public Sprite3D CreateSprite3D(string texFile, Point setSpriteSize, Point setFrameSize)
        {
            Sprite3D s = new Sprite3D(device);
            Texture2D tex = content.Load<Texture2D>(texFile);
            s.setTexture(tex);
            s.createSprite(setSpriteSize, setFrameSize);
            s.setPos(new Vector3(0, 0, 0));
            spList.Add(s);
            return s;
        }
        public void Update()
        {
            foreach (Sprite3D s in spList)
            {
                s.Update();
                s.setView(camera.viewMatrix, camera.projection);
            }
        }
        public void Draw()
        {
            foreach (Sprite3D s in spList)
            {
                s.Draw();
            }
        }
    
    }



}
