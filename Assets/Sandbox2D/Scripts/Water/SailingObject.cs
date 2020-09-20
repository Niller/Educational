using System;
using UnityEngine;

namespace Sandbox2D.Scripts.Water
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SailingObject : MonoBehaviour
    {
        public float FallForceMultiply = 0.04f;
        
        private Rigidbody2D _rigidbody2D;
        private FloatProperty _floatingForce;
        private float _time = 0;
        private bool _useForce;
        private float _period;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _time = 2;
        }

        public void SetFloatingForce(FloatProperty force, float period)
        {
            _floatingForce = force;
            _time = _period = period;
        }

        public float GetFallForce()
        {
            return _rigidbody2D.velocity.y * _rigidbody2D.mass * FallForceMultiply;
        }

        private void Update()
        {
            if (_time <= 0)
            {
                _useForce = true;
                _time = _period;
            }

            _time -= Time.deltaTime;
        }
        
        private void FixedUpdate()
        {
            if (!_useForce)
            {
                return;
            }
            
            ApplyFloatingForce();
            _useForce = false;
        }

        private void ApplyFloatingForce()
        {
            _rigidbody2D.AddForce(new Vector2(0, -_floatingForce.GetValue()), ForceMode2D.Impulse); 
        }
    }
}