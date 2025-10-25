using DG.Tweening;
using Game;
using UnityEngine;

namespace Projectile
{
    public class ProjectileView : AbstractCircleView
    {
        public override Tween MoveTo(Vector3 position)
        {
            return circleTransform.DOJump(position, 1, 3, CircleAnimationConfig.moveTweenDuration);
        }
    }
}
