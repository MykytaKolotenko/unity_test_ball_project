using UnityEngine;
using Zenject;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private CircleGameController _circleGameController;
        [Inject] private UIController _uiController;

        private void OnEnable()
        {
            _circleGameController.OnGameOver += OnGameOver;
        }

        public void Start()
        {
            _circleGameController.Init();
            _uiController.HideUI(false);
        }

        private void OnGameOver(bool isWinGame)
        {
            _uiController.SetLabel(isWinGame);
            _uiController.ShowUi(true);
        }

        public void Restart()
        {
            _circleGameController.Restart();
            _uiController.HideUI(true);
        }

        public void OnDisable()
        {
            _circleGameController.OnGameOver -= OnGameOver;
        }
    }
}
