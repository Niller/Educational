using System.Runtime.CompilerServices;

namespace LinearAlgebra
{
    public readonly struct ArraySlice<T> where T : struct
    {
        public readonly T[] Array;
        public readonly int StartIndex;
        public readonly int Size;

        public ArraySlice(T[] array, int startIndex, int size)
        {
            Array = array;
            StartIndex = startIndex;
            Size = size;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(int i, T value)
        {
            Array[StartIndex + i] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetValue(int i)
        {
            return Array[StartIndex + i];
        }
    }
}