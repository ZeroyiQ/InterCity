using UnityEngine;

namespace InterCity
{
    public class Player : Entity
    {
        [SerializeField]
        private PlayerData m_Data;

        private Rigidbody m_Rigidbody;

        private Vector3 m_PreVelocity = Vector3.zero;

        public float HP;

        public float Power;
        private Animator m_animator;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_animator = GetComponent<Animator>();
        }
    }
}