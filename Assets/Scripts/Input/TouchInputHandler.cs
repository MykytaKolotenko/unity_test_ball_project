using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class TouchInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnTapStarted;
        public event Action OnTapEnded;

        public bool IsInputEnabled { get; set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsInputEnabled) return;
            OnTapStarted?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsInputEnabled) return;
            OnTapEnded?.Invoke();
        }
    }
}
