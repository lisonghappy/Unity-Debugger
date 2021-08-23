using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuggerTest : MonoBehaviour
{  

    private void Start()
    {
        //debugger enable
        var debuggerEnableBtn = transform.Find("ButtonEnableDebugger").GetComponent<Button>();
        var debuggerEnableText = debuggerEnableBtn.transform.GetComponentInChildren<Text>();

        debuggerEnableText.text = "Enable   " + Debugger.ConfigData.Enable;
         debuggerEnableBtn.onClick.AddListener(()=> {
             Debugger.ConfigData.Enable = !Debugger.ConfigData.Enable;
             debuggerEnableText.text = "Enable   " + Debugger.ConfigData.Enable;
         });


        //debugger upload
        var debuggerEnableUploadBtn = transform.Find("ButtonEnableDebuggerUpload").GetComponent<Button>();
        var debuggerEnableUploadText = debuggerEnableUploadBtn.transform.GetComponentInChildren<Text>();

        debuggerEnableUploadText.text = "EnableUploadLog   " + Debugger.ConfigData.EnableUploadLog;
        debuggerEnableUploadBtn.onClick.AddListener(() => {
            Debugger.ConfigData.EnableUploadLog = !Debugger.ConfigData.EnableUploadLog;
            debuggerEnableUploadText.text = "EnableUploadLog   " + Debugger.ConfigData.EnableUploadLog;
        });


        transform.Find("DebugLog").GetComponent<Button>().onClick.AddListener(()=> { 
            Debug.Log("-------Log---------------------------------------------------");
        });
        transform.Find("DebugLogWarning").GetComponent<Button>().onClick.AddListener(() => {
            Debug.LogWarning("-------LogWarning---------------------------------------------------");
        });
        transform.Find("DebugLogError").GetComponent<Button>().onClick.AddListener(() => {
            Debug.LogError("-------LogError---------------------------------------------------");
        });

        transform.Find("DebuggerLog").GetComponent<Button>().onClick.AddListener(() => {
            Debugger.Log("-------Log----------------------Debugger-----------------------------");
        });
        transform.Find("DebuggerLogWarning").GetComponent<Button>().onClick.AddListener(() => {
            Debugger.LogWarning("-------LogWarning----------------------Debugger-----------------------------");
        });
        transform.Find("DebuggerLogError").GetComponent<Button>().onClick.AddListener(() => {
            Debugger.LogError("-------LogError----------------------Debugger-----------------------------");
        });
    }


     
}