using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class TestBObject : GameObject
    {
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        public TestBObject(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object1Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = false;
            _physics.Solid = true;
            _physics.Vertices.Add(new Vector2(x: 0, y: 0));
            _physics.Vertices.Add(new Vector2(x: 50, y: 0));
            _physics.Vertices.Add(new Vector2(x: 127, y: 0));
            /*
            _physics.Vertices.Add(new Vector2(x: 127, y: 127));
            _physics.Vertices.Add(new Vector2(x: 0, y: 127));
            */
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object1Mask"),
                spriteBatch: spriteBatch);
            Features.Add(_display);
        }
        public PhysicsFeature Physics { get => _physics; }
        public override void Update(GameTime gameTime)
        {
            _display.Position = _physics.Position;
            base.Update(gameTime);
        }
        private void HandleCollision(PhysicsFeature other)
        {

        }
    }
}
