using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework.DataTable;

namespace BinBall
{
    [Serializable]
    public class BuilderCubeData : EntityData
    {
        [SerializeField]
        private Vector3 m_Scale = Vector3.one;
        public BuilderCubeData(int entityId, int typeId) : base(entityId, typeId)
        {
        }


        public Vector3 LocalScale
        {
            get
            {
                return m_Scale;
            }
            set
            {
                m_Scale = value;
            }
        }
    }
}