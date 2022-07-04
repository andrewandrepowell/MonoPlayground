using Microsoft.Xna.Framework.Content;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoPlayground
{
    internal class Flag : GameObject
    {
        public AnimationFeature Animation { get; private set; }
        public PhysicsFeature Physics { get; private set; }
        public bool Touched { get; private set; }
        public Flag(ContentManager contentManager)
        {
            Animation = new AnimationFeature(
                gameObject: this,
                textures: Enumerable
                    .Range(1, 6)
                    .Select(x => contentManager.Load<Texture2D>($"flag/flag ({x})"))
                    .ToList());
            Animation.Visible = true;
            Animation.Repeat = true;
            Animation.Play = true;
            
            Physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("bouncer0Mask"),
                collisionHandle: HandleCollision);
            Physics.Physics = false;
            Physics.Solid = false;

            Touched = false;

            Features.Add(Animation);
            Features.Add(Physics);
        }
        public void Touch() => Touched = true;

        private void HandleCollision(PhysicsFeature other)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Animation.Position = Physics.Position + Physics.Mask.Bounds.Center.ToVector2();
            base.Update(gameTime);
        }
    }
}
