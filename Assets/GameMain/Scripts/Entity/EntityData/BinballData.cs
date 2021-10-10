using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework.DataTable;

namespace InterCity
{
    [Serializable]
    public class BinballData : EntityData
    {
        [SerializeField]
        private string m_Name = null;
        [SerializeField]
        private float m_Mass = 1;
        [SerializeField]
        private float m_Drag = 0;
        [SerializeField]
        private float m_Friction = 0.2f;
        [SerializeField]
        private float m_Bounciness = 0.5f;
        [SerializeField]
        private Vector3 m_StartPostion = Vector3.zero;
        public BinballData(int entityId, int typeId) : base(entityId, typeId)
        {
            IDataTable<DRBinball> dtBinball = GameEntry.DataTable.GetDataTable<DRBinball>();
            DRBinball drBinball = dtBinball.GetDataRow(typeId);
            if (drBinball == null)
            {
                return;
            }
            m_Mass = drBinball.Mass;
            m_Drag = drBinball.Drag;
            m_Friction = drBinball.Friction;
            m_Bounciness = drBinball.Bounciness;
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
        /// 获取质量
        /// </summary>
        /// <value></value>
        public float Mass
        {
            get
            {
                return m_Mass;
            }
        }

        /// <summary>
        /// 获取阻力。
        /// </summary>
        public float Drag
        {
            get
            {
                return m_Drag;
            }
        }

        /// <summary>
        /// 获取摩擦力。
        /// </summary>
        public float Friction
        {
            get
            {
                return m_Friction;
            }
        }

        /// <summary>
        /// 获取弹力。
        /// </summary>
        public float Bounciness
        {
            get
            {
                return m_Bounciness;
            }
        }

        public Vector3 StartPosition
        {
            get
            {
                return m_StartPostion;
            }
        }
    }
}