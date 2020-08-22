using UnityEngine;

namespace Sandbox2D.Scripts
{
    public interface IScrollListener
    {
        void OnScroll(Vector2 scroll);
    }
}