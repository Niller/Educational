using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sandbox2D.Scripts.Water
{
    [Serializable]
    public struct FloatProperty
    {
        [SerializeField]
        private float _constant;

        [SerializeField]
        private float _min;
        
        [SerializeField]
        private float _max;

        [SerializeField] 
        private PropertyType _type;
        
        public float GetValue()
        {
            switch (_type)
            {
                case PropertyType.Constant:
                    return _constant;
                    break;
                case PropertyType.Random:
                    return Random.Range(_min, _max);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}