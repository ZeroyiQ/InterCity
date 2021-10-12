using UnityEngine;
using System.Collections.Generic;

namespace InterCity
{


    [RequireComponent(typeof(UnityEngine.UI.Text))]
    public class LanguageText : MonoBehaviour
    {
        [SerializeField]
        public FontType Font;

        [SerializeField]
        public string LanguateKey;

        public static Dictionary<FontType, Font> FontDic;

        public static string GetFontName(FontType type)
        {
            return type.ToString();
        }
    }
}