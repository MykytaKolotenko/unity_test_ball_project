using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obstacle
{
    public class ObstacleManager : MonoBehaviour
    {
        private readonly List<ObstacleController> _obstacles = new List<ObstacleController>();

        public void Reinit()
        {
            _obstacles.ForEach(value => value.gameObject.SetActive(true));
        }

        public void Add(ObstacleController obstacle)
        {
            _obstacles.Add(obstacle);
        }

        public void Remove(ObstacleController obstacle)
        {
            obstacle.gameObject.SetActive(false);
        }

        public void DestroyObstaclesByRadius(float radius, Vector3 position)
        {
            foreach (ObstacleController obstacle in _obstacles.ToList())
            {
                if (obstacle == null) continue;


                Vector2 closestPoint = obstacle.CapsuleCollider.ClosestPoint(position);

                float distance = Vector2.Distance(position, closestPoint);

                if (distance <= radius)
                {
                    Remove(obstacle);
                }
            }
        }
    }
}
