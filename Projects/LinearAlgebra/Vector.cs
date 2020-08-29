using System;

namespace LinearAlgebra
{
    public readonly struct Vector
    {
        private readonly ArraySlice<float> _values;
        private readonly float _size;

        public float Size => _size;

        public float Magnitude
        {
            get
            {
                var result = 0f;
                for (var i = 0; i < _size; ++i)
                {
                    var item = _values.GetValue(i);
                    result += item * item;
                }

                return MathF.Sqrt(result);
            }
        }

        public Vector(int n, params float[] values)
        {
            _size = n;
            _values = MemoryManager<float>.Alloc(n);
            
            for (var i = 0; i < n; ++i)
            {
                _values.SetValue(i, values[i]);
            }
        }
        
    }
}