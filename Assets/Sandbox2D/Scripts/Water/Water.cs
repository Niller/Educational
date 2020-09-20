using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sandbox2D.Scripts.Water
{
    public class Water : MonoBehaviour
    {
        private const string DynamicObjectName = "Dynamic";
        
        public Vector2 Size;
        public int VertexDensity = 5;
        
        public float Spring = 0.02f;
        public float Damping = 0.04f;
        public float Spread = 0.05f;
        public int WaveSpreadIterationsCount = 8;
        
        public Material WaterMaterial;

        private bool _spawned;
        private Mesh _dynamicMesh;
        private GameObject _dynamic;
        private NodeInfo[] _nodeInfos;
        private Vector3[] _cachedVertices;
        

        private void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            TrySpawn();
        }
        
        [ContextMenu("Spawn")]
        private void TrySpawn()
        {
            if (_spawned)
            {
                return;
            }

            _dynamic = new GameObject(DynamicObjectName);
            _dynamic.transform.SetParent(transform);
            var meshFilter = _dynamic.AddComponent<MeshFilter>();
            var meshRenderer = _dynamic.AddComponent<MeshRenderer>();
            _dynamicMesh = meshFilter.mesh = new Mesh();
            meshRenderer.sharedMaterial = WaterMaterial;
            
            FillNodeInfo();
            FillMesh();
            
            _spawned = true;
        }

        private void FillNodeInfo()
        {
            var edgeCount = Mathf.RoundToInt(Size.x) * VertexDensity;
            var nodeCount = edgeCount + 1;
            _nodeInfos = new NodeInfo[nodeCount];
            
            var p = transform.position;
            var left = p.x - Size.x / 2f;
            var top = p.y + Size.y / 2f;
            
            for (var i = 0; i < nodeCount; i++)
            {
                _nodeInfos[i] = new NodeInfo(new Vector2(left + Size.x * i / edgeCount, top), 0, 0);
            }
        }

        private void FillMesh()
        {
            var nodeLength = _nodeInfos.Length;
            var vertexCount = nodeLength * 2;
            _cachedVertices = new Vector3[vertexCount];
            var uv = new Vector2[vertexCount];
            var triangles = new int[vertexCount  * 3];
            for (var i = 0; i < nodeLength - 1; ++i)
            {
                //Vertices
                var index = i * 2;
                var nodeInfo = _nodeInfos[i];
                var nodeInfoNext = _nodeInfos[i + 1];
                
                _cachedVertices[index] = nodeInfo.Position; 
                _cachedVertices[index + 1] = new Vector3(nodeInfo.Position.x, nodeInfo.Position.y - Size.y, 0); 
                _cachedVertices[index + 2] = nodeInfoNext.Position; 
                _cachedVertices[index + 3] = new Vector3(nodeInfoNext.Position.x, nodeInfoNext.Position.y - Size.y, 0);

                //UV
                uv[index] = new Vector2(i / (nodeLength - 1f), 0f);
                uv[index + 1] = new Vector2(i / (nodeLength - 1f), 1f);
                uv[index + 2] = new Vector2((i + 1f) / (nodeLength - 1f), 0f);
                uv[index + 3] = new Vector2((i + 1f) / (nodeLength - 1f), 1f);
                
                //Triangles
                var triangleIndex = i * 6;
                /*
                 * ---
                 * \ |
                 *  \|
                 */
                triangles[triangleIndex] = index;
                triangles[triangleIndex + 1] = index + 2;
                triangles[triangleIndex + 2] = index + 3;
                
                /*
                 * |\
                 * | \
                 * ---
                 */
                triangles[triangleIndex + 3] = index + 3;
                triangles[triangleIndex + 4] = index + 1;
                triangles[triangleIndex + 5] = index;
            }
            
            _dynamicMesh.vertices = _cachedVertices;
            _dynamicMesh.uv = uv;
            _dynamicMesh.triangles = triangles;
        }

        private void UpdateMesh()
        {
            var nodeLength = _nodeInfos.Length;
            for (var i = 0; i < nodeLength - 1; ++i)
            {
                var index = i * 2;
                var nodeInfo = _nodeInfos[i];
                var nodeInfoNext = _nodeInfos[i + 1];
                _cachedVertices[index] = nodeInfo.Position; 
                _cachedVertices[index + 1] = new Vector3(nodeInfo.Position.x, nodeInfo.Position.y - Size.y, 0); 
                _cachedVertices[index + 2] = nodeInfoNext.Position; 
                _cachedVertices[index + 3] = new Vector3(nodeInfoNext.Position.x, nodeInfoNext.Position.y - Size.y, 0);
            }

            _dynamicMesh.vertices = _cachedVertices;
        }

        private void FixedUpdate()
        {
            var baseHeight = transform.position.y + Size.y / 2f;
            for (int i = 0, len = _nodeInfos.Length; i < len; ++i)
            {
                var nodeInfo = _nodeInfos[i];
                var force = Spring * (nodeInfo.Position.y - baseHeight) + nodeInfo.Velocity * Damping;
                nodeInfo.Acceleration = -force;
                nodeInfo.Position.y += nodeInfo.Velocity;
                nodeInfo.Velocity += nodeInfo.Acceleration;
            }
            
            for (var j = 0; j < WaveSpreadIterationsCount; j++)
            {
                for (int i = 0, len = _nodeInfos.Length; i < len; ++i)
                {
                    var nodeInfo = _nodeInfos[i];
                    
                    if (i > 0)
                    {
                        var prevNodeInfo = _nodeInfos[i - 1];
                        nodeInfo.LeftDelta = Spread * (nodeInfo.Position.y - prevNodeInfo.Position.y);
                        prevNodeInfo.Velocity += nodeInfo.LeftDelta;
                    }
                    
                    if (i < len - 1)
                    {
                        var nextNodeInfo = _nodeInfos[i + 1];
                        nodeInfo.RightDelta = Spread * (nodeInfo.Position.y - nextNodeInfo.Position.y);
                        nextNodeInfo.Velocity += nodeInfo.RightDelta;
                    }
                }
            }
            
            for (int i = 0, len = _nodeInfos.Length; i < len; ++i)
            {
                var nodeInfo = _nodeInfos[i];
                if (i > 0)
                {
                    var prevNodeInfo = _nodeInfos[i - 1];
                    prevNodeInfo.Position.y += nodeInfo.LeftDelta;
                }
                if (i < len - 1)
                {
                    var nextNodeInfo = _nodeInfos[i + 1];
                    nextNodeInfo.Position.y += nodeInfo.RightDelta;
                }
            }

            UpdateMesh();
        }


        public void Splash(float xPos, float velocity)
        {
            if (xPos < _nodeInfos[0].Position.x || xPos > _nodeInfos[_nodeInfos.Length - 1].Position.x)
            { 
                return;   
            }

            var relativeXPos = xPos - _nodeInfos[0].Position.x;
            var index = Mathf.RoundToInt(
                (_nodeInfos.Length - 1) * 
                (relativeXPos / (_nodeInfos[_nodeInfos.Length-1].Position.x - _nodeInfos[0].Position.x)));
            _nodeInfos[index].Velocity = velocity;
        }

        public float GetLeftPosition()
        {
            return _nodeInfos[0].Position.x;
        }

        public float GetRightPosition()
        {
            return _nodeInfos[_nodeInfos.Length - 1].Position.x;
        }
        
        public float GetTopPosition()
        {
            return _nodeInfos[0].Position.y;
        }

        [ContextMenu("Splash")]
        private void RandomSplash()
        {
            Splash(
                Random.Range(_nodeInfos[0].Position.x, _nodeInfos[_nodeInfos.Length - 1].Position.x), 
                1f);
        }

        [ContextMenu("Respawn")]
        private void Respawn()
        {
            if (_spawned)
            {
                Despawn();
            }
            
            TrySpawn();
        }

        [ContextMenu("Despawn")]
        private void Despawn()
        {
            if (!_spawned)
            {
                return;
            }
            
            if (Application.isPlaying)
            {
                Destroy(_dynamic);
            }
            else
            {
                DestroyImmediate(_dynamic);
            }
            
            _spawned = false;
        }

        private void OnDrawGizmos()
        {
            var cachedColor = Gizmos.color;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(Size.x, Size.y, 0.1f));
            Gizmos.color = cachedColor;
        }

        private class NodeInfo
        {
            public Vector2 Position;
            public float Acceleration;
            public float Velocity;
            public float RightDelta;
            public float LeftDelta;

            public NodeInfo(Vector2 position, float acceleration, float velocity)
            {
                Position = position;
                Acceleration = acceleration;
                Velocity = velocity;

                RightDelta = 0;
                LeftDelta = 0;
            }
        } 
    }
}
