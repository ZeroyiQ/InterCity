//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace InterCity
{
    public enum MainLvState
    {
        Ready,      // 准备阶段
        Playing,    // 演出阶段
        Pause,      // 暂停
        Stop        // 停止
    }

    public class ProcedureMain : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private int m_ChangeScene = 0; // 1 ：重试 2：下一关
        private float m_GotoMenuDelaySeconds = 0f;

        private MainForm m_MainForm;
        private int m_Id;
        private Player m_Player;

        private MainLvState m_State;

        public MainLvState MyProperty
        {
            get { return m_State; }
        }

        private bool m_OpenDialog;

        public void GotoMenu(bool isImediate = false)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.GameOver = true;
            }
            m_ChangeScene = 1;
            m_CurrentGame = null;
            m_State = MainLvState.Stop;
        }

        public void Retry()
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.GameOver = true;
            }
            m_ChangeScene = 2;
            m_CurrentGame = null;
            m_State = MainLvState.Stop;
        }

        #region Life Cycle

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

#if GAME_EDITOR
            m_Games.Add(GameMode.Edit, new EditGame());
#endif
            m_Games.Add(GameMode.Play, new PlayGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMainUI);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            m_ChangeScene = 0;
            m_State = MainLvState.Ready;
            m_OpenDialog = false;
            PreLoad();
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMainUI);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);

            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }
            if (m_MainForm != null)
            {
                m_MainForm.Close(true);
                m_MainForm = null;
            }
            m_Player = null;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_State != MainLvState.Playing && m_State != MainLvState.Stop)
            {
                return;
            }
            if (m_State == MainLvState.Playing)
            {
                if (m_CurrentGame != null)
                {
                    if (m_CurrentGame.GameOver)
                    {
                        TryOpenDialog();
                    }
                    else
                    {
                        m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                    }
                }
                else
                {
                    m_State = MainLvState.Stop;
                }
            }
            else if (m_State == MainLvState.Stop)
            {
                if (m_ChangeScene == 1)
                {
                    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                }
                else if (m_ChangeScene == 2)
                {
                    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Level"));
                }
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        #endregion Life Cycle

        #region Event

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (m_Player != null || ne.Entity.Id != m_Id)
            {
                return;
            }
            m_Player = (Player)ne.Entity.Logic;
            Utilities.SetObjActive(m_Player.gameObject, false);
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
        }

        private void OnOpenMainUI(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_MainForm = (MainForm)ne.UIForm.Logic;
            SetGameMode(GameMode.Play);
        }

        private void OnEnterToNext(object userdata)
        {
            GotoMenu(true);
        }

        private void OnRetry(object userdata)
        {
            Retry();
        }

        #endregion Event

        #region Ready

        private void PreLoad()
        {
            // 加载玩家
            m_Id = GameEntry.Entity.GenerateSerialId();
            GameEntry.Entity.ShowPlayer(new PlayerData(m_Id, 10000) {
                Name = "Player1",
            });
        }

        #endregion Ready

        #region private

        private void TryOpenDialog()
        {
            if (!m_OpenDialog)
            {
                m_OpenDialog = true;
                GameEntry.UI.OpenDialog(new DialogParams
                {
                    Mode = 2,
                    Title = GameEntry.Localization.GetString("LevelResult.Title"),

                    Message = GameEntry.Localization.GetString("LevelResult.Message", ((PlayGame)m_CurrentGame).Score.ToString("F2")),
                    ConfirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton"),
                    OnClickConfirm = OnEnterToNext,
                    CancelText = GameEntry.Localization.GetString("LevelResult.Retry"),
                    OnClickCancel = OnRetry,
                });
            }
        }

        #endregion private

        #region public

        public bool IsBuildMode()
        {
            return m_CurrentGame != null && m_CurrentGame.GameMode == GameMode.Edit;
        }

        public bool IsShowMode()
        {
            return m_CurrentGame != null && m_CurrentGame.GameMode == GameMode.Play;
        }

        public void ShowAddScoreTip(string message)
        {
            if (IsShowMode() && m_MainForm != null)
            {
                m_MainForm.AddtionScore(message);
            }
        }

        public void SetGameMode(GameMode gameMode)
        {
            if (m_Games.TryGetValue(gameMode, out GameBase game))
            {
                if (m_CurrentGame != null)
                {
                    m_CurrentGame.Shutdown();
                }
                m_CurrentGame = game;
                m_CurrentGame.Initialize(m_Player, m_MainForm);
                m_MainForm.SetMode(m_CurrentGame.GameMode);
                m_State = MainLvState.Playing;
            }
        }

        public void SetRecycleTextVisual(bool enable)
        {
            m_MainForm.RecycleTextVisual(enable);
        }

        public void CreateABuilder(BuilderType builder, Vector3 worldPos)
        {
            if (m_CurrentGame != null && m_CurrentGame.GameMode == GameMode.Edit)
            {
                ((EditGame)m_CurrentGame).CreateABuilder(builder, worldPos);
            }
        }

        #endregion public
    }
}