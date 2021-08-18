#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Debugger
{

    /// <summary>
    /// Control all type log output state.
    /// </summary>
    public static bool Enable { get { return 1== PlayerPrefs.GetInt("Debugger_Enable", 1); } set { PlayerPrefs.SetInt("Debugger_Enable", value ? 1 : 0); } }
    /// <summary>
    /// Control  Log output state.
    /// </summary>
    public static bool EnableLog { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableLog", 1); } set { PlayerPrefs.SetInt("Debugger_EnableLog", value ? 1 : 0); } }
    /// <summary>
    /// Control LogWarning  output state.
    /// </summary>
    public static bool EnableLogWarning { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableLogWarning", 1); } set { PlayerPrefs.SetInt("Debugger_EnableLogWarning", value ? 1 : 0); } }
    /// <summary>
    /// Control LogError output state.
    /// </summary>
    public static bool EnableLogError { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableLogError", 1); } set { PlayerPrefs.SetInt("Debugger_EnableLogError", value ? 1 : 0); } }


    #region color prop
    /// <summary>
    /// Control the color of all type log message.
    /// </summary>
    public static bool EnableColorful { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableColorful", 0); } set { PlayerPrefs.SetInt("Debugger_EnableColorful", value ? 1 : 0); } }
    /// <summary>
    /// Control the color of Log message.
    /// </summary>
    public static bool EnableLogColor { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableLogColor", 1); } set { PlayerPrefs.SetInt("Debugger_EnableLogColor", value ? 1 : 0); } }
    /// <summary>
    /// Control the color of LogWaring message.
    /// </summary>
    public static bool EnableLogWarningColor { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableLogWarningColor", 1); } set { PlayerPrefs.SetInt("Debugger_EnableLogWarningColor", value ? 1 : 0); } }
    /// <summary>
    /// Control the color of LogError message.
    /// </summary>
    public static bool EnableLogErrorColor { get { return 1 == PlayerPrefs.GetInt("Debugger_EnableLogErrorColor", 1); } set { PlayerPrefs.SetInt("Debugger_EnableLogErrorColor", value ? 1 : 0); } }

    /// <summary>
    /// color of Log message.
    /// </summary>
    public static Color LogColor { get { return StringToColor(logColorStr); } set { logColorStr = ColorToString(value); } }
    /// <summary>
    /// color of LogWarning message.
    /// </summary>
    public static Color LogWarningColor { get { return StringToColor(logWarningColorStr); } set { logWarningColorStr = ColorToString(value); } }
    /// <summary>
    /// color of LogError message.
    /// </summary>
    public static Color LogErrorColor { get { return StringToColor(logErrorColorStr); } set { logErrorColorStr = ColorToString(value); } }

    private static string logColorStr { get { return  PlayerPrefs.GetString("Debugger_logColorStr", "00ff00"); } set { PlayerPrefs.SetString("Debugger_logColorStr", value ); } }
    private static string logWarningColorStr { get { return PlayerPrefs.GetString("Debugger_logWarningColorStr", "ffff00"); } set { PlayerPrefs.SetString("Debugger_logWarningColorStr", value); } }
    private static string logErrorColorStr { get { return PlayerPrefs.GetString("Debugger_logErrorColorStr", "ff0000"); } set { PlayerPrefs.SetString("Debugger_logErrorColorStr", value); } }

    /// <summary>
    /// Convert color to hexadecimal value
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static string ColorToString(Color color)
    {
        return ColorUtility.ToHtmlStringRGB(color);
    }

    /// <summary>
    /// Convert hexadecimal value to color
    /// </summary>
    /// <param name="colorStr"></param>
    /// <returns></returns>
    private static Color StringToColor(string colorStr)
    { 
        ColorUtility.TryParseHtmlString("#" + colorStr, out Color color);
        return color;
    }

    #endregion


    #region Log

    /// <summary>
    ///  Logs a message to the Unity Console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void Log(object message) {
        if (!Enable || !EnableLog) return; 
        if(EnableColorful && EnableLogColor)
            Debug.Log("<color=#"+ logColorStr + ">" + message+ "</color>");
        else
            Debug.Log(message);
    }
    /// <summary>
    /// Logs a message to the Unity Console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="context">Object to which the message applies.</param>
    public static void Log(object message, Object context){
        if (!Enable || !EnableLog) return;
        if (EnableColorful && EnableLogColor)
            Debug.Log("<color=#" + logColorStr + ">" + message + "</color>", context);
        else
            Debug.Log(message, context);
    }

    #endregion


    #region LogWarning

    /// <summary>
    /// A variant of Debug.Log that logs a warning message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void LogWarning(object message) {
        if (!Enable || !EnableLogWarning) return;
        if (EnableColorful && EnableLogWarningColor)
            Debug.LogWarning("<color=#" + logWarningColorStr + ">" + message + "</color>");
        else
            Debug.LogWarning(message);
    }
    /// <summary>
    /// A variant of Debug.Log that logs a warning message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="context">Object to which the message applies.</param>
    public static void LogWarning(object message, Object context) {
        if (!Enable || !EnableLogWarning) return;
        if (EnableColorful && EnableLogWarningColor)
            Debug.LogWarning("<color=#" + logWarningColorStr + ">" + message + "</color>", context);
        else
            Debug.LogWarning(message, context);
    }

    /// <summary>
    ///  Logs a formatted warning message to the Unity Console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args"> Format arguments.</param>
    public static void LogWarningFormat(string format, params object[] args) {
        if (!Enable || !EnableLogWarning) return;
        if (EnableColorful && EnableLogWarningColor)
            Debug.LogWarningFormat("<color=#" + logWarningColorStr + ">" + format + "</color>", args);
        else
            Debug.LogWarningFormat( format, args);
    }

    /// <summary>
    /// Logs a formatted warning message to the Unity Console.
    /// </summary>
    /// <param name="context">Object to which the message applies.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">Format arguments.</param>
    public static void LogWarningFormat(Object context, string format, params object[] args) {
        if (!Enable || !EnableLogWarning) return;
        if (EnableColorful && EnableLogWarningColor)
            Debug.LogWarningFormat(context,"<color=#" + logWarningColorStr + ">" + format + "</color>", args);
        else
            Debug.LogWarningFormat(context, format, args);
    }

    #endregion


    #region LogError

    /// <summary>
    /// A variant of Debug.Log that logs an error message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void LogError(object message) {
        if (!Enable || !EnableLogError) return;
        if (EnableColorful && EnableLogErrorColor)
            Debug.LogError("<color=#" + logErrorColorStr + ">" + message + "</color>");
        else
            Debug.LogError(message);
    }
    /// <summary>
    /// A variant of Debug.Log that logs an error message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="context">Object to which the message applies.</param>
    public static void LogError(object message, Object context) {
        if (!Enable || !EnableLogError) return;
        if (EnableColorful && EnableLogErrorColor)
            Debug.LogError("<color=#" + logErrorColorStr + ">" + message + "</color>", context);
        else
            Debug.LogError(message, context);
    }

    /// <summary>
    /// Logs a formatted error message to the Unity console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">Format arguments.</param>
    public static void LogErrorFormat(string format, params object[] args) {
        if (!Enable || !EnableLogError) return;
        if (EnableColorful && EnableLogErrorColor)
            Debug.LogErrorFormat("<color=#" + logErrorColorStr + ">" + format + "</color>", args);
        else
            Debug.LogErrorFormat(format, args);
    }

    /// <summary>
    /// Logs a formatted error message to the Unity console.
    /// </summary>
    /// <param name="context">Object to which the message applies.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">Format arguments.</param>
    public static void LogErrorFormat(Object context, string format, params object[] args) {
        if (!Enable || !EnableLogError) return;
        if (EnableColorful && EnableLogErrorColor)
            Debug.LogErrorFormat(context,"<color=#" + logErrorColorStr + ">" + format + "</color>", args);
        else
            Debug.LogErrorFormat(context, format, args);
    }

    #endregion


#if UNITY_EDITOR
    public class DebuggerWindow : EditorWindow
    {
        [MenuItem("Tools/Debugger")]
        private static void ShowWidow()
        {
            EditorWindow.GetWindow<DebuggerWindow>("Debugger"); 
        }



        private  static bool enable = true;
        private  static bool enableLog = true;
        private  static bool enableLogWarning = true;
        private  static bool enableLogError = true;

        private static bool enableColorful = false;
        private static bool enableLogColor = true;
        private static bool enableLogWarningColor = true;
        private static bool enableLogErrorColor = true;
        private static Color logColor = Color.white;
        private static Color logWarningColor = Color.white;
        private static Color logErrorColor = Color.white;

        private  void OnEnable()
        {
            enable = Debugger.Enable;
            enableLog = Debugger.EnableLog;
            enableLogWarning = Debugger.EnableLogWarning;
            enableLogError = Debugger.EnableLogError;

            enableColorful = Debugger.EnableColorful;
            enableLogColor = Debugger.EnableLogColor;
            enableLogWarningColor = Debugger.EnableLogWarningColor;
            enableLogErrorColor = Debugger.EnableLogErrorColor;

            logColor = Debugger.LogColor;
            logWarningColor = Debugger.LogWarningColor;
            logErrorColor = Debugger.LogErrorColor;
         }


        private void OnGUI()
        {
            GUILayout.Space(20f);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            enable = GUILayout.Toggle(enable, "Enable");
            if (enable != Debugger.Enable)
            {
                Debugger.Enable = enable;
            }
            GUILayout.Space(10f);

            enableColorful = GUILayout.Toggle(enableColorful, "Enable Colorful");
            if (enableColorful != Debugger.EnableColorful)
            {
                Debugger.EnableColorful = enableColorful;
            }
            GUILayout.Space(10f);
            EditorGUILayout.EndVertical();
            GUILayout.Space(20f);

            EditorGUI.BeginDisabledGroup(!enable);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);




            //------------------------------Log
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            enableLog = GUILayout.Toggle(enableLog, "Log");
            if (enableLog != Debugger.EnableLog)
            {
                Debugger.EnableLog = enableLog;
            }

            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(!enableLog || !EnableColorful);
            enableLogColor = GUILayout.Toggle(enableLogColor, "Enable Color");
            if (enableLogColor != Debugger.EnableLogColor)
            {
                Debugger.EnableLogColor = enableLogColor;
            }
            EditorGUI.BeginDisabledGroup(!enableLogColor);

            logColor = EditorGUILayout.ColorField(logColor);
            if (logWarningColor != Debugger.LogColor)
            {
                Debugger.LogColor = logColor;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);


            //------------------------------LogWarning
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            enableLogWarning = GUILayout.Toggle(enableLogWarning, "LogWarning");
            if (enableLogWarning != Debugger.EnableLogWarning)
            {
                Debugger.EnableLogWarning = enableLogWarning;
            }
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(!enableLogWarning || !EnableColorful);
            enableLogWarningColor = GUILayout.Toggle(enableLogWarningColor, "Enable Color");
            if (enableLogWarningColor != Debugger.EnableLogWarningColor)
            {
                Debugger.EnableLogColor = enableLogWarningColor;
            }

            EditorGUI.BeginDisabledGroup(!enableLogWarningColor);
            logWarningColor = EditorGUILayout.ColorField(logWarningColor);
            if (logWarningColor != Debugger.LogWarningColor)
            {
                Debugger.LogWarningColor = logWarningColor;
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);



            //------------------------------LogError
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            enableLogError = GUILayout.Toggle(enableLogError, "LogError");
            if (enableLogError != Debugger.EnableLogError)
            {
                Debugger.EnableLogError = enableLogError;
            }
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!enableLogError|| !EnableColorful);
            enableLogErrorColor = GUILayout.Toggle(enableLogErrorColor, "Enable Color");
            if (enableLogErrorColor != Debugger.EnableLogErrorColor)
            {
                Debugger.EnableLogErrorColor = enableLogErrorColor;
            }
            EditorGUI.BeginDisabledGroup(!enableLogErrorColor);

            logErrorColor = EditorGUILayout.ColorField(logErrorColor);
            if (logErrorColor != Debugger.LogErrorColor)
            {
                Debugger.LogErrorColor = logErrorColor;
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();



            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();

        }

    }

#endif


}


