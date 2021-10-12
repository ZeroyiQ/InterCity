using UnityEngine;

namespace InterCity
{
    public enum PlayerAnimatorState
    {
        Run,
        Attack,
        Fall,
        Hold
    }

    public class Player : Entity
    {
        public float Speed
        {
            get => m_Data.Speed;
        }

        public float HP;

        public float Power;

        public float Score;
        public PlayerAnimatorState State;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Data = userData as PlayerData;
            m_animator = GetComponent<Animator>();
            Reset();
        }

        public void Reset()
        {
            HP = m_Data.Hp;
            Power = 0;
            Score = 0;
            PlayRun();
        }

        #region 能力

        public void PlayRun()
        {
            PlayAnimator(STATE_RUN);
            SetAnimatorSpeed(Speed / 20f);
        }

        public void PlayAttack()
        {
            TriggerAnimator(STR_ATTACK);
        }

        public void PlayFall()
        {
            TriggerAnimator(STR_FALL);
        }

        public void PlayHold()
        {
            ChangeAnimator(STR_HOLD, true);
        }

        public void StopHold()
        {
            ChangeAnimator(STR_HOLD, false);
        }

        #endregion 能力

        #region private
        private void SetAnimatorSpeed(float speed)
        {
            if (m_animator != null)
            {
                m_animator.SetFloat("Speed", speed);
            }
        }

        private void PlayAnimator(string name)
        {
            if (m_animator != null)
            {
                m_animator.Play(name);
            }
        }

        private void TriggerAnimator(string name)
        {
            if (m_animator != null)
            {
                m_animator.ResetTrigger(name);
                m_animator.SetTrigger(name);
            }
        }

        private void ChangeAnimator(string name, bool value)
        {
            if (m_animator != null)
            {
                m_animator.SetBool(name, value);
            }
        }

        #endregion private

        [SerializeField]
        private PlayerData m_Data;

        private Animator m_animator;

        private readonly string STATE_RUN = "run";
        private readonly string STR_ATTACK = "Attack";
        private readonly string STR_FALL = "Fall";
        private readonly string STR_HOLD = "HoldOn";
    }
}