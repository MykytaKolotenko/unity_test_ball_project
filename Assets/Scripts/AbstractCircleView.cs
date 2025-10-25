using DG.Tweening;
using UnityEngine;
using Utils;
using Zenject;

namespace Game
{
    public abstract class AbstractCircleView : MonoBehaviour
    {
        [SerializeField] protected RectTransform circleTransform;

        [Inject] protected CircleAnimationConfig CircleAnimationConfig;

        public Vector3 Position => circleTransform.position;
        public Vector3 LocalPosition => circleTransform.localPosition;

        public float Radius { get; protected set; }

        public void Init(float radius)
        {
            SetRadius(radius);
        }

        public void SetRadius(float radius)
        {
            Radius = radius;
            UpdateView();
        }

        protected void UpdateView()
        {
            circleTransform.sizeDelta = MathUtils.GetDimensionsFromRadius(Radius);
        }

        public abstract Tween MoveTo(Vector3 position);
    }
}
