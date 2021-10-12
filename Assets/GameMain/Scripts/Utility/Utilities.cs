using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class Utilities
{
    public static Vector2 halfXfullY = new Vector2(0.5f, 1);
    public static Vector2 fullXhalfY = new Vector2(1, 0.5f);
    public readonly static string WarriorColor = "86c7ff";
    public readonly static string RangerColor = "8ae44c";
    public readonly static string MagicianColor = "d6aeff";
    public readonly static string MinisterColor = "ffea50";
    public readonly static string NinjaColor = "49eac7";
    public static uint ServerOpenTime_sce; //开服时间
    public static uint ServerMegreTime_sce; //合服时间
    private static ulong GetSystemTimeRunTime = 0;
    private static ulong SystemTime = 0;
    public static int TimeZoneOffset { get; private set; }
    private static ulong SystemTimeOffset = 0;
    private static int outlierCount = 0;

    public static Dictionary<int, Color> QualityColorDic = new Dictionary<int, Color>
    {
        {0, new Color(1, 1, 1, 1)}, //白色
        {1, new Color(1, 1, 1, 1)}, //白色
        {2, new Color(0.329f, 0.631f, 1, 1)}, //藍
        {3, new Color(0.804f, 0.349f, 1, 1)}, //紫
        {4, new Color(1, 0.517f, 0, 1)}, //橙
        {5, new Color(1, 0.31f, 0.31f, 1)}, //紅
        {6, new Color(0.403f, 0.93f, 0.4f, 1)}, //綠
    };

    public static Dictionary<int, Color> TextColorDic = new Dictionary<int, Color>
    {
        {5, new Color(1, 0.274f, 0.286f, 1)}, //紅
        {6, new Color(0.541f, 0.882f, 0.301f, 1)}, //綠
        {0, new Color(1, 1, 1, 1)}, //白色
        {3, new Color(0.686f, 0.584f, 0.870f, 1)}, //灰紫
        {8, new Color(0.647f, 0.647f, 0.647f, 1)}, //灰
        {7, new Color(1, 0.976f, 0.215f, 1)}, //黄
        {4, new Color(1, 0.517f, 0.007f, 1)}, //橙
        {2, new Color(0.250f, 0.796f, 0.858f, 1)}, //藍
        {9, new Color(0.721f, 0.761f, 0.953f, 1f)}, //灰紫2
    };

    public static void SetObjActive(GameObject go, bool val)
    {
        if (go == null)
        {
            Log.Error("SetObjActive obj == null");
            return;
        }

        if (go.activeSelf != val)
            go.SetActive(val);
    }

    public static void SetObjActive(Transform trans, bool val)
    {
        if (trans == null)
        {
            Log.Error("SetObjActive trans == null");
            return;
        }

        if (trans.gameObject.activeSelf != val)
            trans.gameObject.SetActive(val);
    }

    public static void SetComponentActive(Behaviour comp, bool val)
    {
        if (comp == null)
        {
            Log.Error("SetComponentActive component == null, name :" + comp.name);
            return;
        }

        if (comp.enabled != val)
            comp.enabled = val;
    }

    public static void ReArrangeTo2DArray<T>(List<T> src, int subLen, ref List<List<T>> result)
    {
        if (src == null)
            return;

        result = new List<List<T>>();
        for (int i = 0; i < src.Count; ++i)
        {
            int outterIdx = Mathf.FloorToInt(i / subLen);
            if (result.Count < outterIdx + 1)
            {
                for (int j = result.Count; j < outterIdx + 1; ++j)
                {
                    result.Add(new List<T>());
                }
            }

            List<T> innerList = result[outterIdx];
            if (i == outterIdx * subLen)
                innerList.Clear();
            innerList.Add(src[i]);
        }

        int resultLen = Mathf.CeilToInt(src.Count / (float)subLen);
        if (result.Count > resultLen)
            result.RemoveRange(resultLen, result.Count - resultLen);
    }

    public static RawImage CreateRawImg(Transform parent)
    {
        GameObject obj = new GameObject("Bg");
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.SetParent(parent, false);
        rect.SetAsFirstSibling();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        return obj.AddComponent<RawImage>();
    }

    public static Image CreateUIMask(Transform trans)
    {
        //UIMask
        GameObject UIMask = new GameObject("UIMask");
        UIMask.transform.SetParent(trans, false);
        UIMask.transform.SetAsFirstSibling();
        var maskRect = UIMask.AddComponent<RectTransform>();
        var img = maskRect.gameObject.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.8f);
        img.raycastTarget = false;
        SetUIAnchorStretch(maskRect);
        return img;
    }

    public static GameObject AddMask(GameObject rectTran)
    {
        var mask = rectTran.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        if (!mask.MaskEnabled())
        {
            var graphic = rectTran.AddComponent<Image>();
            graphic.raycastTarget = true;
        }

        return rectTran;
    }

    public static Mask AddMask(Component cpnt)
    {
        var mask = cpnt.gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        if (!mask.MaskEnabled())
        {
            var graphic = cpnt.gameObject.AddComponent<Image>();
            graphic.raycastTarget = true;
        }

        return mask;
    }

    public static RectMask2D AddRectMask2D(RectTransform rectTran)
    {
        var rectMask = rectTran.GetComponent<RectMask2D>();
        if (rectMask == null)
            rectMask = rectTran.gameObject.AddComponent<RectMask2D>();
        return rectMask;
    }

    public static LayoutElement CreateStandardVerticalLayoutElement(RectTransform trans)
    {
        LayoutElement elem = trans.gameObject.AddComponent<LayoutElement>();
        elem.preferredHeight = trans.sizeDelta.y;
        return elem;
    }

    public static LayoutElement CreateStandardHorizontalLayoutElement(RectTransform trans)
    {
        LayoutElement elem = trans.gameObject.AddComponent<LayoutElement>();
        elem.preferredWidth = trans.sizeDelta.x;
        return elem;
    }

    public static HorizontalLayoutGroup CreateStandardHorizontalLayout(
        GameObject obj,
        TextAnchor anchor = TextAnchor.MiddleCenter,
        int spacing = 0)
    {
        var layout = obj.AddComponent<HorizontalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = anchor;
        layout.spacing = spacing;
        return layout;
    }

    public static VerticalLayoutGroup CreateStandardVerticalLayout(
        GameObject obj,
        TextAnchor anchor = TextAnchor.MiddleCenter,
        int spacing = 0)
    {
        VerticalLayoutGroup layout = obj.AddComponent<VerticalLayoutGroup>();
        layout.childControlHeight = false;
        layout.childControlWidth = false;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;
        layout.childAlignment = anchor;
        layout.spacing = spacing;
        return layout;
    }

    public static RectTransform SetUIAnchorStretch(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.zero;
        rectTran.anchorMax = Vector2.one;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.pivot = Vector2.one / 2;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchBottomCenter(RectTransform rectTran)
    {
        float h = rectTran.sizeDelta.y;
        rectTran.anchorMin = Vector2.zero;
        rectTran.anchorMax = Vector2.right;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.left;
        rectTran.pivot = new Vector2(0.5f, 0f);
        rectTran.sizeDelta = new Vector2(0f, h);
        return rectTran;
    }

    public static RectTransform SetUIAnchorTopLeft(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.up;
        rectTran.anchorMax = Vector2.up;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;
        rectTran.pivot = Vector2.up;

        return rectTran;
    }

    public static RectTransform SetUIAnchorTopLeft1(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.up;
        rectTran.anchorMax = Vector2.up;
        rectTran.anchoredPosition = Vector2.zero;
        rectTran.pivot = Vector2.up;
        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchTopRight(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.one;
        rectTran.anchorMax = Vector2.one;
        rectTran.pivot = Vector2.one;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    /// <summary>
    /// 正上中心对齐
    /// </summary>
    /// <param name="rectTran"></param>
    /// <param 是否扩张="isStretch"></param>
    /// <returns></returns>
    public static RectTransform SetUIAnchorStretchTopCenter(RectTransform rectTran, bool isStretch = false)
    {
        rectTran.anchorMin = halfXfullY;
        rectTran.anchorMax = halfXfullY;

        //set real position
        var canvasScaler = rectTran.GetComponentInParent<CanvasScaler>();
        var resolution = canvasScaler.referenceResolution;
        rectTran.anchoredPosition =
            new Vector2(rectTran.anchoredPosition.x, rectTran.anchoredPosition.y - resolution.y / 2);

        if (isStretch)
        {
            rectTran.pivot = halfXfullY;
            rectTran.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, rectTran.sizeDelta.x, rectTran.sizeDelta.y);
            rectTran.anchoredPosition = Vector2.zero;
        }

        return rectTran;
    }

    public static RectTransform SetUIAnchorTopCenter(RectTransform rectTran, float anchoredPositionX = 0,
        float anchoredPositionY = 0)
    {
        rectTran.anchorMin = halfXfullY;
        rectTran.anchorMax = halfXfullY;
        rectTran.pivot = halfXfullY;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = new Vector2(anchoredPositionX, anchoredPositionY);

        return rectTran;
    }

    public static RectTransform SetUIAnchorTopCenter1(RectTransform rectTran)
    {
        rectTran.anchorMin = halfXfullY;
        rectTran.anchorMax = halfXfullY;
        rectTran.pivot = halfXfullY;
        rectTran.anchoredPosition = Vector2.zero;
        return rectTran;
    }

    public static RectTransform SetUIAnchorTopRight(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.one;
        rectTran.anchorMax = Vector2.one;
        rectTran.pivot = Vector2.one;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorBottomRight(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.right;
        rectTran.anchorMax = Vector2.right;
        rectTran.pivot = Vector2.right;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorBottomLeft(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.zero;
        rectTran.anchorMax = Vector2.zero;
        rectTran.pivot = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorMiddleRight(RectTransform rectTran)
    {
        rectTran.anchorMin = fullXhalfY;
        rectTran.anchorMax = fullXhalfY;
        rectTran.pivot = fullXhalfY;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorMiddleCenter(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.one / 2;
        rectTran.anchorMax = Vector2.one / 2;
        rectTran.pivot = Vector2.one / 2;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchLeft(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.zero;
        rectTran.anchorMax = Vector2.up;
        rectTran.pivot = Vector2.up / 2;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchMIddleLeft(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.up / 2;
        rectTran.anchorMax = Vector2.up / 2;
        rectTran.pivot = Vector2.up / 2;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchMIddleRight(RectTransform rectTran)
    {
        rectTran.anchorMin = new Vector2(1f, 0.5f);
        rectTran.anchorMax = new Vector2(1f, 0.5f);
        rectTran.pivot = new Vector2(1f, 0.5f);
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchRight(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.right;
        rectTran.anchorMax = Vector2.one;
        rectTran.pivot = fullXhalfY;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchCenter(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.right / 2;
        rectTran.anchorMax = halfXfullY;
        rectTran.pivot = Vector2.one / 2;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchTop(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.up;
        rectTran.anchorMax = Vector2.one;
        rectTran.pivot = halfXfullY;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchMiddle(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.up / 2;
        rectTran.anchorMax = fullXhalfY;
        rectTran.pivot = Vector2.one / 2;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static RectTransform SetUIAnchorStretchButtom(RectTransform rectTran)
    {
        rectTran.anchorMin = Vector2.zero;
        rectTran.anchorMax = Vector2.right;
        rectTran.pivot = Vector2.right / 2;
        rectTran.offsetMin = Vector2.zero;
        rectTran.offsetMax = Vector2.zero;
        rectTran.anchoredPosition = Vector2.zero;

        return rectTran;
    }

    public static T AddComponentSafety<T>(GameObject go) where T : Component
    {
        T temp;

        if (go.GetComponent<T>() == null)
            temp = go.AddComponent<T>();
        else
            temp = go.GetComponent<T>();

        return temp;
    }

    public static void InitSystemTime()
    {
        Log.Info("InitSystemTime");
        SystemTime = 0;
        outlierCount = 0;
        GetSystemTimeRunTime = 0;
        SystemTimeOffset = 0;
    }

    public static void SetSystemTime(ulong systemTime, sbyte timeZone)
    {
        /*
        long getSystemTime = GetSystemTime();
        Log.Info("SetSystemTime:" + Math.Abs(getSystemTime - systemTime).ToString());
        if (SystemTime != 0 && Math.Abs(getSystemTime - systemTime) > 10000)
        {
            outlierCount++;
            if (outlierCount >= 3 && !UIMixedManager.Instance.IsOpen<ConfirmWindow>()){
                string message = string.Format("<color=#FF0000>{0}</color>", "请检查您的网络状态，以在良好的网络环境下进行游戏。");//TODO::硬编码
                string title = string.Format("<color=#FF0000>{0}</color>", "网络异常");//TODO::硬编码
                Action action = () => { outlierCount = 0; MGameManager.instance.StopSyncTime(); MGameManager.instance.Restart(); };
                UIManager.Instance.OpenPopUpWindow<ConfirmWindow>(CONFORM_WINDOW_TYPE.OK, title, message, action);
            }
        }
        else
        {
            outlierCount = 0;
        }*/
        outlierCount = 0;
        SystemTime = systemTime;
        TimeZoneOffset = timeZone;
        GetSystemTimeRunTime = (ulong)(Time.realtimeSinceStartup * 1000);
    }

    public static void SetSystemTimeOffset(int offsetTime)
    {
        SystemTimeOffset = (ulong)offsetTime;
    }

    public static ulong GetSystemTime()
    {
        return SystemTime + ((ulong)(Time.realtimeSinceStartup * 1000) - GetSystemTimeRunTime) +
               (SystemTimeOffset * 1000);
    }

    public static int GetSystemTime_sce()
    {
        return (int)(GetSystemTime() / 1000);
    }

    public static DateTime GetSystemDateTime()
    {
        if (SystemTime == 0)
        {
            return DateTime.Now;
        }

        DateTime dateTime = GetSystemDateTimeFromUTC().AddHours(TimeZoneOffset);
        return dateTime;
    }

    public static DateTime GetSystemDateTimeFromUTC()
    {
        if (SystemTime == 0)
        {
            return DateTime.Now;
        }

        DateTime dateTime = new DateTime(1970, 1, 1);
        dateTime = dateTime.AddMilliseconds(GetSystemTime());
        return dateTime;
    }

    public static DateTime GetSystemDateTimeFromLocal()
    {
        if (SystemTime == 0)
        {
            return DateTime.Now;
        }

        DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(GetSystemDateTimeFromUTC());
        return dateTime;
    }

    // 显示 UTC 后缀
    public static string GetLocalTimeZoneStr(bool ignoreAutoHide = false)
    {
        //var isEn = SDKUtils.IsEn();
        var isEn = false;
        if (isEn || ignoreAutoHide)
        {
            // var hour = TimeZoneInfo.Local.BaseUtcOffset.Hours; // 客户端时区
            var hour = TimeZoneOffset; // 服务器时区
            string str = hour >= 0 ? "+" + hour : hour.ToString();
            return " UTC " + str;
        }

        return String.Empty;
    }

    // 和服务器时区相比 本地时区的偏移量
    // 比如本机时区 UTC+9 但是服务器时区为 UTC+8 那么偏移量就是 +1 显示配置的时间时的小时 +1 即可
    public static TimeSpan GetTimeZoneOffSet()
    {
        // var isEn = SDKUtils.IsEn();
        // // var isEn = true;
        // if (isEn)
        // {
        //     var adjustRules = TimeZoneInfo.Local.GetAdjustmentRules();
        //     var dayLightDelta = TimeSpan.Zero;
        //     // 不加夏令时 本地时区和服务器时区的 offset
        //     var localTimeZoneOffsetFromServer =
        //         TimeZoneInfo.Local.BaseUtcOffset - TimeSpan.FromHours(TimeZoneOffset);
        //     // 不加夏令时的本地时间
        //     var localTimeWithoutTimeSaving = GetSystemDateTime() + localTimeZoneOffsetFromServer;
        //     foreach (var rule in adjustRules)
        //     {
        //         if (localTimeWithoutTimeSaving >= rule.DateStart && localTimeWithoutTimeSaving <= rule.DateEnd)
        //         {
        //             dayLightDelta += rule.DaylightDelta;
        //         }
        //     }
        //
        //     var result = localTimeZoneOffsetFromServer + dayLightDelta;
        //     // var systemDateTime = GetSystemDateTime();
        //     // Log.Info($"server time {systemDateTime} nots {localTimeWithoutTimeSaving} withts{systemDateTime+result}");
        //     return result;
        // }

        // 现在用服务器时区，区间为 0
        return TimeSpan.Zero;
    }

    public static DayOfWeek GetDayOfWeek(int n)
    {
        if (n == 1) return DayOfWeek.Monday;
        if (n == 2) return DayOfWeek.Tuesday;
        if (n == 3) return DayOfWeek.Wednesday;
        if (n == 4) return DayOfWeek.Thursday;
        if (n == 5) return DayOfWeek.Friday;
        if (n == 6) return DayOfWeek.Saturday;
        if (n == 7) return DayOfWeek.Sunday;
        return DayOfWeek.Monday;
    }

    public static List<DayOfWeek> GetDayOfWeek(List<int> n)
    {
        return n.Select(GetDayOfWeek).ToList();
    }

    /// <summary>
    /// 四点刷新
    /// </summary>
    public static readonly long ResetTime = 4 * 3600;

    /// <summary>
    /// 获得今天刷新的时间戳
    /// </summary>
    public static long GetResetTime()
    {
        return GetResetTime(GetTimeStamp());
    }

    /// <summary>
    /// 获得每日刷新的时间戳
    /// </summary>
    /// <param name="time">时间戳</param>
    /// <returns>每日刷新的时间戳</returns>
    public static long GetResetTime(long time)
    {
        var then = GetDayStart(time) + ResetTime;
        long result;
        if (then > time)
        {
            result = then - 86400;
        }
        else
        {
            result = then;
        }

        return result;
    }

    /// <summary>
    /// 获得每日0点的时间戳
    /// </summary>
    public static long GetDayStart()
    {
        return GetDayStart(GetTimeStamp());
    }

    /// <summary>
    /// 获得当前时间戳
    /// </summary>
    public static long GetTimeStamp()
    {
        return GetTimeStamp(Utilities.GetSystemDateTimeFromUTC());
    }

    public static long GetTimeStamp(DateTime time)
    {
        TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long timestamp = Convert.ToInt64(ts.TotalSeconds);
        return timestamp;
    }

    public static long GetDayStart(long timestamp)
    {
        long localTime = U2L(timestamp);
        long result = L2U(localTime - localTime % 86400);
        return result;
    }

    /// <summary>
    /// UTC 0 -> Local
    /// </summary>
    public static long U2L(long timestamp)
    {
        return timestamp + GetTimeZone() * 3600;
    }

    /// <summary>
    /// Local -> UTC 0
    /// </summary>
    public static long L2U(long timestamp)
    {
        return timestamp - GetTimeZone() * 3600;
    }

    /// <summary>
    /// 获得时区，算上夏令时冬令时
    /// 这里直接返回服务器的时区间隔
    /// </summary>
    /// <returns></returns>
    public static int GetTimeZone()
    {
        return TimeZoneOffset;
    }

    public static bool GetIsNewServer()
    {
        DateTime curSystemDate = new DateTime(1970, 1, 1);
        curSystemDate = curSystemDate.AddMilliseconds(GetSystemTime());

        DateTime openServerDate = new DateTime(1970, 1, 1);
        curSystemDate = curSystemDate.AddMilliseconds(ServerOpenTime_sce * 1000);

        return curSystemDate.Year == openServerDate.Year && curSystemDate.Month == openServerDate.Month;
    }

    /// <summary>
    /// 当前是星期几
    /// </summary>
    /// <param name="refreshTime">几点算更新（1~24时整点）</param>
    /// <returns>星期几（1~7）</returns>
    public static int GetCurWeekDay(int refreshTime = 4)
    {
        DateTime curDate = GetSystemDateTime();
        int curWeek = (int)curDate.DayOfWeek;
        if (curWeek == 0) curWeek = 7; //DayOfWeek.Sunday 变成 7
        if (curDate.Hour < refreshTime)
        {
            curWeek--; //4点才算新的一天
            if (curWeek == 0) curWeek = 7;
        }

        return curWeek;
    }

    public static float GetStringLength(string str)
    {
        if (str.Equals(string.Empty))
            return 0;
        float strlen = 0;
        UTF32Encoding utf32Encoding = new UTF32Encoding();
        byte[] strBytes = utf32Encoding.GetBytes(str);
        for (int i = 0; i < strBytes.Length; i += 4)
        {
            uint code = BitConverter.ToUInt32(strBytes, i);
            if (code <= 0x7f)
            {
                strlen += 1;
            }
            else if (code >= 0x0e00 && code <= 0x0e7f)
            {
                strlen += 1.4f;
            }
            else
            {
                strlen += 2;
            }
        }
        return strlen;
    }

    public static string GetOverflowStr(string name)
    {
        var str = string.Empty;
        int maxbit = 7;
        int bit = 0;
        for (int i = 0; i < name.Length; i++)
        {
            var s = string.Empty + name[i];
            var slen = Encoding.Default.GetByteCount(s);
            if (bit + slen <= maxbit)
            {
                bit += slen;
            }
            else
            {
                break;
            }
            str += s;
        }
        if (str != name)
        {
            str += "...";
        }
        return str;
    }

    public static int GetClientCfgId(int x, int y)
    {
        var clientId = x * (int)Mathf.Pow(10,
            y.ToString().Length) + y;

        return clientId;
    }

    public static bool CompareArr(List<uint> arr1, List<uint> arr2)
    {
        var q = from a in arr1 join b in arr2 on a equals b select a;
        bool flag = arr1.Count == arr2.Count && q.Count() == arr1.Count;
        return flag;//内容相同返回true,反之返回false。
    }

    public static bool CompareArr(List<int> arr1, List<int> arr2)
    {
        var q = from a in arr1 join b in arr2 on a equals b select a;
        bool flag = arr1.Count == arr2.Count && q.Count() == arr1.Count;
        return flag;//内容相同返回true,反之返回false。
    }

    public static bool CompareArr(int[] arr1, int[] arr2)
    {
        var q = from a in arr1 join b in arr2 on a equals b select a;
        bool flag = arr1.Length == arr2.Length && q.Count() == arr1.Length;
        return flag;//内容相同返回true,反之返回false。
    }
}