using UnityEngine;

namespace InterCity
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static OperationUIComponent OperationUI
        {
            get;
            private set;
        }
        public static FontComponent Font
        {
            get;
            private set;
        }
        public static LevelComponent Level
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            OperationUI = UnityGameFramework.Runtime.GameEntry.GetComponent<OperationUIComponent>();
            Font = UnityGameFramework.Runtime.GameEntry.GetComponent<FontComponent>();
            Level = UnityGameFramework.Runtime.GameEntry.GetComponent<LevelComponent>();
        }
    }
}
