//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

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

        protected Player m_MyPlayer;
        protected MainForm m_MainForm;
        protected LevelGetter m_Getter;
        public LevelGetter Getter { get => m_Getter ?? (m_Getter = GameObject.Find("Level[Main]").GetComponent<LevelGetter>()); }

        public virtual void Initialize(Player player, MainForm form)
        {
            GameOver = false;
            m_MyPlayer = player;
            m_MainForm = form;
            player.gameObject.transform.SetParent(Getter.PlayerRoot, false);
            Utilities.SetObjActive(player.gameObject, true);
            OnInit();
        }

        public virtual void OnInit()
        {
        }

        public virtual void Shutdown()
        {
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}