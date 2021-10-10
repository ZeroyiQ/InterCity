using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace InterCity
{
    public class OperationUIObject : ObjectBase
    {
        public static OperationUIObject Create(object target)
        {
            OperationUIObject rotationUIObject = ReferencePool.Acquire<OperationUIObject>();
            rotationUIObject.Initialize(target);
            return rotationUIObject;
        }

        protected override void Release(bool isShutdown)
        {
            OperationUI rotationUI = (OperationUI)Target;
            if (rotationUI == null)
            {
                return;
            }

            Object.Destroy(rotationUI.gameObject);
        }
    }
}
