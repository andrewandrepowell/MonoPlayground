using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoPlayground
{
    internal class TestDObject : GameObject
    {
        private readonly PhysicsFeature _physics;
        private readonly DisplayFeature _display;
        public TestDObject(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _physics = new PhysicsFeature(
                gameObject: this,
                mask: contentManager.Load<Texture2D>("object3Mask"),
                collisionHandle: HandleCollision);
            _physics.Physics = false;
            _physics.Solid = true;
            _physics.Vertices.Add(new Vector2(x: 0, y: 127));
            _physics.Vertices.Add(new Vector2(x: 69, y: 121));
            _physics.Vertices.Add(new Vector2(x: 92, y: 116));
            _physics.Vertices.Add(new Vector2(x: 104, y: 111));
            _physics.Vertices.Add(new Vector2(x: 114, y: 102));
            _physics.Vertices.Add(new Vector2(x: 122, y: 90));
            _physics.Vertices.Add(new Vector2(x: 127, y: 80));
            _display = new DisplayFeature(
                gameObject: this,
                texture: contentManager.Load<Texture2D>("object3Mask"),
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
