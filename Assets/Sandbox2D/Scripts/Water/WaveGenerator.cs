using System;
using UnityEngine;

namespace Sandbox2D.Scripts.Water
{
    [RequireComponent(typeof(Water))]
    public class WaveGenerator : MonoBehaviour
    {
        private Water _water;

        public FloatProperty Force;
        public float Period;
        public WaveDirection Direction;

        private float _time;
        
        private void Awake()
        {
            _water = GetComponent<Water>();
            _time = Period;
        }

        private void Update()
        {
            if (_time <= 0)
            {
                switch (Direction)
                {
                    case WaveDirection.Left:
                        _water.Splash(_water.GetLeftPosition(), Force.GetValue());
                        break;
                    case WaveDirection.Right:
                        _water.Splash(_water.GetRightPosition(), Force.GetValue());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _time = Period;
            }

            _time -= Time.deltaTime;
        }
    }
}