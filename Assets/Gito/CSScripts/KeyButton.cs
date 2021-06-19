using UnityEngine;
using UnityEngine.EventSystems;

namespace Niwatori
{
    public class KeyButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] private EInput input;

        private PlayerController _playerController;
        private PlayerController playerController
        {
            get { return _playerController == null ? _playerController = GetComponentInParent<PlayerController>() : _playerController; }
        }

        private bool isPushing = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            isPushing = true;
            playerController.OnUIButtonDown(input);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(isPushing){
                isPushing = false;
                playerController.OnUIButtonUp(input);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(isPushing){
                isPushing = false;
                playerController.OnUIButtonUp(input);
            }
        }
    }
}
