using System;
using System.Buffers;
using System.Collections.Generic;
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
    
    public static class MemoryManager<T> where T : struct
    {
        private static ArrayPool<T> _pool;
        //private static List<T[]> _arrays = new List<T[]>();
        private static T[] _array;
        private static int _currentIndex = 0;
        
        static MemoryManager()
        {
            _pool = ArrayPool<T>.Shared;
            _array = _pool.Rent((int)1E+7);
            //var arrayPool = ArrayPool<T>.Shared;
            //arrayPool.Rent()

        }

        public static ArraySlice<T> Alloc(int size)
        {
            var result = new ArraySlice<T>(_array, _currentIndex, size);
            _currentIndex += size;
            return result;
        }
    }
}