using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace InterCity

{
    public enum FontType
    {
        MainFont,
        BitFont,
    }

    public class FontComponent : GameFrameworkComponent
    {
        private Dictionary<FontType, Font> FontDic = new Dictionary<FontType, Font>();

        public Font this[FontType type]
        {
            get
            {
                if (FontDic.TryGetValue(type, out Font font))
                {
                    return font;
                }
                Log.Error($"Get UnLoad Font Asset!({type.ToString()})");
                return null;
            }
        }

        public void SetFont(FontType type, Font font)
        {
            if (FontDic.TryGetValue(type, out Font findFont))
            {
                FontDic[type] = font;
            }
            else
            {
                FontDic.Add(type, font);
            }
        }
    }
}