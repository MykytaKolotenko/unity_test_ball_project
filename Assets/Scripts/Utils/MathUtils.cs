using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static Vector2 GetDimensionsFromRadius(float radius)
        {
            return new Vector2(radius * 2, radius * 2);
        }

        public static Quaternion GetAngle(Vector2 direction)
        {
            float angleRad = Mathf.Atan2(direction.y, direction.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            return Quaternion.Euler(0f, 0f, angleDeg - 90f);
        }

        public static float CalculateCircleArea(float radius)
        {
            return Mathf.PI * radius * radius;
        }

        public static float GetRadiusFromArea(float area)
        {
            return Mathf.Sqrt(area / Mathf.PI);
        }
    }
}
