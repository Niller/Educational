using System;
using NUnit.Framework;
using SharpDX;

namespace LinearAlgebra.Tests
{
    public class VectorTests
    {
        private Random _random;
        
        [SetUp]
        public void Setup()
        {
            _random = new Random();
        }

        [Test]
        public void MagnitudeTest()
        {
            const int size = 3;
            const int testCount = 100;

            for (int i = 0; i < testCount; i++)
            {
                var values = new float[3]
                {
                    _random.Next(),
                    _random.Next(),
                    _random.Next()
                };
            
                var sharpDxVector = new Vector3(values);
                var myVector = new Vector(size, values);
            
                Assert.AreEqual(sharpDxVector.Length(), myVector.Magnitude);
            }
        }
    }
}