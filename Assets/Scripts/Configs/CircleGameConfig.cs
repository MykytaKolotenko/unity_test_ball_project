using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "CircleGameConfig", menuName = "CircleGameConfig", order = 1)]
    public class CircleGameConfig : ScriptableObject
    {
        public float PlayerRadius;
        public float MinimumPlayerRadius;
        public float SquareReductionPercent = 1;
        public float MinSquareReduction = 10000f;

        public float ProjectileSpeed;
    }
}
