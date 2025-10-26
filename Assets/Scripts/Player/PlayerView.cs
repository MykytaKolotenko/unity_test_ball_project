using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerView : AbstractCircleView
    {
        public new void Init(float radius)
        {
            base.Init(radius);
            circleTransform.transform.position = StartPosition;
        }

        public override Tween MoveTo(Vector3 position)
        {
            return circleTransform.DOLocalJump(position, 10f, 1, CircleAnimationConfig.moveTweenDuration);
        }
    }
}
