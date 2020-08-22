using System.Collections.Generic;
using UnityEngine;

namespace Rope2D
{
    [RequireComponent(typeof(LineRenderer))]
    public class Bowable : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private readonly List<RopeSegment> _ropeSegments = new List<RopeSegment>();
        
        [SerializeField]
        private float _ropeSegLen = 0.25f;
        [SerializeField]
        private int _segmentLength = 35;
        [SerializeField]
        private float _lineWidth = 0.1f;
        
        [SerializeField]
        private float _startAngle = 90f;
        [SerializeField]
        private float _angleDeviation = 30f;
        [SerializeField] 
        private float _speed = 1f;
        [SerializeField] 
        private int _simulationCount = 50;
        
        private float _time;
        private float _currentAngle;
        private float _length;

        void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            var ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            for (var i = 0; i < _segmentLength; i++)
            {
                _ropeSegments.Add(new RopeSegment(ropeStartPoint));
                ropeStartPoint.y -= _ropeSegLen;
            }

            _length = _ropeSegments.Count * _ropeSegLen;
            _currentAngle = _startAngle;
        }

        void Update()
        {
            DrawRope();
            _time += Time.deltaTime * _speed;
        }

        private void FixedUpdate()
        {
            Simulate();
        }

        private void Simulate()
        {
            for (var i = 0; i < _simulationCount; i++)
            {
                ApplyConstraint();
            }
        }

        private void ApplyConstraint()
        {
            var firstSegment = _ropeSegments[0];
            firstSegment.PosNow = transform.position;
            _ropeSegments[0] = firstSegment;
            
            var lastSegment = _ropeSegments[_ropeSegments.Count - 1];

            var x = _length * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
            var y = _length * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);
            
            _currentAngle = _startAngle + _angleDeviation * Mathf.Sin(_time);
            
            lastSegment.PosNow = firstSegment.PosNow + new Vector2(x, y);
            _ropeSegments[_ropeSegments.Count - 1] = lastSegment;

            for (var i = _segmentLength - 2; i >= 0; i--)
            {
                
                var firstSeg = _ropeSegments[i];
                var secondSeg = _ropeSegments[i + 1];

                var diff = (firstSeg.PosNow - secondSeg.PosNow);

                if (i != 0)
                {
                    firstSeg.PosNow -= diff * 0.25f;
                    _ropeSegments[i] = firstSeg;
                    secondSeg.PosNow += diff * 0.25f;
                    _ropeSegments[i + 1] = secondSeg;
                }
                else
                {
                    secondSeg.PosNow += diff;
                    this._ropeSegments[i + 1] = secondSeg;
                }
            }
        }

        private void DrawRope()
        {
            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;

            var ropePositions = new Vector3[_segmentLength];
            for (var i = 0; i < _segmentLength; i++)
            {
                ropePositions[i] = _ropeSegments[i].PosNow;
            }

            _lineRenderer.positionCount = ropePositions.Length;
            _lineRenderer.SetPositions(ropePositions);
        }

        private struct RopeSegment
        {
            public Vector2 PosNow;
            public Vector2 PosOld;

            public RopeSegment(Vector2 pos)
            {
                this.PosNow = pos;
                this.PosOld = pos;
            }
        }
    }
}