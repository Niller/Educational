using System;

namespace LinearAlgebra
{
    public readonly struct NaiveVector
    {
        private readonly float[] _values;
        private readonly float _size;

        public float Size => _size;

        public float Magnitude
        {
            get
            {
                var result = 0f;
                for (var i = 0; i < _size; ++i)
                {
                    var item = _values[i];
                    result += item * item;
                }

                return MathF.Sqrt(result);
            }
        }

        public NaiveVector(int n, params float[] values)
        {
            _size = n;
            _values = values;
        }
    }
}