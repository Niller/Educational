using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sandbox2D.Scripts.Water
{
    [RequireComponent(typeof(Water))]
    [RequireComponent(typeof(BuoyancyEffector2D))]
    public class WaterBuoyancy : MonoBehaviour
    {
        public bool UseWaves;
        public float BaseFloatForce;
        public float WaveForce;
        
        private Water _water;
        private BuoyancyEffector2D _buoyancy;
        private WaveGenerator _waveGenerator;
        
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
                return;
            }
            
            _buoyancy.flowVariation = BaseFloatForce;
            var direction = (WaveDirection)Random.Range(0, 2);
            UpdateFlowAngle(direction);
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
    }
}