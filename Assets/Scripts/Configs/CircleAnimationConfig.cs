using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CircleAnimationConfig", menuName = "CircleAnimationConfig", order = 1)]
    public class CircleAnimationConfig : ScriptableObject
    {
        public float radiusTweenDuration = 1f;
        public float moveTweenDuration = 1f;
    }
}
