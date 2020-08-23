using System;
using UnityEngine;

namespace Sandbox2D.Scripts
{
    public class Rotatable : MonoBehaviour, IForceable
    {
        [SerializeField] 
        private float _minDeviation;
        [SerializeField] 
        private float _maxDeviaton;
        [SerializeField] 
        private float _damping;
        [SerializeField] 
        private float _torqueSpeed;
        [SerializeField]
        private float _springForceMultiplier;
        [SerializeField] 
        private float _torqueForceMultiplier = 10f;
        [SerializeField] 
        private float _customForce;

        private float _torqueForce;
        private float _springForce;
        
        private Quaternion _minAngle;
        private Quaternion _maxAngle;
        private Quaternion _centerAngle;
        private float _angularVelocity;
        private float _angularAcceleration;
        

        private void Awake()
        {
            _centerAngle = transform.rotation;
            _minAngle = Quaternion.Euler(_centerAngle.eulerAngles + Vector3.forward * _minDeviation);
            _maxAngle = Quaternion.Euler(_centerAngle.eulerAngles + Vector3.forward * _maxDeviaton);
            Time.timeScale = 0.1f;
            
        }

        [ContextMenu("Apply Impulse")]
        private void ApplyImpulse()
        {
            ApplyForce(_customForce);
            Debug.Break();
        }

        [ContextMenu("Reset")]
        private void Reset()
        {
            _angularVelocity = 0;
            _angularAcceleration = 0;
        }

        private void ApplyForce(float force)
        {
            _angularAcceleration = force * _torqueForceMultiplier;
            /*
            force *= _torqueForceMultiplier;
            if (force * _torqueForce < 0)
            {
                _torqueForce = force;
            }
            else
            {
                if (_torqueForce * _torqueForce < force * force)
                {
                    _torqueForce = force;
                }    
            }
            */
        }

        private void LateUpdate()
        {
            
            var rotation = Quaternion.AngleAxis(_angularVelocity, Vector3.forward);
            rotation = ClampRotation(rotation, new Vector3(0, 0, _maxDeviaton));
            transform.rotation = rotation;
            
            UpdateVelocity(); 
            //UpdateAcceleration();
            
            /*
            if (_torqueForce * _torqueForce > Mathf.Epsilon)
            {
                
            }
            else
            {
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, _centerAngle, _springForce * _torqueSpeed);
            }

            ApplyDamping();
            */
        }

        private void UpdateVelocity()
        {
            _angularVelocity += _angularAcceleration;
            var diff = Quaternion.Angle(transform.rotation, _centerAngle);
            var relativeDiff = diff / _maxDeviaton;
            _angularVelocity -= relativeDiff * _angularVelocity;
        }

        private void UpdateAcceleration()
        {
            
            var diff = Quaternion.Angle(transform.rotation, _centerAngle);
            _angularAcceleration += diff * _damping;
            Debug.Log(_angularAcceleration);
            //_angularAcceleration *= _damping;
        }

        /*
        private void ApplyDamping()
        {
            var angle = Quaternion.Angle(transform.rotation, _centerAngle);
            //_springForce = 
            
            if (_torqueForce * _torqueForce > Mathf.Epsilon)
            {
                _torqueForce *= _damping;
            }
            else
            {
                _springForce *= _damping;
            }
        }
        */
        
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

        public void ApplyForce(Vector2 force)
        {
            ApplyForce(force.x);
        }
    }
}