using System.Collections;
using UnityEngine;

namespace Sandbox2D.Scripts.Water
{
    public class WaterDrops : MonoBehaviour
    {
        public float Duration;
        
        public void Initialize()
        {
            StartCoroutine(WaitDuration());
        }

        private IEnumerator WaitDuration()
        {
            yield return new WaitForSeconds(Duration);
            Destroy(gameObject);
        }
    }
}