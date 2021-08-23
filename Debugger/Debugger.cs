 
using System.IO;
using System.Text;
using UnityEngine;

public class Debugger
{
    /// <summary>
    /// configdata file path.
    /// </summary>
    public static string ConfigDataFilePath { get { return Application.persistentDataPath+ "/DebuggerConfigData.json"; } }
    private static string ResourcesConfigFilePath = Application.dataPath+ "/Resources/" + Path.GetFileName(ConfigDataFilePath);

    #region color prop

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

    #region /////////////////////////////////       ConfigData       /////////////////////////////////


    private static DebuggerConfig configData;
    public static DebuggerConfig ConfigData
    {
        get
        {
            if (configData == null)
            {
                string filePath = ConfigDataFilePath;



#if UNITY_EDITOR

                var textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(ResourcesConfigFilePath.Replace(Application.dataPath, "Assets/"));

                if (textAsset != null && !string.IsNullOrEmpty(textAsset.text))
                {
                    try { configData = JsonUtility.FromJson<DebuggerConfig>(textAsset.text); }
                    catch (System.Exception) { }
                }


                if (configData == null)
                {

                    if (File.Exists(ResourcesConfigFilePath))
                        File.Delete(ResourcesConfigFilePath);
                    configData = new DebuggerConfig();
                    configData.UPDATEDATE = System.DateTime.Now.ToString("yyyyMMddHHmmss");

                    File.Create(ResourcesConfigFilePath).Close();
                    File.WriteAllText(ResourcesConfigFilePath, JsonUtility.ToJson(configData));
                    UnityEditor.AssetDatabase.Refresh();
                }

#else
   var textAsset = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(filePath));
                DebuggerConfig tempConfigData = null;
                bool isNeedUpdate = false;

                //get the last configdata from resources file
                if (textAsset != null && !string.IsNullOrEmpty(textAsset.text))
                {
                    try { tempConfigData = JsonUtility.FromJson<DebuggerConfig>(textAsset.text); }
                    catch (System.Exception) { }
                }

                //get config data from cache file
                if (File.Exists(filePath))
                {
                    using (var fileRead = File.OpenRead(filePath))
                    {
                        var bytes = new byte[fileRead.Length];
                        fileRead.Read(bytes, 0, bytes.Length);
                        var jsonStr = UnicodeEncoding.UTF8.GetString(bytes);
                        if (jsonStr != null && !string.IsNullOrEmpty(jsonStr))
                        {
                            try { configData = JsonUtility.FromJson<DebuggerConfig>(jsonStr); }
                            catch (System.Exception) { }
                        }

                    }
                }
                 

                //check update or create new 
                 if (tempConfigData != null)
                {
                    if (configData == null || !configData.UPDATEKEY.Equals(tempConfigData.UPDATEKEY))
                    {
                        configData = tempConfigData;
                        isNeedUpdate = true;

                    }
                    else
                    {
                        if (configData == null)
                        {
                            configData = new DebuggerConfig();
                            isNeedUpdate = true;
                        }
                    }

                    //update 
                    if (isNeedUpdate)
                    {
                        if (configData == null)
                            configData = new DebuggerConfig();
                        File.Create(filePath).Close();

                        using (var fw = File.OpenWrite(filePath))
                        {
                            var bytes = UnicodeEncoding.UTF8.GetBytes(JsonUtility.ToJson(configData));
                            fw.Write(bytes, 0, bytes.Length);
                        }
                    }
                }
#endif

            if (configData.EnableUploadLog && configData.EnableUploadLogToSercer)
                DebuggerUploader.Instance.Upload();
            }

            return configData;
        }

    }


    /// <summary>
    /// save config data to file.
    /// </summary>
    public static void SaveDebuggerConfigData() {
        string filePath = ConfigDataFilePath;
        if (configData == null) 
            configData = new DebuggerConfig();

        configData.UPDATEDATE = System.DateTime.Now.ToString("yyyyMMddHHmmss");

#if UNITY_EDITOR
         if (!File.Exists(ResourcesConfigFilePath))
            File.Create(ResourcesConfigFilePath).Close();
        File.WriteAllText(ResourcesConfigFilePath, JsonUtility.ToJson(configData));
        UnityEditor.AssetDatabase.Refresh();
#endif

        Debug.LogWarning("debugger config data file update.");


       DebuggerUploader.RefreshUploadPath();
    }


#endregion




#region /////////////////////////////////       Log     /////////////////////////////////

    /// <summary>
    ///  Logs a message to the Unity Console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void Log(object message) {
        if (!ConfigData.Enable || !ConfigData.EnableLog) return; 
        if(ConfigData.EnableColorful && ConfigData.EnableLogColor)
            Debug.Log("<color=#"+ ColorToString(ConfigData.LogColor) + ">" + message+ "</color>");
        else
            Debug.Log(message);
    }
    /// <summary>
    /// Logs a message to the Unity Console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="context">Object to which the message applies.</param>
    public static void Log(object message, Object context){
        if (!ConfigData.Enable || !ConfigData.EnableLog) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogColor)
            Debug.Log("<color=#" + ColorToString(ConfigData.LogColor) + ">" + message + "</color>", context);
        else
            Debug.Log(message, context);
    }

#endregion


#region /////////////////////////////////       LogWarning      /////////////////////////////////

    /// <summary>
    /// A variant of Debug.Log that logs a warning message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void LogWarning(object message) {
        if (!ConfigData.Enable || !ConfigData.EnableLogWarning) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogWarningColor)
            Debug.LogWarning("<color=#" + ColorToString(ConfigData.LogWarningColor) + ">" + message + "</color>");
        else
            Debug.LogWarning(message);
    }
    /// <summary>
    /// A variant of Debug.Log that logs a warning message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="context">Object to which the message applies.</param>
    public static void LogWarning(object message, Object context) {
        if (!ConfigData.Enable || !ConfigData.EnableLogWarning) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogWarningColor)
            Debug.LogWarning("<color=#" + ColorToString(ConfigData.LogWarningColor) + ">" + message + "</color>", context);
        else
            Debug.LogWarning(message, context);
    }

    /// <summary>
    ///  Logs a formatted warning message to the Unity Console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args"> Format arguments.</param>
    public static void LogWarningFormat(string format, params object[] args) {
        if (!ConfigData.Enable || !ConfigData.EnableLogWarning) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogWarningColor)
            Debug.LogWarningFormat("<color=#" + ColorToString(ConfigData.LogWarningColor) + ">" + format + "</color>", args);
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
        if (!ConfigData.Enable || !ConfigData.EnableLogWarning) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogWarningColor)
            Debug.LogWarningFormat(context,"<color=#" + ColorToString(ConfigData.LogWarningColor) + ">" + format + "</color>", args);
        else
            Debug.LogWarningFormat(context, format, args);
    }

#endregion


#region /////////////////////////////////       LogError        /////////////////////////////////

    /// <summary>
    /// A variant of Debug.Log that logs an error message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    public static void LogError(object message) {
        if (!ConfigData.Enable || !ConfigData.EnableLogError) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogErrorColor)
            Debug.LogError("<color=#" + ColorToString(ConfigData.LogErrorColor)+ ">" + message + "</color>");
        else
            Debug.LogError(message);
    }
    /// <summary>
    /// A variant of Debug.Log that logs an error message to the console.
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="context">Object to which the message applies.</param>
    public static void LogError(object message, Object context) {
        if (!ConfigData.Enable || !ConfigData.EnableLogError) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogErrorColor)
            Debug.LogError("<color=#" + ColorToString(ConfigData.LogErrorColor) + ">" + message + "</color>", context);
        else
            Debug.LogError(message, context);
    }

    /// <summary>
    /// Logs a formatted error message to the Unity console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">Format arguments.</param>
    public static void LogErrorFormat(string format, params object[] args) {
        if (!ConfigData.Enable || !ConfigData.EnableLogError) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogErrorColor)
            Debug.LogErrorFormat("<color=#" + ColorToString(ConfigData.LogErrorColor) + ">" + format + "</color>", args);
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
        if (!ConfigData.Enable || !ConfigData.EnableLogError) return;
        if (ConfigData.EnableColorful && ConfigData.EnableLogErrorColor)
            Debug.LogErrorFormat(context,"<color=#" + ColorToString(ConfigData.LogErrorColor) + ">" + format + "</color>", args);
        else
            Debug.LogErrorFormat(context, format, args);
    }

#endregion


#region ////////////////////////        Solve the log double-click traceability problem     ////////////////////////

//#if UNITY_EDITOR
//    //Solve the log double-click traceability problem
//    [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
//    static bool OnOpenAsset(int instanceID, int line)
//    {
//        string stackTrace = GetStackTrace();
//        if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains(typeof(Debugger).Name+":Log"))
//        {
//            //Use regular expressions to match which line of at which script
//            var matches = System.Text.RegularExpressions.Regex.Match(stackTrace, @"\(at (.+)\)",System.Text.RegularExpressions.RegexOptions.IgnoreCase);
//            string pathLine = "";
//            while (matches.Success)
//            {
//                pathLine = matches.Groups[1].Value;

//                if (!pathLine.Contains(typeof(Debugger).Name+".cs"))
//                {
//                    int splitIndex = pathLine.LastIndexOf(":");
//                    // script path
//                    string path = pathLine.Substring(0, splitIndex);
//                    // line number
//                    line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
//                    string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
//                    fullPath = fullPath + path;
 
//                    // jump to target script code line
//                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
//                    break;
//                }
//                matches = matches.NextMatch();
//            }
//            return true;
//        }
//        return false;
//    }

//    /// <summary>
//    /// get current consolewindow log stack info.
//    /// </summary>
//    /// <returns></returns>
//    static string GetStackTrace()
//    {
//        // get ConsoleWindow class
//        var ConsoleWindowType = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
//        //  get consolewindow instance.
//        var fieldInfo = ConsoleWindowType.GetField("ms_ConsoleWindow",System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.NonPublic);
//        var consoleInstance = fieldInfo.GetValue(null);
//        if (consoleInstance != null)
//        {
//            if ((object)UnityEditor.EditorWindow.focusedWindow == consoleInstance)
//            {
//                // get m_ActiveText in consolewindow
//                fieldInfo = ConsoleWindowType.GetField("m_ActiveText",System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.NonPublic);
//                // get m_ActiveText's value
//                string activeText = fieldInfo.GetValue(consoleInstance).ToString();
//                return activeText;
//            }
//        }
//        return null;
//    }
//#endif

#endregion


}
