 


public class DebuggerConfig
{

    /// <summary>
    /// check debugger config data be latest.
    /// </summary>
    public string UPDATEDATE = "";

    //-------------------------------------state

    /// <summary>
    /// Control all type log output state.
    /// </summary>
    public bool Enable = false;
    /// <summary>
    /// Control  Log output state.
    /// </summary>
    public bool EnableLog = true;
    /// <summary>
    /// Control LogWarning  output state.
    /// </summary>
    public bool EnableLogWarning = true;
    /// <summary>
    /// Control LogError output state.
    /// </summary>
    public bool EnableLogError = true;


    //-------------------------------------log color

    /// <summary>
    ///  Control the color of all type log message.
    /// </summary>
    public bool EnableColorful = false;
    /// <summary>
    ///  Control the color of Log message.
    /// </summary>
    public bool EnableLogColor = true;
    /// <summary>
    /// Control the color of LogWaring message.
    /// </summary>
    public bool EnableLogWarningColor = true;
    /// <summary>
    ///  Control the color of LogError message.
    /// </summary>
    public bool EnableLogErrorColor = true;

    /// <summary>
    /// Color of Log message.
    /// </summary>
    public UnityEngine.Color LogColor = UnityEngine.Color.green;
    /// <summary>
    /// Color of LogWarning message.
    /// </summary>
    public UnityEngine.Color LogWarningColor = UnityEngine.Color.yellow;
    /// <summary>
    /// Color of LogError message.
    /// </summary>
    public UnityEngine.Color LogErrorColor = UnityEngine.Color.red;


    //-------------------------------------upload
    /// <summary>
    /// Control log upload state.
    /// </summary>
    public bool EnableUploadLog = false;
    /// <summary>
    /// Control log upload to server.
    /// </summary>
    public bool EnableUploadLogToSercer = false;
    /// <summary>
    /// Control log upload to local.
    /// </summary>
    public bool EnableUploadLogToLocal = false;


    /// <summary>
    /// The url of the server where the logs are uploaded.
    /// </summary>
    public string UploadServerURL;
    /// <summary>
    /// The url of the local where the logs are uploaded.
    /// </summary>
    public string UploadLocalURL = UnityEngine.Application.persistentDataPath;

    /// <summary>
    /// Frequency of uploading to server.(single upload to server log delay time(:second))
    /// </summary>
    [UnityEngine.Range(1f,10f)]public float uploadServerRate = 1f;
    /// <summary>
    /// single upload to server  log size(:Kb).
    /// </summary>
    [UnityEngine.Range(1, 1025)] public int uploadServerSize = 1;
}