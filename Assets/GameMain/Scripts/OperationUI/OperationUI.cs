using UnityEngine;
using UnityGameFramework.Runtime;

namespace InterCity
{
    public class OperationUI : MonoBehaviour
    {
        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private RectTransform CanchedTransform { get => m_CachedTransform ?? (m_CachedTransform = GetComponent<RectTransform>()); }
        private CanvasGroup m_CachedCanvasGroup = null;
        private CanvasGroup CachedCanvasGroup { get => m_CachedCanvasGroup ?? (m_CachedCanvasGroup = GetComponent<CanvasGroup>()); }

        private Entity m_Owner = null;
        private int m_OwnerId = 0;
        protected Vector2 m_ShowOffset = Vector2.zero;

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public void Init(Entity owner, Canvas parentCanvas)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            m_ParentCanvas = parentCanvas;

            gameObject.SetActive(true);

            CachedCanvasGroup.alpha = 1f;
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                m_Owner = owner;
                m_OwnerId = owner.Id;
            }

            Refresh();
        }

        public bool Refresh()
        {
            if (m_CachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }
            UpdateSelfPosition();
            return true;
        }

        protected virtual void UpdateSelfPosition()
        {
            if (m_Owner != null && Owner.Available && Owner.Id == m_OwnerId)
            {
                Vector3 worldPosition = m_Owner.CachedTransform.position + Vector3.forward;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    CanchedTransform.localPosition = position + m_ShowOffset;
                }
            }
        }
        public void Reset()
        {
            CachedCanvasGroup.alpha = 1f;
            m_Owner = null;
            gameObject.SetActive(false);
        }
    }
}