using UnityEngine;

namespace BinBall
{
    public static class TransformExtension
    {
        /// <summary>
        /// 增加相对旋转的 x 分量
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="deltaValue"></param>
        public static void AddLocalRoationX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localEulerAngles;
            v.x += deltaValue;
            transform.localEulerAngles = v;
        }

        /// <summary>
        /// 增加相对旋转的 x 分量
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="deltaValue"></param>
        public static void AddLocalRoationY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localEulerAngles;
            v.y += deltaValue;
            transform.localEulerAngles = v;
        }

        /// <summary>
        /// 增加相对旋转的 x 分量
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="deltaValue"></param>
        public static void AddLocalRoationZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localEulerAngles;
            v.z += deltaValue;
            transform.localEulerAngles = v;
        }

        public static Vector3 TransformScreentPointToWorld(Vector3 ScreenPoint, Transform target)
        {
            //1 得到物体在主相机的xx方向
            Vector3 dir = (target.position - Camera.main.transform.position);
            //2 计算投影 (计算单位向量上的法向量)
            Vector3 norVec = Vector3.Project(dir, Camera.main.transform.forward);
            //返回世界空间
            return Camera.main.ViewportToWorldPoint(new Vector3(ScreenPoint.x / Screen.width, ScreenPoint.y / Screen.height, norVec.magnitude));
        }
    }
}