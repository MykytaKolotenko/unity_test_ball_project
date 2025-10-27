using System;
using System.Threading;
using Castle;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private LayerMask collisionLayerMask;

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
        private Sequence _playerMoveSequence;

        public event Action<bool> OnGameOver;

        private void Awake()
        {
            _direction = _castleView.LocalPosition - _playerView.LocalPosition;
            _rotation = MathUtils.GetAngle(_direction);
        }

        public void Restart()
        {
            if (_projectileController != null)
            {
                Destroy(_projectileController.gameObject);
                _projectileController = null;
            }

            _obstacleManager.Reinit();

            Init();
        }

        public void Init()
        {
            _model.SetRadius(_circleGameConfig.PlayerRadius);

            _playerView.Init(_circleGameConfig.PlayerRadius);
            _pathView.Init(_circleGameConfig.PlayerRadius * 2, EvaluatePathDistance(_playerView.LocalPosition), _rotation);
            _castleView.Init();

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

            _playerMoveSequence.Kill();
            _playerMoveSequence = null;
        }

        private void SubscribeToProjectile(ProjectileController projectile)
        {
            projectile.OnObstacleHit += OnObstacleHit;
            projectile.OnProjectileWayEnd += OnProjectileDestroyed;
        }

        private void UnsubscribeToProjectile(ProjectileController projectile)
        {
            projectile.OnObstacleHit -= OnObstacleHit;
            projectile.OnProjectileWayEnd -= OnProjectileDestroyed;
        }

        private void ShootProjectile()
        {
            if (_projectileController == null) return;

            _inputHandler.IsInputEnabled = false;
            ClearToken();

            _projectileController.Move(_direction, EvaluatePathDistance(_playerView.LocalPosition)).Forget();
        }

        private void CreateProjectile()
        {
            Vector2 offset = _direction.normalized * _model.Radius;
            Vector3 spawnPosition = _playerView.LocalPosition + new Vector3(offset.x, offset.y, 0);

            _projectileController = _projectileViewFactory.Create(projectileParent, spawnPosition, 0);
            SubscribeToProjectile(_projectileController);

            TransferCircleSquare().Forget();
        }

        private void OnObstacleHit(Vector2 obstaclePos)
        {
            UnsubscribeToProjectile(_projectileController);

            AsyncOnObstacleHit(obstaclePos).Forget();
        }

        private async UniTask AsyncOnObstacleHit(Vector2 obstaclePos)
        {
            _obstacleManager.DestroyObstaclesByRadius(_projectileController.Radius, obstaclePos);

            _projectileController.RemoveProjectile();
            _projectileController = null;

            Vector2 nearestColliderPos =
                ColliderUtils.GetClosestPointOnRectTransform(_pathView.PathRectTransform, _direction, collisionLayerMask, _playerView.LocalPosition);

            float distance = Vector3.Distance(_playerView.LocalPosition, nearestColliderPos) - _model.Radius * _circleGameConfig.StopPositionMultiplier;
            Vector2 distancePos = distance * _direction.normalized;
            Vector3 pos = _playerView.LocalPosition + new Vector3(distancePos.x, distancePos.y, 0);

            float dis = Vector3.Distance(pos, _playerView.LocalPosition);

            if (dis > _circleGameConfig.MovePositionThreshold &&
                pos != Vector3.zero)
            {
                CreateMoveSequence(pos);
                await _playerMoveSequence.AsyncWaitForCompletion();
            }

            float pathDistance = EvaluatePathDistance(_playerView.LocalPosition);

            if (!_castleView.IsDoorOpen &&
                pathDistance < _circleGameConfig.DoorOpenDistance)
            {
                _castleView.OpenDoor();
            }

            await TryEndGame();
        }

        private async UniTask TryEndGame()
        {
            bool isWinGame = !ColliderUtils.IsRectTransformCollidingWithObstacles(_pathView.PathRectTransform, _direction, collisionLayerMask);

            if (isWinGame)
            {
                await MoveToDestination();
                OnGameOver?.Invoke(true);
                return;
            }

            if (_model.Radius < _circleGameConfig.MinimumPlayerRadius)
            {
                OnGameOver?.Invoke(false);
                return;
            }

            _inputHandler.IsInputEnabled = true;
        }

        private void OnProjectileDestroyed()
        {
            _projectileController.RemoveProjectile();
            _inputHandler.IsInputEnabled = true;
        }

        private async UniTask TransferCircleSquare()
        {
            _cts = new CancellationTokenSource();

            while (_cts is { IsCancellationRequested: false })
            {
                if (_model.Radius >= _circleGameConfig.MinimumPlayerRadius)
                {
                    (float playerRadius, float projectileRadius) = MathUtils.EvaluatePlayerAndProjectileRadius(
                        _model.Radius,
                        _projectileController.Radius,
                        _circleGameConfig.SquareReductionPercent,
                        _circleGameConfig.MinSquareReduction
                    );

                    _model.SetRadius(playerRadius);
                    _playerView.SetRadius(playerRadius);
                    _pathView.SetWidth(playerRadius * 2);
                    _projectileController.SetRadius(projectileRadius);
                }
                else
                {
                    ShootProjectile();
                    return;
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        public async UniTask MoveToDestination()
        {
            _inputHandler.IsInputEnabled = false;
            CreateMoveSequence(_castleView.LocalPosition, false);
            await _playerMoveSequence.AsyncWaitForCompletion();
        }

        private void CreateMoveSequence(Vector3 pos, bool hasPathView = true)
        {
            _playerMoveSequence = DOTween.Sequence();

            float pathHeight = hasPathView ? EvaluatePathDistance(pos) : 0f;

            _playerMoveSequence.Append(_playerView.MoveTo(pos));
            _playerMoveSequence.Join(_pathView.SetHeightAnimated(pathHeight));
            _playerMoveSequence.Join(_pathView.SetPositionAnimated(pos));
        }

        private float EvaluatePathDistance(Vector2 playerLocalPos)
        {
            return Vector3.Distance(playerLocalPos, _castleView.LocalPosition);
        }

        private void ClearToken()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
