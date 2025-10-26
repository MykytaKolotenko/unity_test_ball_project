using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class TouchInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnTapStarted;
        public event Action OnTapEnded;

        private bool _isInputEnabled;
        public bool IsInputEnabled
        {
            get => _isInputEnabled;
            set
            {
                _isInputEnabled = value;
                _isPointerDown = false;
            }
        }

        private bool _isPointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isInputEnabled) return;

            _isPointerDown = true;
            OnTapStarted?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isInputEnabled &&
                !_isPointerDown) return;

            _isPointerDown = false;
            OnTapEnded?.Invoke();
        }
    }
}
