using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class TouchInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnTapStarted;
        public event Action OnTapEnded;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnTapStarted?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnTapEnded?.Invoke();
        }
    }
}
