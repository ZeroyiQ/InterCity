using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace InterCity
{
    public class DialogForm : UGuiForm
    {
        [SerializeField]
        private Text m_TitleText = null;

        [SerializeField]
        private Text m_MessageText = null;

        [SerializeField]
        private GameObject[] m_ModeObjects = null;

        [SerializeField]
        private Text[] m_ConfirmTexts = null;

        [SerializeField]
        private Text[] m_CancelTexts = null;

        [SerializeField]
        private Text[] m_OtherTexts = null;

        private int m_DialogMode = 1;
        private bool m_PauseGame = false;
        private object m_UserData = null;
        private GameFrameworkAction<object> m_OnClickConfirm = null;
        private GameFrameworkAction<object> m_OnClickCancel = null;
        private GameFrameworkAction<object> m_OnClickOther = null;

        public int DialogMode
        {
            get
            {
                return m_DialogMode;
            }
        }

        public bool PauseGame
        {
            get
            {
                return m_PauseGame;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void OnConfirmButtonClick()
        {
            Close(true);

            if (m_OnClickConfirm != null)
            {
                m_OnClickConfirm(m_UserData);
                m_OnClickConfirm = null;
            }
        }

        public void OnCancelButtonClick()
        {
            Close();

            if (m_OnClickCancel != null)
            {
                m_OnClickCancel(m_UserData);
                m_OnClickCancel = null;
            }
            Close(true);
        }

        public void OnOtherButtonClick()
        {
            if (m_OnClickOther != null)
            {
                m_OnClickOther(m_UserData);
                m_OnClickOther = null;
            }
            Close();
        }

        protected override void OnOpen(object userData)

        {
            base.OnOpen(userData);

            DialogParams dialogParams = (DialogParams)userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            m_DialogMode = dialogParams.Mode;
            RefreshDialogMode();

            m_TitleText.text = dialogParams.Title;
            m_MessageText.text = dialogParams.Message;

            m_PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();

            m_UserData = dialogParams.UserData;

            RefreshConfirmText(dialogParams.ConfirmText);
            m_OnClickConfirm = dialogParams.OnClickConfirm;

            RefreshCancelText(dialogParams.CancelText);
            m_OnClickCancel = dialogParams.OnClickCancel;

            RefreshOtherText(dialogParams.OtherText);
            m_OnClickOther = dialogParams.OnClickOther;
        }

        protected override void OnClose(bool isShutdown, object userData)

        {
            if (m_PauseGame)
            {
                GameEntry.Base.ResumeGame();
            }

            m_DialogMode = 1;
            m_TitleText.text = string.Empty;
            m_MessageText.text = string.Empty;
            m_PauseGame = false;
            m_UserData = null;

            RefreshConfirmText(string.Empty);
            RefreshCancelText(string.Empty);
            RefreshOtherText(string.Empty);
            base.OnClose(isShutdown, userData);
        }

        private void RefreshDialogMode()
        {
            for (int i = 1; i <= m_ModeObjects.Length; i++)
            {
                m_ModeObjects[i - 1].SetActive(i == m_DialogMode);
            }
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameEntry.Base.PauseGame();
            }
        }

        private void RefreshConfirmText(string confirmText)
        {
            if (string.IsNullOrEmpty(confirmText))
            {
                confirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton");
            }

            for (int i = 0; i < m_ConfirmTexts.Length; i++)
            {
                m_ConfirmTexts[i].text = confirmText;
            }
        }

        private void RefreshCancelText(string cancelText)
        {
            if (string.IsNullOrEmpty(cancelText))
            {
                cancelText = GameEntry.Localization.GetString("Dialog.CancelButton");
            }

            for (int i = 0; i < m_CancelTexts.Length; i++)
            {
                m_CancelTexts[i].text = cancelText;
            }
        }

        private void RefreshOtherText(string otherText)
        {
            if (string.IsNullOrEmpty(otherText))
            {
                otherText = GameEntry.Localization.GetString("Dialog.OtherButton");
            }

            for (int i = 0; i < m_OtherTexts.Length; i++)
            {
                m_OtherTexts[i].text = otherText;
            }
        }
    }
}