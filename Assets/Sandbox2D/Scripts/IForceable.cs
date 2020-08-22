using UnityEngine;

namespace Sandbox2D.Scripts
{
    public interface IForceable
    {
        void ApplyForce(Vector2 force);
    }
}