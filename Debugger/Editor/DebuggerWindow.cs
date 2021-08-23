#if UNITY_EDITOR
 
using System.IO;
using UnityEditor;
using UnityEngine; 

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Networking;
#endif


public class DebuggerWindow : EditorWindow
{
    [MenuItem("Tools/Debugger")]
    private static void ShowWidow()
    {
        EditorWindow.GetWindow<DebuggerWindow>("Debugger");
    }

    private string Version = "ver 0.0.2";
    private GUIStyle linkBtnStyle;
    private GUIStyle titleStyle;

    Vector2 pos = Vector2.zero;

    private void OnGUI()
    {
        this.linkBtnStyle = new GUIStyle("IconButton") { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fixedWidth = 50f };
        this.titleStyle = new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold,fontSize = 40,richText = true };
        
        if (GUILayout.Button(Version, linkBtnStyle)) { OpenLink(); }
        GUILayout.Label("<color=#00FFA7>Debugger</color>", titleStyle);

         
        GUILayout.Space(20f);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        Debugger.ConfigData.Enable = GUILayout.Toggle(Debugger.ConfigData.Enable, "Enable Log");
        GUILayout.Space(10f);
        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.Enable);
         Debugger.ConfigData.EnableColorful = GUILayout.Toggle(Debugger.ConfigData.EnableColorful, "Enable Log Color");
        EditorGUI.EndDisabledGroup();
        GUILayout.Space(10f);
        Debugger.ConfigData.EnableUploadLog = GUILayout.Toggle(Debugger.ConfigData.EnableUploadLog, "Enable Log Upload");
        GUILayout.Space(10f);
        EditorGUILayout.EndVertical();


        pos = GUILayout.BeginScrollView(pos);

        GUILayout.Space(10f);
        DrawUploadPanel();
  
        GUILayout.Space(10f);
        DrawLogStatePanel();
        GUILayout.EndScrollView();

        GUILayout.Space(20f);
        if (GUILayout.Button("Save", GUILayout.Height(50f)))
        {
            Debugger.SaveDebuggerConfigData();
        }
    }


    private static void OpenLink()
    { 
        Application.OpenURL("https://github.com/lisonghappy/Unity-Debugger");
    }

    //-------------------------------------------------------------------------------------------------------LOG----------------------------------
    #region Draw Log State
    /// <summary>
    /// draw log state panel
    /// </summary>
    private void DrawLogStatePanel() {
        GUILayout.Label("LOG");
        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.Enable);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);


        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        Debugger.ConfigData.EnableLog = GUILayout.Toggle(Debugger.ConfigData.EnableLog, "Log");

        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableLog || !Debugger.ConfigData.EnableColorful);
        GUILayout.Space(15f);
        Debugger.ConfigData.EnableLogColor = GUILayout.Toggle(Debugger.ConfigData.EnableLogColor, "Enable Color");

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableLogColor);

        Debugger.ConfigData.LogColor = EditorGUILayout.ColorField(Debugger.ConfigData.LogColor);

        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        GUILayout.Space(10f);


        //------------------------------LogWarning
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        Debugger.ConfigData.EnableLogWarning = GUILayout.Toggle(Debugger.ConfigData.EnableLogWarning, "LogWarning");

        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableLogWarning || !Debugger.ConfigData.EnableColorful);
        GUILayout.Space(15f);
        Debugger.ConfigData.EnableLogWarningColor = GUILayout.Toggle(Debugger.ConfigData.EnableLogWarningColor, "Enable Color");

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableLogWarningColor);
        Debugger.ConfigData.LogWarningColor = EditorGUILayout.ColorField(Debugger.ConfigData.LogWarningColor);

        EditorGUI.EndDisabledGroup();

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
        GUILayout.Space(10f);



        //------------------------------LogError
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        Debugger.ConfigData.EnableLogError = GUILayout.Toggle(Debugger.ConfigData.EnableLogError, "LogError");

        GUILayout.Space(10f);
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableLogError || !Debugger.ConfigData.EnableColorful);
        GUILayout.Space(15f);
        Debugger.ConfigData.EnableLogErrorColor = GUILayout.Toggle(Debugger.ConfigData.EnableLogErrorColor, "Enable Color");

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableLogErrorColor);

        Debugger.ConfigData.LogErrorColor = EditorGUILayout.ColorField(Debugger.ConfigData.LogErrorColor);

        EditorGUI.EndDisabledGroup();
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();



        EditorGUILayout.EndVertical();
        EditorGUI.EndDisabledGroup();
    }

    #endregion


    //-------------------------------------------------------------------------------------------------------UPLOAD----------------------------------
    #region UPLOAD
    /// <summary>
    /// draw upload panel
    /// </summary>
    private void DrawUploadPanel() {
        GUILayout.Label("UPLOAD");
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableUploadLog);
      
         
        //------------------------------------------ editor lcoal upload
  
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
         
        GUILayout.BeginVertical();
        Debugger.ConfigData.EnableUploadLogToLocal = GUILayout.Toggle(Debugger.ConfigData.EnableUploadLogToLocal, "Upload to Local");

        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableUploadLogToLocal);


        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUILayout.Space(15f);

        Debugger.ConfigData.UploadLocalURL = EditorGUILayout.TextField("Path(in Editor)", Debugger.ConfigData.UploadLocalURL);

        if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
        {
            var path = EditorUtility.SaveFolderPanel("selected folder", Debugger.ConfigData.UploadLocalURL, Application.dataPath);
           if(!string.IsNullOrEmpty(path))
                Debugger.ConfigData.UploadLocalURL = path;
         }

        if (GUILayout.Button("Open Folder", GUILayout.ExpandWidth(false)))
        {
            if (Directory.Exists(Debugger.ConfigData.UploadLocalURL))
                UnityEditor.EditorUtility.RevealInFinder(Debugger.ConfigData.UploadLocalURL);
            else
                EditorUtility.DisplayDialog("Error!", "not find this folder path !", "OK");
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);
        if (Debugger.ConfigData.EnableUploadLogToLocal)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);
            EditorGUILayout.HelpBox("When not Editor mode or 'Path(in Editor)' is empty,will use persistentData path.", MessageType.Warning);
            GUILayout.Space(15f);
            GUILayout.EndHorizontal();
        }

        EditorGUI.EndDisabledGroup();


        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        //------------------------------------------server upload
        GUILayout.Space(10f);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
         
        GUILayout.BeginVertical();
        Debugger.ConfigData.EnableUploadLogToSercer = GUILayout.Toggle(Debugger.ConfigData.EnableUploadLogToSercer, "Upload to Server");
        EditorGUI.BeginDisabledGroup(!Debugger.ConfigData.EnableUploadLogToSercer);

        GUILayout.BeginHorizontal();
        GUILayout.Space(20f);
        Debugger.ConfigData.UploadServerURL = EditorGUILayout.TextField("URL", Debugger.ConfigData.UploadServerURL);

        EditorGUI.BeginDisabledGroup(isChecking);
        if (GUILayout.Button(serverCheckState, GUILayout.ExpandWidth(false)))
        {
            isChecking = true;
            ConnectCheck();
        }
        EditorGUI.EndDisabledGroup();

        GUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();


        //------

        EditorGUI.EndDisabledGroup();

        GUILayout.Space(10f);
        EditorGUILayout.EndVertical();
    }
    #endregion

    #region Upoad server check

    private bool isChecking = false;
    private string serverCheckState = "Server Check";
    private DebuggerConnectServerCheck connectCchecker = null;
    private void ConnectCheck()
    {
        serverCheckState = "Connecting";
        connectCchecker = new DebuggerConnectServerCheck(Debugger.ConfigData.UploadServerURL);
        connectCchecker.Check();

        EditorApplication.update += LocalUpdate;
    }

    private void LocalUpdate()
    {
        if (connectCchecker.IsDone())
        {
            EditorApplication.update -= LocalUpdate;
            isChecking = false;
            serverCheckState = connectCchecker.GetResult()?  "Connected": "Connect Failed";
        }
    }

    private class DebuggerConnectServerCheck
    {
        private string url;
        public DebuggerConnectServerCheck(string url)
        {
            this.url = url;
        }

#if UNITY_2018_3_OR_NEWER
        UnityWebRequest www;
#else
        WWW www;
#endif
        public void Check()
        {
            WWWForm form = new WWWForm();
            form.AddField("dec", "test");
            form.AddBinaryData("logfile", new byte[] { });
#if UNITY_2018_3_OR_NEWER
            www = UnityWebRequest.Post(url, form);
            www.SendWebRequest();
#else
        www = new WWW(url, form);
#endif
        }

        public bool IsDone()
        {
            return www.isDone;
        }

        public bool GetResult()
        {
            return string.IsNullOrEmpty(www.error);
        }
    }

    #endregion


}



#endif
