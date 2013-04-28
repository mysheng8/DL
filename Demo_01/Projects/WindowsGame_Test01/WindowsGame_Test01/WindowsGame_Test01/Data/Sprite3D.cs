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
    public struct Rect
    {
        public Vector2 min;
        public Vector2 max;
    }

    public class Sprite3D
    {
        public Vector3 pos;

        VertexPositionTexture[] vpt;
        GraphicsDevice device;
        Matrix world;
 
        public Point frameSize;
        public Point currentFrame;
        public BasicEffect effect;

        public Sprite3D( GraphicsDevice setDevice)
        {
            vpt = new VertexPositionTexture[4];
            device = setDevice;
            world = Matrix.CreateWorld(pos, Vector3.Forward, Vector3.Up);
            effect = new BasicEffect(device);
            effect.World = world;
            effect.TextureEnabled = true;
        }
        public void createSquard(Point setSpriteSize)
        {
            int x = setSpriteSize.X;
            int y = setSpriteSize.Y;
            currentFrame = new Point(0, 0);
            vpt[0] = new VertexPositionTexture(new Vector3(0, 0, 0), new Vector2(0, 1));
            vpt[1] = new VertexPositionTexture(new Vector3(0, y, 0), new Vector2(0, 0));
            vpt[2] = new VertexPositionTexture(new Vector3(x, 0, 0), new Vector2(1, 1));
            vpt[3] = new VertexPositionTexture(new Vector3(x, y, 0), new Vector2(1, 0));
        }
        public void setFrameSize(Point setSize)
        {
            frameSize = setSize;
            float u = 1.0f / frameSize.X;
            float v = 1.0f / frameSize.Y;
            vpt[0].TextureCoordinate = new Vector2(0, v);
            vpt[1].TextureCoordinate = new Vector2(0, 0);
            vpt[2].TextureCoordinate = new Vector2(u, v);
            vpt[3].TextureCoordinate = new Vector2(u, 0);
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
        public void setUV(Rect UV)
        {
            vpt[0].TextureCoordinate = new Vector2(UV.min.X, UV.max.Y);
            vpt[1].TextureCoordinate = new Vector2(UV.min.X, UV.min.Y);
            vpt[2].TextureCoordinate = new Vector2(UV.max.X, UV.max.Y);
            vpt[3].TextureCoordinate = new Vector2(UV.max.X, UV.min.Y);
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

    public class AnimSp
    {
        Point frameSize, currentFrame;
        public AnimSp(Point setFrameSize)
        {
            frameSize = setFrameSize;
        }
        public Rect Update()
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
            Rect UV;
            UV.min = new Vector2(deltaU * currentFrame.X, deltaV * currentFrame.Y);
            UV.max = new Vector2(deltaU * (currentFrame.X + 1), deltaV * (currentFrame.Y + 1));
            return UV;
        }
    }
   
    public class Sprite3DManager
    {
        GraphicsDevice device;
        ContentManager content;
        Camera camera;

        List<Sprite3D> spList;
        private Sprite3DManager() 
        {
            device = GameServices.GetService<GraphicsDevice>();
            content = GameServices.GetService<ContentManager>();
            spList = new List<Sprite3D>();
        }
        private static Sprite3DManager sprite3DManager;
        public static Sprite3DManager instance
        {
            get
            { 
                if(sprite3DManager==null)
                {
                    sprite3DManager = new Sprite3DManager();
                }
                return sprite3DManager;
            }
        }
        public void setCamera(Camera setCamera)
        {
            camera=setCamera;
        }
        public Sprite3D CreateSprite3D(Point setSpriteSize)
        {
            Sprite3D s = new Sprite3D(device);
            s.createSquard(setSpriteSize);
            s.setPos(new Vector3(0, 0, 0));
            spList.Add(s);
            return s;
        }
        public Sprite3D CreateSimpleSprite3D(string texFile, Point setSpriteSize, Point setFrameSize)
        {
            Sprite3D s = CreateSprite3D(setSpriteSize);
            Texture2D tex = content.Load<Texture2D>(texFile);
            s.setTexture(tex);
            s.setFrameSize(setFrameSize);
            return s;
        }
        public void Update()
        {
            foreach (Sprite3D s in spList)
            {
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
