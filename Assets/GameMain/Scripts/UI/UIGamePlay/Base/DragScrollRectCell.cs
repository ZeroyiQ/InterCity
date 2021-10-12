using UnityEngine;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
namespace InterCity
{

    public class DragScrollRectCell : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private BuilderType m_Builder;
        private bool tryToCreate = false;



        public void OnPointerDown(PointerEventData eventData)
        {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            DragScrollRect.Instance.TryCreateBuilder(m_Builder, Input.GetTouch(0).position);
#endif
            DragScrollRect.Instance.TryCreateBuilder(m_Builder, Input.mousePosition);
        }
    }
}