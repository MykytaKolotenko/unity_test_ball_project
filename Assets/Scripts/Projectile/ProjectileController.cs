using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using Obstacle;
using UnityEngine;
using Zenject;

namespace Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private ProjectileView projectileView;
        [SerializeField] private CircleCollider2D circleCollider;

        [Inject] private ObstacleManager _obstacleManager;
        [Inject] private CircleGameConfig _circleGameConfig;

        private bool canMove = true;
        private CancellationTokenSource _cts;

        public event Action<Vector2> OnObstacleHit;
        public event Action OnProjectileWayEnd;
        public float Radius => projectileView.Radius;

        public void Init(float radius)
        {
            projectileView.Init(radius);
        }

        public void SetRadius(float radius)
        {
            projectileView.SetRadius(radius);
            circleCollider.radius = radius;
        }

        public async UniTask Move(Vector3 direction, float maxDistance)
        {
            float distanceTraveled = 0f;

            _cts = new CancellationTokenSource();

            while (!_cts.IsCancellationRequested)
            {
                float stepDistance = _circleGameConfig.ProjectileSpeed * Time.deltaTime;

                transform.Translate(direction.normalized * stepDistance, Space.World);

                distanceTraveled += stepDistance;
                await UniTask.Yield(PlayerLoopTiming.Update);

                if (distanceTraveled >= maxDistance)
                {
                    CancelToken();
                    OnProjectileWayEnd?.Invoke();
                    break;
                }
            }
        }

        private void CancelToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public void RemoveProjectile()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CancelToken();
            OnObstacleHit?.Invoke(other.transform.position);
        }
    }
}
