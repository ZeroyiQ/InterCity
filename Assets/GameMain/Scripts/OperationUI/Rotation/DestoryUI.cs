using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace BinBall
{
    public class DestoryUI : OperationUI
    {
        [SerializeField]
        private Button Destory;
        private void Awake()
        {
            Destory.onClick.AddListener(OnDestory);
            m_ShowOffset = new Vector2(150, 0);
        }

        public void OnDestory()
        {
            if (Owner != null && Owner.GetType().BaseType == typeof(MoveObject))
            {
                MoveObject obj = Owner as MoveObject;
                obj.DestoryThis();
            }
        }
    }
}
