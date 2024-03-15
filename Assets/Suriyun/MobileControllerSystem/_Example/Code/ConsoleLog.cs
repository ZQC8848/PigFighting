using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConsoleLog : MonoBehaviour
{

    public Text txt;
    public List<string> cachedStrings;
    public int lineLimit = 10;

    void OnEnable()
    {
        Application.logMessageReceivedThreaded += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceivedThreaded -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        cachedStrings.Add(logString);

        if (cachedStrings.Count > lineLimit)
        {
            cachedStrings.RemoveAt(0);
        }

        txt.text = "";

        for (int i = 0; i < cachedStrings.Count; i++)
        {
            txt.text += cachedStrings[i] + "\n";
        }

    }
}
