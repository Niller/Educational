using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SharpDX;

namespace LinearAlgebra.Performance
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [RPlotExporter]
    public class VectorPerformanceTests
    {
        private Random _random;
        private float[] _values;
        private Vector3[] _sharpDxVector3;
        private Vector[] _myVector;
        private NaiveVector[] _naiveVectors;
        private List<float[]> _temp = new List<float[]>();
        
        [GlobalSetup]
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public void Setup()
        {
            _random = new Random();
            const int collectionSize = 1000000;

            _sharpDxVector3 = new Vector3[collectionSize];
            _myVector = new Vector[collectionSize];
            _naiveVectors = new NaiveVector[collectionSize];
            for (var i = 0; i < collectionSize; i++)
            {
                _values = new float[3]
                {
                    _random.Next(),
                    _random.Next(),
                    _random.Next()
                };
                
                _sharpDxVector3[i] = new Vector3(_values);
                _myVector[i] = new Vector(3, _values);
                _naiveVectors[i] = new NaiveVector(3, _values);
                _temp.Add(ArrayPool<float>.Shared.Rent((int) 1E+3));
            }
            
        }
        
        [Benchmark]
        public float MagnitudeSharpDx()
        {
            var sum = 0f;
            for (int i = 0, len = _sharpDxVector3.Length; i < len; i++)
            {
                sum += _sharpDxVector3[i].Length();
            }

            return sum;

        }
        
        [Benchmark]
        public float MagnitudeMyVector()
        {
            var sum = 0f;
            for (int i = 0, len = _myVector.Length; i < len; i++)
            {
                sum += _myVector[i].Magnitude;
            }

            return sum;
        }
        /*
        [Benchmark]
        public float MagnitudeNaiveVector()
        {
            var sum = 0f;
            for (int i = 0, len = _naiveVectors.Length; i < len; i++)
            {
                sum += _naiveVectors[i].Magnitude;
            }

            return sum;
        }
        */
    }
}