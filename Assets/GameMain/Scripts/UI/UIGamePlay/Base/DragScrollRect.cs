using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InterCity
{
    public class DragScrollRect : ScrollRect, IPointerExitHandler
    {
        private static DragScrollRect m_Instance;
        public static DragScrollRect Instance { get => m_Instance; }
        public int NowObj;
        private bool isExit = false;

        protected override void Awake()
        {
            base.Awake();
            if (!m_Instance)
            {
                m_Instance = this;
            }
        }

        public void TryCreateBuilder(BuilderType builder, Vector3 screenPosition)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
            var procedure = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
            procedure.CreateABuilder(builder, worldPos);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!isExit)
            {
                base.OnDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            isExit = false;
            base.OnBeginDrag(eventData);
        }

        /// <summary>
        /// 是否离开UI
        /// </summary>
        /// <returns></returns>
        private bool ExitUI()
        {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return true;
#else
            if (!EventSystem.current.IsPointerOverGameObject())
                return true;
#endif
            else
                return false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }
    }
}