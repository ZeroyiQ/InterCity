using GameFramework.DataTable;
using System;
using UnityEngine;

namespace InterCity
{
    [Serializable]
    public class PlayerData : EntityData
    {
        [SerializeField]
        private string m_Name = null;

        [SerializeField]
        private float m_Speed = 1;

        [SerializeField]
        private Vector3 m_StartPostion = Vector3.zero;

        public PlayerData(int entityId, int typeId) : base(entityId, typeId)
        {
            IDataTable<DRPlayer> dtPlayer = GameEntry.DataTable.GetDataTable<DRPlayer>();
            DRPlayer drBinball = dtPlayer.GetDataRow(typeId);
            if (drBinball == null)
            {
                return;
            }
            m_Speed = drBinball.Speed;
            m_StartPostion = drBinball.StartPostion;
        }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 获取速度
        /// </summary>
        /// <value></value>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }

        /// <summary>
        /// 获取起始位置
        /// </summary>
        public Vector3 StartPosition
        {
            get
            {
                return m_StartPostion;
            }
        }
    }
}