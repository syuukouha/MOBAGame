using System;

namespace Assets.Scripts.Manager
{
    /// <summary>
    /// Log封装
    /// </summary>
    public class LogManager
    {
        public static Action<object> Log = UnityEngine.Debug.Log;
        public static Action<object> LogError = UnityEngine.Debug.LogError;
        public static Action<object> LogWarning = UnityEngine.Debug.LogWarning;

        //public static void Log(object log) { }
        //public static void LogError(object log) { }
        //public static void LogWarning(object log) { }


    }
}
