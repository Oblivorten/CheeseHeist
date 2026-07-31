using UnityEngine;
using UnityEngine.EventSystems;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class VirtualJoystickInputAdapter : MonoBehaviour,
        IMoveInputProvider,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler
    {
        [SerializeField] private RectTransform _joystick;
        [SerializeField] private RectTransform _handle;

        private Vector2 _input;

        public float Horizontal => _input.x;
        public float Vertical => _input.y;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _joystick,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPosition
            );

            Vector2 radius = _joystick.rect.size / 2f;

            _input = localPosition / radius;

            _input = Vector2.ClampMagnitude(_input, 1f);

            _handle.anchoredPosition = new Vector2(
                _input.x * radius.x,
                _input.y * radius.y
            );
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;
            _handle.anchoredPosition = Vector2.zero;
        }
    }
}