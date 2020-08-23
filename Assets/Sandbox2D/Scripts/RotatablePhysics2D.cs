using UnityEngine;

namespace Sandbox2D.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RotatablePhysics2D : MonoBehaviour, IForceable
    {
        [SerializeField]
        private float _forceMultiplier;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void ApplyForce(Vector2 force)
        {
            _rigidbody2D.AddTorque(force.x * _forceMultiplier);
        }
    }
}