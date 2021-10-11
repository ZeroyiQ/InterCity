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
    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }
        public bool GameOver
        {
            get;
            set;
        }
        public bool ModeChange
        {
            get;
            protected set;
        }
        protected Player m_MyPlayer;
        protected MainForm m_MainForm;

        public virtual void Initialize(Player player , MainForm form)
        {
            GameOver = false;
            m_MyPlayer = player;
            m_MainForm = form;
        }

        public virtual void Shutdown()
        {

        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}
