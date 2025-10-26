using Configs;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Path
{
    public class PathView : MonoBehaviour
    {
        [field: SerializeField] public RectTransform PathRectTransform { get; private set; }

        [Inject] private CircleAnimationConfig _circleAnimationConfig;

        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = PathRectTransform.position;
        }

        public void Init(float width, float height, Quaternion rotation)
        {
            PathRectTransform.sizeDelta = new Vector2(width, height);
            PathRectTransform.rotation = rotation;
            PathRectTransform.position = _startPosition;
        }

        public void SetWidth(float width)
        {
            PathRectTransform.sizeDelta = new Vector2(width, PathRectTransform.sizeDelta.y);
        }

        public Tween SetHeightAnimated(float height)
        {
            return PathRectTransform.DOSizeDelta(new Vector2(PathRectTransform.sizeDelta.x, height), _circleAnimationConfig.moveTweenDuration);
        }

        public Tween SetPositionAnimated(Vector3 position)
        {
            return PathRectTransform.DOLocalJump(position, 10f, 1, _circleAnimationConfig.moveTweenDuration);
        }
    }
}
