using System;
using System.Threading;
using Castle;
using Configs;
using Cysharp.Threading.Tasks;
using Input;
using Obstacle;
using Path;
using Player;
using Projectile;
using UnityEngine;
using Utils;
using Zenject;

namespace Game
{
    public class CircleGameController : MonoBehaviour
    {
        [SerializeField] private RectTransform projectileParent;
        [Inject] private PlayerModel _model;
        [Inject] private CircleGameConfig _circleGameConfig;

        [Inject] private PlayerView _playerView;
        [Inject] private PathView _pathView;
        [Inject] private CastleView _castleView;

        [Inject] private ProjectileViewFactory _projectileViewFactory;
        [Inject] private TouchInputHandler _inputHandler;

        [Inject] private ObstacleManager _obstacleManager;

        private ProjectileController _projectileController;
        private CancellationTokenSource _cts;
        private Quaternion _rotation;
        private Vector2 _direction;

        private void Awake()
        {
            _direction = _castleView.Position - _playerView.Position;
            _rotation = MathUtils.GetAngle(_direction);
        }

        private void Start()
        {
            Init();
        }

        public void Restart()
        {
            Destroy(_projectileController.gameObject);
            _projectileController = null;

            Init();
        }

        public void Init()
        {
            _model.SetRadius(_circleGameConfig.PlayerRadius);

            _playerView.Init(_circleGameConfig.PlayerRadius);
            _pathView.Init(_circleGameConfig.PlayerRadius * 2, EvaluatePathDistance(), _rotation);

            _inputHandler.IsInputEnabled = true;
        }

        private void OnEnable()
        {
            _inputHandler.OnTapStarted += CreateProjectile;
            _inputHandler.OnTapEnded += ShootProjectile;
        }

        private void OnDisable()
        {
            _inputHandler.OnTapStarted -= CreateProjectile;
            _inputHandler.OnTapEnded -= ShootProjectile;
        }

        private void ShootProjectile()
        {
            try
            {
                _cts?.Cancel();
                _cts?.Dispose();

                _projectileController.Move(_direction, EvaluatePathDistance()).Forget();
            }
            catch (Exception e)
            {
                // over clicking
            }
        }

        private void CreateProjectile()
        {
            Vector2 offset = _direction.normalized * _model.Radius;
            Vector3 spawnPosition = _playerView.LocalPosition + new Vector3(offset.x, offset.y, 0);

            _projectileController = _projectileViewFactory.Create(projectileParent, spawnPosition, 0);
            SubscribeToProjectile(_projectileController);

            TransferCircleSquare();
        }

        private void SubscribeToProjectile(ProjectileController projectile)
        {
            projectile.OnObstacleHit += OnObstacleHit;
            projectile.OnProjectileWayEnd += OnProjectileDestroed;
        }

        private void UnsubscribeToProjectile(ProjectileController projectile)
        {
            projectile.OnObstacleHit -= OnObstacleHit;
            projectile.OnProjectileWayEnd -= OnProjectileDestroed;
        }

        private void OnObstacleHit(Vector2 obj)
        {
            UnsubscribeToProjectile(_projectileController);

            _obstacleManager.DestroyObstaclesByRadius(_projectileController.Radius, obj);

            _projectileController.RemoveProjectile();
            _projectileController = null;

            _inputHandler.IsInputEnabled = true;
        }

        private void OnProjectileDestroed()
        {
            _projectileController.RemoveProjectile();
            _inputHandler.IsInputEnabled = true;
        }

        private async UniTask TransferCircleSquare()
        {
            _cts = new CancellationTokenSource();

            while (!_cts.IsCancellationRequested &&
                   _model.Radius > _circleGameConfig.MinimumPlayerRadius)
            {
                (float playerRadius, float projectileRadius) = EvaluatePlayerAndProjectileRadius();

                _model.SetRadius(playerRadius);
                _playerView.SetRadius(playerRadius);
                _pathView.SetWidth(playerRadius * 2);
                _projectileController.SetRadius(projectileRadius);

                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            _inputHandler.IsInputEnabled = false;
        }

        private (float playerRadius, float projectileRadius) EvaluatePlayerAndProjectileRadius()
        {
            float playerCircleSquare = MathUtils.CalculateCircleArea(_model.Radius);
            float squareDelta = Math.Max(playerCircleSquare * _circleGameConfig.SquareReductionPercent * Time.deltaTime,
                _circleGameConfig.MinSquareReduction * Time.deltaTime);
            float currentPlayerCircleSquare = playerCircleSquare - squareDelta;
            float currentPlayerRadius = MathUtils.GetRadiusFromArea(currentPlayerCircleSquare);

            float projectileCircleSquare = MathUtils.CalculateCircleArea(_projectileController.Radius);
            float currentProjectileSquare = projectileCircleSquare + squareDelta;
            float currentProjectileRadius = MathUtils.GetRadiusFromArea(currentProjectileSquare);

            return (currentPlayerRadius, currentProjectileRadius);
        }

        private float EvaluatePathDistance()
        {
            return Vector3.Distance(_playerView.Position, _castleView.Position);
        }
    }
}
