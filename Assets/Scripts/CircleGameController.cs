using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Castle;
using Game.Input;
using Game.Path;
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

        private ProjectileView _projectileView;
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
            Destroy(_projectileView.gameObject);
            _projectileView = null;

            Init();
        }

        public void Init()
        {
            _model.SetRadius(_circleGameConfig.PlayerRadius);

            _playerView.Init(_circleGameConfig.PlayerRadius);
            _pathView.Init(_circleGameConfig.PlayerRadius * 2, EvaluatePathDistance(), _rotation);
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
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void CreateProjectile()
        {
            Vector2 offset = _direction.normalized * _model.Radius;
            Vector3 spawnPosition = _playerView.LocalPosition + new Vector3(offset.x, offset.y, 0);

            _projectileView = _projectileViewFactory.Create(projectileParent, spawnPosition, 0);

            TransferCircleSquare();
        }

        private async UniTask TransferCircleSquare()
        {
            _cts = new CancellationTokenSource();

            while (!_cts.IsCancellationRequested &&
                   _model.Radius > _circleGameConfig.MinimumPlayerRadius)
            {
                float playerCircleSquare = MathUtils.CalculateCircleArea(_model.Radius);
                float squareDelta = Math.Max(playerCircleSquare * _circleGameConfig.SquareReductionPercent * Time.deltaTime,
                    _circleGameConfig.MinSquareReduction * Time.deltaTime);
                float currentPlayerCircleSquare = playerCircleSquare - squareDelta;
                float playerRadius = MathUtils.GetRadiusFromArea(currentPlayerCircleSquare);

                float projectileCircleSquare = MathUtils.CalculateCircleArea(_projectileView.Radius);
                float currentProjectileSquare = projectileCircleSquare + squareDelta;
                float projectileRadius = MathUtils.GetRadiusFromArea(currentProjectileSquare);

                _model.SetRadius(playerRadius);
                _playerView.SetRadius(playerRadius);
                _pathView.SetWidth(playerRadius * 2);
                _projectileView.SetRadius(projectileRadius);

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private float EvaluatePathDistance()
        {
            return Vector3.Distance(_playerView.Position, _castleView.Position);
        }
    }
}
