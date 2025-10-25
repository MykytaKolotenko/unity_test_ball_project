using UnityEngine;

namespace Game.Castle
{
    public class CastleView : MonoBehaviour
    {
        [SerializeField] private RectTransform castleTransform;

        public Vector3 Position => castleTransform.position;
    }
}
