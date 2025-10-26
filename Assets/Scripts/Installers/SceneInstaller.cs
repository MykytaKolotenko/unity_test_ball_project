using Castle;
using Configs;
using Game;
using Input;
using Obstacle;
using Path;
using Player;
using Projectile;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private PlayerView playerView;
        [SerializeField] private ProjectileView projectileView;
        [SerializeField] private PathView pathView;
        [SerializeField] private CastleView castleView;
        [SerializeField] private CircleGameConfig circleGameConfig;
        [SerializeField] private CircleAnimationConfig circleAnimationConfig;
        [SerializeField] private TouchInputHandler touchInputHandler;
        [SerializeField] private ObstacleManager obstacleManager;
        [SerializeField] private CircleGameController circleGameController;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIController uiController;

        public override void InstallBindings()
        {
            Container.Bind<PlayerView>().FromInstance(playerView).AsSingle();
            Container.Bind<PathView>().FromInstance(pathView).AsSingle();
            Container.Bind<CastleView>().FromInstance(castleView).AsSingle();
            Container.Bind<CircleGameConfig>().FromInstance(circleGameConfig).AsSingle();
            Container.Bind<CircleAnimationConfig>().FromInstance(circleAnimationConfig).AsSingle();
            Container.Bind<TouchInputHandler>().FromInstance(touchInputHandler).AsSingle();
            Container.Bind<ObstacleManager>().FromInstance(obstacleManager).AsSingle();
            Container.Bind<CircleGameController>().FromInstance(circleGameController).AsSingle();
            Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
            Container.Bind<UIController>().FromInstance(uiController).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();

            Container.BindFactory<ProjectileController, ProjectileViewFactory>().FromComponentInNewPrefab(projectileView);
        }
    }
}
