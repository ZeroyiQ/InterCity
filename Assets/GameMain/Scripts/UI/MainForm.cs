using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace InterCity
{
    public class MainForm : UGuiForm
    {
        [SerializeField]
        private GameObject m_Close = null;

        [SerializeField]
        private GameObject m_Show = null;

        [SerializeField]
        private GameObject m_Build = null;

        [SerializeField]
        private Text m_ScoreText = null;

        [SerializeField]
        private Text m_EndPointText = null;

        [SerializeField]
        private Text m_ScoreTips1 = null;

        [SerializeField]
        private Text m_ScoreTips2 = null;

        private GameMode m_Mode;
        private ProcedureMain m_Procedure;
        private int m_ShowTips = 0;

        public void SetScoreText(float score)
        {
            m_ScoreText.text = GameEntry.Localization.GetString("Main.Score", score.ToString("F2"));
        }

        public void SetMode(GameMode mode)
        {
            m_Close.SetActive(true);
            RecycleTextVisual(false);
            switch (mode)
            {
                case GameMode.Edit:
                    m_Build.SetActive(true);
                    m_Show.SetActive(false);
                    break;

                case GameMode.Play:
                    m_Show.SetActive(true);
                    m_Build.SetActive(false);
                    break;
            }
        }

        #region Life Cycle

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)

        {
            base.OnOpen(userData);

            m_Procedure = (ProcedureMain)userData;
            if (m_Procedure == null)
            {
                Log.Warning("ProcedureMain is invalid when open MenuForm.");
                return;
            }
            m_ShowTips = 0;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            ResetTips();
        }

        #endregion Life Cycle

        //public void OnClickStart()
        //{
        //    if (m_Procedure != null)
        //    {
        //        m_Procedure.SetGameMode(GameMode.Play);
        //    }
        //}

        //public void OnClickBackToBuild()
        //{
        //    if (m_Procedure != null)
        //    {
        //        m_Procedure.SetGameMode(GameMode.Edit);
        //    }
        //}

        public void OnClickGoMenu()
        {
            if (m_Procedure != null)
            {
                m_Procedure.GotoMenu(true);
            }
        }

        public void AddtionScore(string score)
        {
            m_ShowTips++;
            if (m_ShowTips > 2)
            {
                m_ShowTips = 1;
                ResetTips();
            }
            StartCoroutine(ShowTips(score));
        }

        private IEnumerator ShowTips(string score)
        {
            Text tip = m_ShowTips == 1 ? m_ScoreTips1 : m_ScoreTips2;
            tip.text = "+" + score;
            tip.gameObject.SetActive(true);
            tip.rectTransform.localScale = Vector3.one * 1.5f;
            tip.rectTransform.localPosition = Vector3.zero;
            tip.color = Color.white;

            float time = 0f;
            float duration = 1f;
            float originalValue = 1.5f;
            float tragetValue = 0.5f;
            while (time < duration)
            {
                time += Time.deltaTime;
                tip.rectTransform.localScale = Vector3.one * Mathf.Lerp(originalValue, tragetValue, time / duration);
                tip.color = new Color(1, 1, 1, Mathf.Lerp(1f, 0.7f, time / duration));
                tip.rectTransform.localPosition = new Vector3(0, Mathf.Lerp(0, 80f, time / duration), 0);
                yield return new WaitForEndOfFrame();
            }

            tip.rectTransform.localScale = Vector3.one * tragetValue;
            tip.color = new Color(1, 1, 1, 0.7f);
            tip.rectTransform.localPosition = new Vector3(0, 80f, 0);
            tip.gameObject.SetActive(false);
        }

        private void ResetTips()
        {
            StopAllCoroutines();
            m_ScoreTips1.gameObject.SetActive(false);
            m_ScoreTips2.gameObject.SetActive(false);
        }
        public void RecycleTextVisual(bool enable)
        {
            m_EndPointText.gameObject.SetActive(enable);
        }
    }
}