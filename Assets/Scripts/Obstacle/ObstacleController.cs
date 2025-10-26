using UnityEngine;
using Zenject;

namespace Obstacle
{
    public class ObstacleController : MonoBehaviour
    {
        [field: SerializeField] public CapsuleCollider2D CapsuleCollider { get; private set; }
        [Inject] private ObstacleManager _obstacleManager;

        private void Awake()
        {
            _obstacleManager.Add(this);
        }
    }
}
