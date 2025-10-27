using UnityEngine;
using UnityEngine.UI;

namespace Castle
{
    public class CastleView : MonoBehaviour
    {
        [SerializeField] private RectTransform castleTransform;
        [SerializeField] private Image _door;

        public Vector3 LocalPosition => castleTransform.localPosition;

        public bool IsDoorOpen => !_door.gameObject.activeSelf;

        public void Init()
        {
            CloseDoor();
        }

        public void OpenDoor()
        {
            _door.gameObject.SetActive(false);
        }

        private void CloseDoor()
        {
            _door.gameObject.SetActive(true);
        }
    }
}
