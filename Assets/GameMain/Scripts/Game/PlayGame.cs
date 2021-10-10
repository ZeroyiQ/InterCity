//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

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
                if (m_MyBall != null)
                {
                    realScore += m_MyBall.Score;
                }
                return realScore;
            }
        }
        private float m_TimeScore;

        public override void Initialize(BinBall ball)
        {
            base.Initialize(ball);
            m_TimeScore = 0;
            if (ball != null)
            {
                ball.ResumeBall();
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
            m_TimeScore += realElapseSeconds * 0.5f;
            if (m_MyBall != null)
            {
                if (!m_MyBall.Visible)
                {
                    GameOver = true;
                }
            }
        }
    }
}
