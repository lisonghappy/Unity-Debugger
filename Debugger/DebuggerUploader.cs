using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

#if UNITY_2018_3_OR_NEWER
using UnityEngine.Networking;
#endif

public class DebuggerUploader : MonoBehaviour
{
    private static object _lock = new object();
    private static DebuggerUploader instance;
    public static DebuggerUploader Instance { get {
            lock (_lock) {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<DebuggerUploader>();
                    if(instance==null)
                        instance = new GameObject(typeof(DebuggerUploader).Name).AddComponent<DebuggerUploader>();
                    DontDestroyOnLoad(Instance.gameObject);
                } 
                return instance;
            }
        } }

    

    public  void Upload()
    {
          StartCoroutine(UploadLogInfoToServer());
        
    }



    private static string uploadFileName = "";
    private static System.Text.StringBuilder logCache = new System.Text.StringBuilder();
    /// <summary>
    /// debug output callback
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    private static void OnLogCallBack(string condition, string stackTrace, LogType type)
    {

        if (string.IsNullOrEmpty(uploadFileName)) return;

        logCache.Append(System.DateTime.Now.ToString("[yyyy-MM-dd  HH:mm:ss]"));
        logCache.Append("\n");
        logCache.Append(condition);
        logCache.Append("\n");
        logCache.Append(stackTrace);
        logCache.Append("\n");

        if (logCache.Length <= 0) return;


        if (!File.Exists(uploadFileName))
            File.Create(uploadFileName).Close();

        using (var fs = File.AppendText(uploadFileName))
        {
            fs.WriteLine(logCache.ToString());
        }
        logCache.Clear();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RefreshUploadPath()
    {
        Application.logMessageReceived -= OnLogCallBack;
        var tempPath = "";
        if (Debugger.ConfigData.EnableUploadLog  && Debugger.ConfigData.EnableUploadLogToLocal)
            tempPath = Application.persistentDataPath;
        else
            tempPath = Application.temporaryCachePath;

        if (!tempPath.Equals(uploadFileName)) {
            uploadIndex = 0;
            uploadFileName = tempPath;
        }

        Application.logMessageReceived += OnLogCallBack;


#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(Debugger.ConfigData.UploadLocalURL))
            uploadFileName = Debugger.ConfigData.UploadLocalURL;
#endif


        StringBuilder headInfoStr = new StringBuilder();
        headInfoStr.Append("unityVersion:    " + Application.unityVersion);
        headInfoStr.Append("\nversion:    " + Application.version);
        headInfoStr.Append("\nidentifier:    " + Application.identifier);
        headInfoStr.Append("\nplatform:    " + Application.platform);
        headInfoStr.Append("\nsystemLanguage:    " + Application.systemLanguage);
        headInfoStr.Append("\ninstallerName:    " + Application.installerName);
        headInfoStr.Append("\ninstallMode:    " + Application.installMode);
        headInfoStr.Append("\ndeviceUniqueIdentifier:    " + SystemInfo.deviceUniqueIdentifier);
        headInfoStr.Append("\ndeviceName:    " + SystemInfo.deviceName);
        headInfoStr.Append("\noperatingSystem:    " + SystemInfo.operatingSystem);
        headInfoStr.Append("\nsystemMemorySize:    " + SystemInfo.systemMemorySize);
        headInfoStr.Append("\n=================================================\n");
        uploadFileName += "/debugger_" + System.DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log";
        File.Create(uploadFileName).Close();
        File.WriteAllText(uploadFileName, headInfoStr.ToString());

        //Debug.LogWarning("uploadFileName:"+ uploadFileName);
    }










/// <summary>
/// upload log to server
/// </summary>
/// <returns></returns>
    private IEnumerator UploadLogInfoToServer()
    {
        while (true)
        {
            if (!Debugger.ConfigData.EnableUploadLog || !Debugger.ConfigData.EnableUploadLogToSercer)
            { 
                yield return new WaitForSeconds(Debugger.ConfigData.uploadServerRate*10f);
                yield break;
            }
            else { 
                yield return new WaitForSeconds(Debugger.ConfigData.uploadServerRate);

                if (isReading) continue;
                if (!File.Exists(uploadFileName)) continue;
                var url = Debugger.ConfigData.UploadServerURL;

                var fileName = Path.GetFileName(uploadFileName);
                var data = ReadLogFile(uploadFileName);

                if (data==null || data.Length <= 0) continue;

                WWWForm form = new WWWForm();

                //insert descript info.
                form.AddField("desc", "debugger-loginfo");
                //insert log info bytes.
                form.AddBinaryData("logfile", data, fileName, "application/x-gzip");
                  

#if UNITY_2018_3_OR_NEWER
                using (UnityWebRequest request = UnityWebRequest.Post(url, form))
                {
                    var result = request.SendWebRequest();

                    while (!result.isDone)
                    {
                        yield return null;
                        //Debug.Log ("uploadProgress: " + request.uploadProgress);
                    }
                    if (!string.IsNullOrEmpty(request.error)) { 
                        //Debug.LogError(request.error);
                    }
                    else { 
                        //Debug.Log("log upload completed. callback info: " + request.downloadHandler.text);
                    }
                }
#else
        using (WWW request = new WWW(url, form))
        {
 
            while (!request.isDone)
            {
                yield return null;
                //Debug.Log ("uploadProgress: " + request.uploadProgress);
            }
            if (!string.IsNullOrEmpty(request.error)){
                //Debug.LogError(request.error);
            }
            else{
                //Debug.Log("log upload completed. callback info: " + request.text);
            }
        }
#endif

            }

        }

        yield return StartCoroutine(UploadLogInfoToServer());

    }

    private static bool isReading = false;
    private static int uploadIndex = 0;

    private byte[] ReadLogFile(string logFilePath)
    {
        byte[] data = null;

        using (FileStream fs = File.OpenRead(logFilePath))
        { 
            long totalLength = fs.Length;

            if (uploadIndex >= totalLength) return data;

            isReading = true;

            //limit read file data
            var readSize = (int)Mathf.Min(Debugger.ConfigData.uploadServerSize * 1024, totalLength- uploadIndex);
            data = new byte[readSize];

            int count  = (int)(readSize - uploadIndex);
             count = Mathf.Min(1024, count);

            while (uploadIndex < readSize)
            {
                int readByteCnt = fs.Read(data, uploadIndex, count);
                uploadIndex += readByteCnt;
              var  leftByteCnt = readSize - uploadIndex;
                count = leftByteCnt > count ? count : (int)leftByteCnt;
            }
        }

        isReading = false;
        return data;
    }
}
