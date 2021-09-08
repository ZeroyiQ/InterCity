using UnityEngine;
using UnityEngine.EventSystems;

namespace BinBall
{
    /// <summary>
    /// 可移动的实体类。
    /// </summary>
    public class MoveObject : Entity
    {
        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 offset;
        private bool m_OperationUIVisible;
        private RotationUI m_RotationUI;
        private DestoryUI m_DestoryUI;
        protected Vector3 TransformConstraint
        {
            set { transform.position = ConstraintUtility.GetPositionInLimitArea(value, ProcedureMain.LimitArea, .5f); }
        }

        public void ChangeRoataion(float value)
        {
            transform.AddLocalRoationZ(value);
        }

        public void DestoryThis()
        {
            GameEntry.Entity.HideEntity(this);
            HideAllUI();
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_OperationUIVisible = false;
        }

        public void OnMouseDown()
        {
            if (Verify())
            {
                StarDrag();
            }
        }

        public void OnMouseDrag()
        {
            if (Verify())
            {
                Drag();
                var procedure = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
                procedure.SetRecycleTextVisual(true);
            }
        }

        public void OnMouseUp()
        {
            TransformConstraint = transform.position;
            var procedure = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
            procedure.SetRecycleTextVisual(false);
        }

        private void OnMouseUpAsButton()
        {
            if (Verify() && ExitUI())
            {
                ChangeOperationUIVisible();
            }
        }

        private bool Verify()
        {
            var procedure = GameEntry.Procedure.CurrentProcedure;
            if (procedure.GetType() == typeof(ProcedureMain))
            {
                var procedureMain = procedure as ProcedureMain;
                return procedureMain.IsBuildMode();
            }
            return false;
        }

        private void StarDrag(Vector2? position = null)
        {
            startPos = TransformExtension.TransformScreentPointToWorld(Input.mousePosition, transform);
        }

        private void Drag()
        {
            endPos = TransformExtension.TransformScreentPointToWorld(Input.mousePosition, transform);
            //计算偏移量
            offset = endPos - startPos;
            //让cube移动
            transform.position += offset;
            //这一次拖拽的终点变成了下一次拖拽的起点
            startPos = endPos;
        }

        private void ChangeOperationUIVisible()
        {
            m_OperationUIVisible = !m_OperationUIVisible;
            if (m_OperationUIVisible)
            {
                m_RotationUI = GameEntry.OperationUI.ShowRotationUI(this);
            }
            else
            {
                HideAllUI();
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (!Verify() && m_OperationUIVisible)
            {
                HideAllUI();
            }
        }

        /// <summary>
        /// 是否离开UI
        /// </summary>
        /// <returns></returns>
        private bool ExitUI()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return true;
#else
            if (!EventSystem.current.IsPointerOverGameObject())
                return true;
#endif
            else
                return false;
        }

        private void HideAllUI()
        {
            if (m_RotationUI != null)
            {
                GameEntry.OperationUI.HideOperationUI(m_RotationUI);
                m_RotationUI = null;
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "EndPoint")
            {
                DestoryThis();
            }
        }

    }
}