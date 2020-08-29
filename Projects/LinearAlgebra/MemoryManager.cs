using System.Buffers;

namespace LinearAlgebra
{
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