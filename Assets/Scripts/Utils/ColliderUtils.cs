using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ColliderUtils
    {
        public static bool IsRectTransformCollidingWithObstacles(RectTransform pictureTransform, Vector2 direction, LayerMask collisionLayerMask)
        {
            Vector2 size = pictureTransform.rect.size;
            Vector2 position = pictureTransform.transform.position;

            Vector2 offset = direction.normalized * (size.magnitude / 2);
            Vector2 newPos = position + offset;

            Collider2D[] collision = Physics2D.OverlapBoxAll(
                newPos,
                size,
                pictureTransform.eulerAngles.z,
                collisionLayerMask
            );

            return collision.Any(value => value.CompareTag("Obstacle"));
        }
    }
}
