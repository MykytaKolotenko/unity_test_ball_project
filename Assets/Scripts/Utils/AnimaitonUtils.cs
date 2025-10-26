using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class AnimationUtils
    {
        public static Tween JumpTo(Transform transform, Vector3 position, float duration)
        {
            return transform.DOLocalJump(position, 100f, 3, duration).SetEase(Ease.Linear);
        }
    }
}
