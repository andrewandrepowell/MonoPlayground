using System;
using System.Collections.Generic;
using System.Text;

namespace MonoPlayground
{
    internal abstract class GameObject 
    {
#nullable enable
        private readonly ICollection<GameObject> _children;
        private readonly PhysicsFeature? _physics;
        public GameObject(PhysicsFeature? physics = null)
        {
            _children = new List<GameObject>();
            _physics = physics;
        }
        public ICollection<GameObject> Children { get => _children; }
        public PhysicsFeature Physics { get => (_physics == null) ? throw new NullReferenceException() : _physics; }
#nullable disable
    }
}
