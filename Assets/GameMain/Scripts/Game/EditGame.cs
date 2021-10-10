//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace InterCity
{
    public class EditGame : GameBase
    {
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Edit;
            }
        }

        public override void Initialize(BinBall ball)
        {
            base.Initialize(ball);
            if (ball != null)
            {
                ball.Reset();
                ball.PauseBall();
            }
            else
            {
                Log.Error("实例空");
            }
            currentDragEntity = 0;
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        }

        public int currentDragEntity;
        private MoveObject builder;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            if (builder != null)
            {
                if (CheckUserInput())
                {
                    builder.OnMouseDrag();
                }
                else
                {
                    builder.OnMouseUp();
                    currentDragEntity = 0;
                    builder = null;
                }
            }
        }

        public void CreateABuilder(BuilderType builder, Vector3 worldPos)
        {
            if (currentDragEntity == 0)
            {
                currentDragEntity = GameEntry.Entity.GenerateSerialId();
                switch (builder)
                {
                    case BuilderType.Cube:
                        GameEntry.Entity.ShowBuildCube(new BuilderCubeData(currentDragEntity, 70004)
                        {
                            Position = new Vector3(worldPos.x, worldPos.y, 0),
                            LocalScale = new Vector3(5, .5f, 1)
                        });
                        break;

                    case BuilderType.BombBoard:
                        break;
                }
            }
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (builder != null || ne.Entity.Id != currentDragEntity)
            {
                return;
            }
            builder = (MoveObject)ne.Entity.Logic;
            builder.OnMouseDown();
        }

        /// <summary>
        ///检测用户当前输入
        /// </summary>
        /// <returns></returns>
        private bool CheckUserInput()
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved;
#else
            return Input.GetMouseButton(0);
#endif
        }
    }
}