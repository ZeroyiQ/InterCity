using UnityEngine;

namespace InterCity
{
    public static class ConstraintUtility
    {
        private const float LimitZ = 0f;

        public static Vector3 GetPositionInLimitArea(Vector3 position, Vector4 limitArea, float space)
        {
            float x = position.x;
            float minX = Mathf.Min(limitArea.x, limitArea.z);
            float maxX = Mathf.Max(limitArea.x, limitArea.z);
            if (x <= minX)
            {
                x = minX;
            }
            else if (x >= maxX)
            {
                x = maxX;
            }
            else
            {
                x = Mathf.RoundToInt((x - minX) / space) * space + minX;
            }
            float y = position.y;
            float minY = Mathf.Min(limitArea.y, limitArea.w);
            float maxY = Mathf.Max(limitArea.y, limitArea.w);
            if (y <= minY)
            {
                y = minY;
            }
            else if (y >= maxY)
            {
                y = maxY;
            }
            else
            {
                y = Mathf.RoundToInt((y - minY) / space) * space + minY;
            }
            return new Vector3(x, y, LimitZ);
        }
    }
}