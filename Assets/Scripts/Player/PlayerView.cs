using DG.Tweening;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerView : AbstractCircleView
    {
        public new void Init(float radius)
        {
            base.Init(radius);
            circleTransform.transform.position = StartPosition;
        }

        public Tween MoveTo(Vector3 position)
        {
            return AnimationUtils.JumpTo(circleTransform, position, CircleAnimationConfig.moveTweenDuration);
        }
    }
}
