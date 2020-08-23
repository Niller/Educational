using System;
using UnityEngine;

namespace Sandbox2D.Scripts
{
    public class RotatablePhysics2D : MonoBehaviour, IForceable
    {
        
        [SerializeField] 
        private float _customForce;

        [SerializeField]
        private float _forceMultiplier;
        
        private float _minDeviation;
        private float _maxDeviation;
        
        private void Awake()
        {
            _maxDeviation = GetComponent<HingeJoint2D>().limits.max;
            _minDeviation = GetComponent<HingeJoint2D>().limits.min;
        }
        
        public void ApplyForce(Vector2 force)
        {
            GetComponent<Rigidbody2D>().AddTorque(force.x * _forceMultiplier);
            //GetComponent<HingeJoint>().sp
        }

        private void LateUpdate()
        {
            /*
            var r = GetComponent<Rigidbody2D>();
            var rotation = r.rotation;
            var newRotation = Mathf.Clamp(rotation, _minDeviation, _maxDeviation);
            if (Mathf.Abs(newRotation - rotation) > Mathf.Epsilon) 
            {
                r.SetRotation(rotation);    
            }
            */
        }

        [ContextMenu("Apply Impulse")]
        private void ApplyImpulse()
        {
            GetComponent<Rigidbody2D>().AddTorque(_customForce);
            //Debug.Break();
        }

        [ContextMenu("Reset")]
        private void Reset()
        {
            //_angularVelocity = 0;
            //_angularAcceleration = 0;
        }
        
        private static Quaternion ClampRotation(Quaternion q, Vector3 bounds)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
 
            var angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
 
            var angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
            angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
            q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);
 
            var angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
            angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
            q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);
 
            return q;
        }
    }
}