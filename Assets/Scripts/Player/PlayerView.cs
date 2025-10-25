using DG.Tweening;
using Game;
using UnityEngine;

namespace Player
{
    public class PlayerView : AbstractCircleView
    {
        public override Tween MoveTo(Vector3 position)
        {
            return circleTransform.DOMove(position, CircleAnimationConfig.moveTweenDuration);
        }
    }
}
