using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sandbox2D.Scripts
{
    public class SandboxWorld : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField]
        private float _scrollSpeed;

        [SerializeField] 
        private float _scrollEventThreshold;
        
        private IScrollListener[] _scrollListeners;
        private Camera _camera;
        private Vector2 _screenResolution;
        private float _lastScrollSqrMagnitude;
        
        private void Awake()
        {
            _camera = Camera.main;
            _screenResolution = new Vector2(Screen.width, Screen.height);
            _scrollListeners = GetComponentsInChildren<IScrollListener>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //no op
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //no op            
        }

        public void OnDrag(PointerEventData eventData)
        {
            var relativeScroll = eventData.delta / _screenResolution;
            var scroll = relativeScroll * _scrollSpeed;
            scroll.y = 0;
            _camera.transform.position += (Vector3)scroll;
            var currentScrollSqrMagnitude = relativeScroll.sqrMagnitude * 10000f;
            //Debug.Log(Mathf.Abs(currentScrollSqrMagnitude - _lastScrollSqrMagnitude));
            //if (Mathf.Abs(currentScrollSqrMagnitude - _lastScrollSqrMagnitude) > _scrollEventThreshold)
            {
                //Debug.Log(Mathf.Abs(currentScrollSqrMagnitude - _lastScrollSqrMagnitude));
                foreach (var scrollListener in _scrollListeners)
                {
                    scrollListener.OnScroll(scroll);
                }
            }

            _lastScrollSqrMagnitude = currentScrollSqrMagnitude;
        }
    }
}