using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Sandbox2D.Scripts.Water
{
    [RequireComponent(typeof(Water))]
    [RequireComponent(typeof(BuoyancyEffector2D))]
    public class WaterBuoyancy : MonoBehaviour
    {
        public bool UseWaves;
        public float BaseAngleForce;
        public float WaveForce;
        public FloatProperty FloatingForce;
        public float FloatingPeriod;
        
        private Water _water;
        private BuoyancyEffector2D _buoyancy;
        private WaveGenerator _waveGenerator;
        private readonly HashSet<SailingObject> _sailingObjects = new HashSet<SailingObject>();
        
        private void Awake()
        {
            _water = GetComponent<Water>();
            _buoyancy = GetComponent<BuoyancyEffector2D>();
            _waveGenerator = GetComponent<WaveGenerator>();
        }

        private void Start()
        {
            SetupPhysics();
        }

        private void SetupPhysics()
        {
            var c = GetComponent<BoxCollider2D>();
            if (c == null)
            {
                return;
            }
            c.size = _water.Size;
            _buoyancy.surfaceLevel = _water.Size.y / 2;
        }

        private void FixedUpdate()
        {
            if (UseWaves)
            {
                _buoyancy.flowVariation = WaveForce * _waveGenerator.Force.GetValue();
                UpdateFlowAngle(_waveGenerator.Direction);
            }
            else
            {
                _buoyancy.flowVariation = BaseAngleForce;
                var direction = (WaveDirection) Random.Range(0, 2);
                UpdateFlowAngle(direction);
            }
        }

        private void UpdateFlowAngle(WaveDirection direction)
        {
            switch (direction)
            {
                case WaveDirection.Left:
                    _buoyancy.flowAngle = 0;
                    break;
                case WaveDirection.Right:
                    _buoyancy.flowAngle = 180;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var sailingObject = other.gameObject.GetComponent<SailingObject>();
            if (sailingObject == null)
            {
                return;
            }
            
            _water.Splash(other.transform.position.x, sailingObject.GetFallForce());

            sailingObject.SetFloatingForce(FloatingForce, FloatingPeriod);
            _sailingObjects.Add(sailingObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var sailingObject = other.gameObject.GetComponent<SailingObject>();
            if (sailingObject != null)
            {
                _sailingObjects.Remove(sailingObject);
            }
        }
    }
}