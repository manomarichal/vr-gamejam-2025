using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public TextMeshProUGUI debugText; // Assign this in Inspector
    private static DebugLogger instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Application.logMessageReceived += HandleLog;
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (debugText != null)
        {
            debugText.text += logString + "\n";
            if (debugText.text.Length > 5000) // Prevent overflow
            {
                debugText.text = debugText.text.Substring(debugText.text.Length - 5000);
            }
        }
    }
}
