
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using GameFramework.Event;

namespace InterCity
{
    public class PlayGame : GameBase
    {
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Play;
            }
        }
        public float Score
        {
            get
            {
                float realScore = m_TimeScore;
                if (m_MyPlayer != null)
                {
                    realScore += m_MyPlayer.Score;
                }
                return realScore;
            }
        }
        private float m_TimeScore;

        public override void OnInit()
        {
            base.OnInit();
            m_TimeScore = 0;
            GameEntry.Event.Subscribe(UIInputEventArgs.EventId, OnInput);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.Event.Unsubscribe(UIInputEventArgs.EventId, OnInput);
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            if (m_MyPlayer.HP <= 0)
            {
                GameOver = true;
            }
            if (!GameOver)
            {
                m_TimeScore += realElapseSeconds * 0.5f;
                Getter.PlayerRoot.transform.AddLocalPositionX(elapseSeconds * m_MyPlayer.Speed);
            }
        }

        #region Event
        private void OnInput(object sender, GameEventArgs e)
        {
            UIInputEventArgs inputEvent = e as UIInputEventArgs;
            if (GameOver)
            {
                return;
            }
            if (inputEvent.MoveDirection == 1)
            {
                m_MyPlayer.PlayAttack();
            }
        }
        #endregion
    }
}
