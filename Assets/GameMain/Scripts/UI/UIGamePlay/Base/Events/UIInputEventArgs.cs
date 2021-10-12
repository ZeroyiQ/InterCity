using UnityEngine;
using GameFramework.Event;
using System.Collections;
using GameFramework;

namespace InterCity
{
    public sealed class UIInputEventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(UIInputEventArgs).GetHashCode();


        public UIInputEventArgs()
        {
            MoveDirection = 0;
        }

        /// <summary>
        /// 获取激活场景被改变事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        
        /// <summary>
        /// 移动方向 （上0，下1，左2，右3）
        /// </summary>
        public int MoveDirection
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建触摸输入事件
        /// </summary>
        /// <param name="moveDirection">移动方向</param>
        /// <returns></returns>
        public static UIInputEventArgs Create(int moveDirection)
        {
            UIInputEventArgs activeSceneChangedEventArgs = ReferencePool.Acquire<UIInputEventArgs>();
            activeSceneChangedEventArgs.MoveDirection  = moveDirection;
            return activeSceneChangedEventArgs;
        }

        public override void Clear()
        {
            MoveDirection = 0;
        }
    }
  
}
