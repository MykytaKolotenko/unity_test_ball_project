using UnityEngine;

namespace Castle
{
    public class CastleView : MonoBehaviour
    {
        [SerializeField] private RectTransform castleTransform;

        public Vector3 LocalPosition => castleTransform.localPosition;
    }
}
