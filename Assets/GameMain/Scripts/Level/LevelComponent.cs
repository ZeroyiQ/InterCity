using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using GameFramework;
using UnityEngine;

namespace InterCity
{
    public class LevelComponent : GameFrameworkComponent
    {
        private List<LevelInfo> m_InfoList;

        public LevelInfo this[int id]
        {
            get
            {
                if (m_InfoList != null)
                {
                    for (int i = 0; i < m_InfoList.Count; i++)
                    {
                        if (m_InfoList[i].Id == id)
                        {
                            return m_InfoList[i];
                        }
                    }
                }
                Log.Error("[LevelComponent] 关卡配置获取失败!");
                return null;
            }
        }
        private string m_AssetPath;

        public void InitLevelInfo(string assetPath, Action success, Action<string> failure)
        {
            m_AssetPath = assetPath;
            GameEntry.Resource.LoadAsset(assetPath, Constant.AssetPriority.LevelConfig, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    m_InfoList = Utility.Json.ToObject<List<LevelInfo>>(((TextAsset)asset).text);
                    success.Invoke();
                },
                (assetName, status, errorMessage, userData) =>
                {
                    failure.Invoke(errorMessage);
                }));
        }

        public void UpdateLevelInfo()
        {

        }
    }
}