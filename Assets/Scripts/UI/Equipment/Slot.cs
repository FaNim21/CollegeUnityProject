using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.UI.Equipment
{
    public abstract class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler
    {
        protected abstract void Start();

        public abstract void OnPointerClick(PointerEventData eventData);
        public abstract void OnBeginDrag(PointerEventData eventData);
    }
}