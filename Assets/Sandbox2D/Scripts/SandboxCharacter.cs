using UnityEngine;

namespace Sandbox2D.Scripts
{
    public class SandboxCharacter : MonoBehaviour, IScrollListener
    {
        private IForceable[] _forceables;

        private void Awake()
        {
            _forceables = GetComponentsInChildren<IForceable>();
        }

        public void OnScroll(Vector2 scroll)
        {
            foreach (var forceable in _forceables)
            {
                forceable.ApplyForce(scroll * 10);
            }
        }
    }
}
