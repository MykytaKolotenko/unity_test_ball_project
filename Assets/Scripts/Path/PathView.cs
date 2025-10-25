using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Game.Path
{
    public class PathView : MonoBehaviour
    {
        [Inject] private CircleAnimationConfig _circleAnimationConfig;
        [SerializeField] private RectTransform _pathTransform;

        public void Init(float width, float height, Quaternion rotation)
        {
            _pathTransform.sizeDelta = new Vector2(width, height);
            _pathTransform.rotation = rotation;
        }

        public void SetWidth(float width)
        {
            _pathTransform.sizeDelta = new Vector2(width, _pathTransform.sizeDelta.y);
        }

        public Tween SetHeightAnimated(float height)
        {
            return _pathTransform.DOSizeDelta(new Vector2(_pathTransform.sizeDelta.x, height), _circleAnimationConfig.radiusTweenDuration);
        }
    }
}
