using UnityGameFramework.Runtime;

namespace InterCity
{
    /// <summary>
    /// 可移动的实体类。
    /// </summary>
    public class InteractionPoint : MoveObject
    {
        private InteractionPointData m_Data;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_Data = userData as InteractionPointData;
            if (m_Data == null)
            {
                Log.Error("Builder data is invalid.");
                return;
            }
        }
    }
}