using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace InterCity
{
    public class OperationUIComponent : GameFrameworkComponent
    {
        [SerializeField]
        private RotationUI m_RotationOpUI;

        [SerializeField]
        private DestoryUI m_DestoryOpUI;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 4;

        private IObjectPool<OperationUIObject> m_OperationPool = null;
        private List<OperationUI> m_ActiveOperation = null;
        private Canvas m_CachedCanvas = null;

        public RotationUI ShowRotationUI(Entity entity)
        {
            RotationUI ui = null;
            if (!TryGetActiveOperation(entity, ref ui))
            {
                ui = CreateRotationUI(entity);
                m_ActiveOperation.Add(ui);
            }
            ui.Init(entity, m_CachedCanvas);
            return ui;
        }

        public DestoryUI ShowDestoryUI(Entity entity)
        {
            DestoryUI ui = null;
            if (!TryGetActiveOperation(entity, ref ui))
            {
                ui = CreateDestoryUI(entity);
                m_ActiveOperation.Add(ui);
            }
            ui.Init(entity, m_CachedCanvas);
            return ui;
        }

        public void HideOperationUI(OperationUI ui)
        {
            ui.Reset();
            m_ActiveOperation.Remove(ui);
            m_OperationPool.Unspawn(ui);
        }

        #region cycle life

        protected override void Awake()
        {
            base.Awake();
            m_CachedCanvas = m_InstanceRoot.GetComponent<Canvas>();
            m_ActiveOperation = new List<OperationUI>();
        }

        private void Start()
        {
            if (m_OperationPool == null)
            {
                m_OperationPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<OperationUIObject>("OperationUI", m_InstancePoolCapacity);
            }
        }

        private void Update()
        {
            for (int i = m_ActiveOperation.Count - 1; i >= 0; i--)
            {
                OperationUI ui = m_ActiveOperation[i];
                if (ui.Refresh())
                {
                    continue;
                }

                HideOperationUI(ui);
            }
        }

        #endregion cycle life

        #region private



        private bool TryGetActiveOperation<T>(Entity entity, ref T operation) where T : OperationUI
        {
            if (entity == null)
            {
                return false;
            }

            for (int i = 0; i < m_ActiveOperation.Count; i++)
            {
                if (m_ActiveOperation[i].Owner == entity && m_ActiveOperation[i].GetType() == typeof(T))
                {
                    operation = m_ActiveOperation[i] as T;
                    return true;
                }
            }

            return false;
        }

        private RotationUI CreateRotationUI(Entity entity)
        {
            RotationUI ui = null;
            OperationUIObject obj = m_OperationPool.Spawn();
            if (obj != null)
            {
                ui = obj.Target as RotationUI;
            }
            else
            {
                ui = Instantiate(m_RotationOpUI);
                Transform transform = ui.GetComponent<Transform>();
                transform.SetParent(m_InstanceRoot);
                transform.localScale = Vector3.one;
                m_OperationPool.Register(OperationUIObject.Create(ui), true);
            }
            return ui;
        }

        private DestoryUI CreateDestoryUI(Entity entity)
        {
            DestoryUI ui = null;
            OperationUIObject obj = m_OperationPool.Spawn();
            if (obj != null)
            {
                ui = obj.Target as DestoryUI;
            }
            else
            {
                ui = Instantiate(m_DestoryOpUI);
                Transform transform = ui.GetComponent<Transform>();
                transform.SetParent(m_InstanceRoot);
                transform.localScale = Vector3.one;
                m_OperationPool.Register(OperationUIObject.Create(ui), true);
            }
            return ui;
        }

        #endregion private
    }
}