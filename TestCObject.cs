using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class TestCObject : GameObject
    {
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        public TestCObject(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object2Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = false;
            _physics.Solid = true;
            _physics.Vertices.Add(new Vector2(x: 0, y: 0));
            _physics.Vertices.Add(new Vector2(x: 15, y: 3));
            _physics.Vertices.Add(new Vector2(x: 33, y: 17));
            _physics.Vertices.Add(new Vector2(x: 48, y: 40));
            _physics.Vertices.Add(new Vector2(x: 50, y: 45));
            _physics.Vertices.Add(new Vector2(x: 63, y: 72));
            _physics.Vertices.Add(new Vector2(x: 71, y: 87));
            _physics.Vertices.Add(new Vector2(x: 79, y: 99));
            _physics.Vertices.Add(new Vector2(x: 88, y: 110));
            _physics.Vertices.Add(new Vector2(x: 118, y: 126));
            _physics.Vertices.Add(new Vector2(x: 127, y: 127));
            Features.Add(_physics);

            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object2Mask"),
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
