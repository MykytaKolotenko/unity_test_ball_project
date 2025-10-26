using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ColliderUtils
    {
        public static bool IsRectTransformCollidingWithObstacles(RectTransform pictureTransform, LayerMask collisionLayerMask)
        {
            Vector2 size = pictureTransform.sizeDelta;
            Vector2 position = pictureTransform.transform.position;

            Collider2D[] collision = Physics2D.OverlapBoxAll(position, size, pictureTransform.eulerAngles.z, collisionLayerMask);

            return collision.Any(value => value.CompareTag("Obstacle"));
        }
    }
}
