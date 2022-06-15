using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MonoPlayground
{
    internal class PhysicsFeature : GameFeature
    {
        private readonly Texture2D _mask;
        private readonly Action<PhysicsFeature> _collisionHandle;
        private readonly ICollection<PhysicsFeature> _collidablePhysics;
        private readonly IList<Vector2> _vertices;
        private Vector2 _collisionPoint;
        private Vector2 _collisionNormal;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _acceleration;
        private Vector2 _stickApplied;
        private float _maxSpeed;
        private float _friction;
        private float _stick;
        private float _stickThreshold;
        private float _bounce;
        private bool _solid;
        private bool _physics;
        public PhysicsFeature(GameObject gameObject, Texture2D mask, Action<PhysicsFeature> collisionHandle) : base(gameObject: gameObject)
        {
            _mask = mask;
            _collisionHandle = collisionHandle;
            _collidablePhysics = new List<PhysicsFeature>();
            _vertices = new List<Vector2>();
            _collisionPoint = Vector2.Zero;
            _collisionNormal = Vector2.Zero;
            _position = Vector2.Zero;
            _velocity = Vector2.Zero;
            _acceleration = Vector2.Zero;
            _stickApplied = Vector2.Zero;
            _friction = 0f;
            _maxSpeed = 0f;
            _stick = 0f;
            _stickThreshold = 0f;
            _bounce = 0f;
            _solid = false;
            _physics = false;
            // https://codepen.io/OliverBalfour/post/implementing-velocity-acceleration-and-friction-on-a-canvas
        }
        public override void Update(GameTime gameTime)
        {
            // Only apply the physic operations if physics is enabled in the first place.
            if (!_physics)
                return;

            // The elapsed time is needed since position, velocity, and acceleration affect in each with respect to time.
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update velocity based on current acceleration.
            _velocity += _acceleration * timeElapsed; // Apply acceleration to velocity.

            // Update velocity based on the effects of total friction and maximum speed.
            if (_velocity != Vector2.Zero)
            {
                float _speed = GameMath.Min(_velocity.Length(), _maxSpeed);
                _velocity = Vector2.Normalize(_velocity) * _speed;
            }

            // Apply velocity and stick to position.
            _position += (_velocity - _stickApplied) * timeElapsed; 

            _stickApplied = Vector2.Zero;
            _collidablePhysics.ForEach(x => Collide(gameTime, x)); // Apply collision to position and velocity.
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
        public Texture2D Mask { get => _mask; }
        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Center { get => _position + _mask.Bounds.Center.ToVector2(); }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }
        public Vector2 Acceleration { get => _acceleration; set => _acceleration = value; }
        public float Friction 
        { 
            get => _friction;
            set 
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException();
                _friction = value; 
            } 
        }
        public float MaxSpeed 
        { 
            get => _maxSpeed;
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException();
                _maxSpeed = value;
            }
        }
        public float Bounce
        {
            get => _bounce;
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException();
                _bounce = value;
            }
        }
        public float Stick
        {
            get => _stick;
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException();
                _stick = value;
            }
        }
        public float StickThreshold
        {
            get => _stickThreshold;
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException();
                _stickThreshold = value;
            }
        }
        public bool Solid {  get => _solid; set => _solid = value; }
        public bool Physics { get => _physics; set => _physics = value; }
        public Vector2 CollisionPoint { get => _collisionPoint; }
        public Vector2 CollisionNormal { get => _collisionNormal; }
        public ICollection<PhysicsFeature> CollidablePhysics { get => _collidablePhysics; }
        public IList<Vector2> Vertices { get => _vertices; }
        private void Collide(GameTime gameTime, PhysicsFeature other)
        {
            // Determine the bounding rectangles for this physics and the other physics.
            Rectangle thisBounds = new Rectangle(
                location: _position.ToPoint(),
                size: _mask.Bounds.Size);
            Rectangle otherBounds = new Rectangle(
                location: other.Position.ToPoint(),
                size: other.Mask.Bounds.Size);

            // Determine whether the bounding rectangles intersect.
            // If they don't, don't bother going further.
            if (!thisBounds.Intersects(otherBounds))
                return;

            // Find the bounding rectangle that represents the overlap between 
            // this and other bounding rectangles. 
            Rectangle intersection = Rectangle.Intersect(thisBounds, otherBounds);

            // Find the equivalent rectangles that represent the intersection relative
            // to this and other's mask bounding rectangles.
            // These are needed to extract the corresponding pixel data from the masks.
            Rectangle thisIntersection = new Rectangle(
                x: intersection.X - thisBounds.X,
                y: intersection.Y - thisBounds.Y,
                width: intersection.Width,
                height: intersection.Height);
            Rectangle otherIntersection = new Rectangle(
                x: intersection.X - otherBounds.X,
                y: intersection.Y - otherBounds.Y,
                width: intersection.Width,
                height:intersection.Height);
            int Area = intersection.Height * intersection.Width;

            Color[] thisData = new Color[Area];
            Color[] otherData = new Color[Area];

            _mask.GetData(
                level: 0,
                rect: thisIntersection,
                data: thisData,
                startIndex: 0,
                elementCount: thisData.Length);
            other.Mask.GetData(
                level: 0,
                rect: otherIntersection,
                data: otherData,
                startIndex: 0,
                elementCount: otherData.Length);

            // Build a collision mask, where each element is a boolean that indicates the pixel in the same place
            // in this pixel data and other pixel data are both not the transparent color.
            // If there's at least one element in the collision mask that's true, then we know
            // a collision occured.
            bool[] collisionMask = thisData.Zip(otherData, (td, od) => td != Color.Transparent && od != Color.Transparent).ToArray();
            bool collisionOccurred = collisionMask.Contains(true);

            // Specific operations occur on collision.
            if (collisionOccurred)
            {
                // Extra functionality is needed for the case where
                // both this physics and the other physics are solid. 
                // 1) This physic's position needs correction to remove the overlap.
                // 2) "Collision physics" is applied at this step.  
                if (_solid && other.Solid)
                {
                    // The first correction is to apply floor rounding to the position.
                    // This is needed because MonoGame's Rectangle is only integers.
                    _position.X = (int)_position.X;
                    _position.Y = (int)_position.Y;

                    // Several important pieces of information is needed in order
                    // to detect what direction to perform the correction, and 
                    // how much correction is needed.

                    // topSum, bottomSum, leftSum, and rightSum indicate what portion of this physics 
                    // the collision occurred. This information is used to partly determine
                    // what direction this physics should get shifted.
                    // It's worth noting this approach assumes the center of mass is directly in the
                    // middle of the mask, which will need to be corrected at some point.

                    // rowSums and colSums are used to determine what dimension has the most overlap.
                    int thisMidHeight = thisBounds.Height / 2;
                    int thisMidWidth = thisBounds.Width / 2;
                    int thisRowOffset = intersection.Y - thisBounds.Y;
                    int thisColOffset = intersection.X - thisBounds.X;
                    int topSum = 0, bottomSum = 0, leftSum = 0, rightSum = 0;
                    int[] rowSums = new int[intersection.Height];
                    int[] colSums = new int[intersection.Width];
                    for (int row = 0; row < intersection.Height; row++)
                        for (int col = 0; col < intersection.Width; col++)
                            if (collisionMask[col + row * intersection.Width])
                            {
                                rowSums[row]++;
                                colSums[col]++;
                                if (row + thisRowOffset < thisMidHeight)
                                    topSum++;
                                else
                                    bottomSum++;
                                if (col + thisColOffset < thisMidWidth)
                                    leftSum++;
                                else
                                    rightSum++;
                            }

                    // The maxes of each dimension are used to determine
                    // how much movement is necessary to eliminate the overlap.
                    int rowMax = rowSums.Max();
                    int colMax = colSums.Max();

                    // The respective index of each max is needed in order
                    // to calculate the point of collision. 
                    // There's a special case where there could be multiple indices of the same max.
                    // The average of the indices are taken. 
                    List<int> rowMaxIndexes = new List<int>();
                    List<int> colMaxIndexes = new List<int>();

                    for (int row = 0; row < intersection.Height; row++)
                        if (rowSums[row] == rowMax)
                            rowMaxIndexes.Add(row);

                    for (int col = 0; col < intersection.Width; col++)
                        if (colSums[col] == colMax)
                            colMaxIndexes.Add(col);

                    int colMaxIndex = (int)colMaxIndexes.Average();
                    int rowMaxIndex = (int)rowMaxIndexes.Average();

                    // The correction to the position and the calculation of the point of collision
                    // is done here.
                    // The correction is done only is one dimension, and such that minimum
                    // correction is needed.
                    
                    // Move in Y direction case:
                    if (colMax < rowMax)
                    {
                        // Since the correction is based on the column max, 
                        // the X position of the collision point is just the X position
                        // of the intersection bounding rectangle offset by the
                        // index of the column max.
                        _collisionPoint.X = intersection.X + colMaxIndex;

                        // The topSum and bottomSum are used to know where the intersection
                        // occurred on this mask.

                        // "Collision occurred on the bottom of this" case:
                        if (topSum < bottomSum)
                        {
                            // Apply the correction.
                            _position.Y -= colMax;

                            // The Y position of the collision point is the first instance
                            // of collision in the column of the maximum index, 
                            // starting from the top.
                            for (int row = 0; row < intersection.Height; row++)
                                if (collisionMask[colMaxIndex + row * intersection.Width])
                                {
                                    _collisionPoint.Y = intersection.Y + row;
                                    break;
                                }
                        }
                        // "Collision occurred on the top of this" case:
                        else
                        {
                            // Apply the correction.
                            _position.Y += colMax;

                            // The Y position of the collision point is the first instance
                            // of collision in the column of the maximum index, 
                            // starting from the bottom.
                            for (int row = intersection.Height - 1; row >= 0; row--)
                                if (collisionMask[colMaxIndex + row * intersection.Width])
                                {
                                    _collisionPoint.Y = intersection.Y + row;
                                    break;
                                }
                        }
                    }
                    // Move in X direction case:
                    // The operations below follow the same rationale as that
                    // done in the Y direction case.
                    else
                    {
                        _collisionPoint.Y = intersection.Y + rowMaxIndex;

                        if (leftSum < rightSum)
                        {
                            _position.X -= rowMax;

                            for (int col = 0; col < intersection.Width; col++)
                                if (collisionMask[col + rowMaxIndex * intersection.Width])
                                {
                                    _collisionPoint.X = intersection.X + col;
                                    break;
                                }
                        }
                        else 
                        {
                            _position.X += rowMax;

                            for (int col = intersection.Width - 1; col >= 0; col--)
                                if (collisionMask[col + rowMaxIndex * intersection.Width])
                                {
                                    _collisionPoint.X = intersection.X + col;
                                    break;
                                }
                        }
                    }

                    // Collision physics is applied at this block,
                    // if the other physics consists of three or more vertices. 
                    // Requiring three or more vertices is needed to have enough information
                    // to unambigiously know the directions of the normal vectors.
                    if (other.Vertices.Count >= 3)
                    {
                        // Find the closest vertex to the collision point and its index.
                        Vector2 localCollisionPoint = _collisionPoint - other.Position;
                        float[] distances = other.Vertices.Select(x => Vector2.DistanceSquared(x, localCollisionPoint)).ToArray();
                        int minIndex = distances.IndexOfMin();

                        // Find the adjacent vertices to the closest vertex.
                        int indexLower = GameMath.Modulus((minIndex - 1), other.Vertices.Count);
                        int indexHigher = GameMath.Modulus((minIndex + 1), other.Vertices.Count);
                        (Vector2 vertix, float distance, int index)[] tuples = new (Vector2 vertix, float distance, int index)[]
                        {
                            (other.Vertices[minIndex], distances[minIndex], minIndex),
                            (other.Vertices[indexLower], distances[indexLower], indexLower),
                            (other.Vertices[indexHigher], distances[indexHigher], indexHigher)
                        };

                        // Find the closest vertex and the next closest vertex that's next to the closest vertex.
                        (Vector2 vertix, int index)[] pairs = tuples
                            .OrderBy(tuple => tuple.distance)
                            .Take(2).Select(tuple => (tuple.vertix, tuple.index))
                            .ToArray();

                        // Determine the start and end point based on the original order of the vertices.
                        // Handle the special when the selected vertices are the first and last.
                        Vector2 start = Vector2.Zero, end = Vector2.Zero;
                        if (pairs[0].index > pairs[1].index || (pairs[1].index == other.Vertices.Count - 1 && pairs[0].index == 0))
                        {
                            start = pairs[0].vertix;
                            end = pairs[1].vertix;
                        }
                        else
                        {
                            start = pairs[1].vertix;
                            end = pairs[0].vertix;
                        }
                        Vector2 direction = start - end;

                        // The normal vector is the normalized vector perpendicular to the direction of the two vertices.
                        _collisionNormal = Vector2.Normalize(new Vector2(
                            x: direction.Y,
                            y: -direction.X));
                        Debug.Assert(!Double.IsNaN(_collisionNormal.X) && !Double.IsNaN(_collisionNormal.Y));
                        
                    }
                }

                // This physic handles the collision with an action.
                _collisionHandle(other);

                // Correction to position and determining point of collision
                // occurs before handling the collision with user-defined action.
                // After, physic-specific operations are applied. This way the user
                // can detect a collision and then change the physics before they occur.
                if (_solid && other.Solid)
                {
                    // Apply the bounce to velocity's normal basis vector. 
                    // The current velocity is broken down into the normal and orthogonal basis vectors.
                    // Normal scalar represents the speed along normal direction,
                    // whereas orthog scalar represents the speed along the direction perpendicular to normal direction.
                    // Bounce flips, or squashes, the normal scalar.
                    // Stick is effectively a constant velocity applied in the direction opposite of the normal direction.
                    Vector2 collisionOrthog = new Vector2(x: -_collisionNormal.Y, y: _collisionNormal.X);
                    float normalScalar = Vector2.Dot(_collisionNormal, _velocity);
                    float orthogScalar = Vector2.Dot(collisionOrthog, _velocity);
                    float totalBounce = _bounce + other.Bounce;
                    float totalFriction = _friction + other.Friction;
                    float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (Math.Abs(orthogScalar) > _stickThreshold)
                        _stickApplied = (_stick + other.Stick) * _collisionNormal;
                    
                    _velocity =  
                        - totalBounce * normalScalar * _collisionNormal // bounce component
                        + Math.Sign(orthogScalar) * GameMath.Max(Math.Abs(orthogScalar) - totalFriction * timeElapsed, 0) * collisionOrthog; // orthog component
                    Debug.Assert(!Double.IsNaN(_velocity.X) && !Double.IsNaN(_velocity.Y));
                }
            }
        }
    }
}
