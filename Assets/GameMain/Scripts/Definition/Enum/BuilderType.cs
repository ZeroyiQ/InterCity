using UnityEngine;
using System;

namespace BinBall
{
    /// <summary>
    /// 关系类型。
    /// </summary>
    [Serializable]
    public enum BuilderType : byte
    {
        /// <summary>
        /// 立方体
        /// </summary>
        Cube,

        /// <summary>
        /// 弹板
        /// </summary>
        BombBoard,
    }
}
