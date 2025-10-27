using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ColliderUtils
    {
        public static bool IsRectTransformCollidingWithObstacles(RectTransform pictureTransform, Vector2 direction, LayerMask collisionLayerMask)
        {
            return GetAllColliders(pictureTransform, direction, collisionLayerMask).Any(value => value.CompareTag("Obstacle"));
        }

        public static Vector2 GetClosestPointOnRectTransform(RectTransform pictureTransform, Vector2 direction, LayerMask collisionLayerMask, Vector2 targetPos)
        {
            Collider2D[] colliders = GetAllColliders(pictureTransform, direction, collisionLayerMask);

            float minDistance = float.MaxValue;
            Vector2 closestPoint = targetPos;

            foreach (Collider2D collider in colliders)
            {
                Vector2 point = collider.ClosestPoint(targetPos);

                float distance = Vector2.Distance(targetPos, point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        private static Collider2D[] GetAllColliders(RectTransform pictureTransform, Vector2 direction, LayerMask collisionLayerMask)
        {
            Vector2 size = pictureTransform.rect.size;
            Vector2 position = pictureTransform.transform.position;

            Vector2 offset = direction.normalized * (size.magnitude / 2);
            Vector2 newPos = position + offset;

            return Physics2D.OverlapBoxAll(
                newPos,
                size,
                pictureTransform.eulerAngles.z,
                collisionLayerMask
            );
        }
    }
}
