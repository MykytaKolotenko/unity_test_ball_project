using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private float _appearDuration = 0.5f;

        public void SetLabel(bool isWinGame)
        {
            _label.text = isWinGame ? "Win" : "Lose";
        }

        public void ShowUi(bool animated)
        {
            gameObject.SetActive(true);

            if (!animated)
            {
                _canvasGroup.alpha = 1f;
                return;
            }

            _canvasGroup.DOFade(1f, _appearDuration);
        }

        public void HideUI(bool animated)
        {
            if (!animated)
            {
                _canvasGroup.alpha = 0f;
                gameObject.SetActive(false);
                return;
            }

            _canvasGroup.DOFade(0f, _appearDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
