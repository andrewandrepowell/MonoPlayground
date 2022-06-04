using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal class CameraFeature : GameFeature
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get => CameraBounds.Width; }
        public int Height { get => CameraBounds.Height; }
        public int Top { get => Y; }
        public int Bottom { get => Y + CameraBounds.Height; }
        public int Left { get => X; }
        public int Right { get => X + CameraBounds.Width;  }
        public Point Center { get => new Point(x: X + Width / 2, y: Y + Height / 2); }
        public Matrix Transform { get; private set; }
        public Rectangle RoomBounds { get; set; }
        public Rectangle CameraBounds { get; set; }
        public PhysicsFeature Physics { get; set; }
        public int Threshold { get; set; }
        public CameraFeature(GameObject gameObject, Rectangle roomBounds, Rectangle cameraBounds, PhysicsFeature physics, int threshold) : base(gameObject)
        {
            RoomBounds = roomBounds;
            CameraBounds = cameraBounds;
            Physics = physics;
            Transform = Matrix.CreateTranslation(xPosition: 0, yPosition: 0, zPosition: 0);
            Threshold = threshold;
            X = 0;
            Y = 0;
        }
        public override void Update(GameTime gameTime)
        {
            Point physicsCenter = Physics.Center.ToPoint();

            // Determine distances from camera bounds.
            int leftDistance = physicsCenter.X - Left;
            int rightDistance = Right - physicsCenter.X;
            int topDistance = physicsCenter.Y - Top;
            int bottomDistance = Bottom - physicsCenter.Y;

            // Update camera if thresholds are met, in other words
            // if the physics being tracked gets close to the camera bounds.
            if (leftDistance < Threshold)
                X -= Threshold - leftDistance;
            if (rightDistance < Threshold)
                X += Threshold - rightDistance;
            if (topDistance < Threshold)
                Y -= Threshold - topDistance;
            if (bottomDistance < Threshold)
                Y += Threshold - bottomDistance;

            // Fix camera position to ensure it never leaves
            // the bounds of the room itself.
            if (X < RoomBounds.Left)
                X = RoomBounds.Left;
            else if (X > RoomBounds.Right - CameraBounds.Width)
                X = RoomBounds.Right - CameraBounds.Width;
            if (Y < RoomBounds.Top)
                Y = RoomBounds.Top;
            else if (Y > RoomBounds.Bottom - CameraBounds.Height)
                Y = RoomBounds.Bottom - CameraBounds.Height;

            // Update the transform.
            Transform = Matrix.CreateTranslation(xPosition: -X, yPosition: -Y, zPosition: 0);
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
