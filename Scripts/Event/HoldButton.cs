using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SMoonUniversalAsset
{
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Button button;
        [SerializeField]
        private float holdOnTriggerInterval = .5f;
        [SerializeField]
        private float nextTriggerInterval = .1f;
        private bool isButtonHeld = false;
        private TimeChecker timeChecker;


        private void Awake()
        {
            timeChecker = new(nextTriggerInterval);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isButtonHeld = true;
            timeChecker.UpdateTime(holdOnTriggerInterval);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isButtonHeld = false;
        }

        private void Update()
        {
            if (button == null)
            {
                return;
            }
            if (!isButtonHeld)
            {
                return;
            }
            if (!timeChecker.IsDurationEnd())
            {
                return;
            }

            timeChecker.UpdateTime(nextTriggerInterval);
            button.onClick?.Invoke();
        }
    }

}