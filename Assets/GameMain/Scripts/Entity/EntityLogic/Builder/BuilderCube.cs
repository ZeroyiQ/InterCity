using UnityEngine;
using UnityGameFramework.Runtime;
namespace InterCity
{
    /// <summary>
    /// 可移动的实体类。
    /// </summary>
    public class BuilderCube : MoveObject
    {
        private BuilderCubeData m_Data;
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_Data = userData as BuilderCubeData;
            if (m_Data == null)
            {
                Log.Error("Builder data is invalid.");
                return;
            }
            transform.localScale = m_Data.LocalScale;
        }
    }
}