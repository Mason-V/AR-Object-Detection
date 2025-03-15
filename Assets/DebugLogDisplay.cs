using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DebugLogDisplay : MonoBehaviour
{
    [SerializeField] private Text debugText;
    [SerializeField] private int maxLines = 100;

    private void Awake()
    {
        if (debugText == null)
        {
            Debug.LogError("DebugText is not assigned in DebugLogDisplay script!");
            return;
        }

        debugText.text = "";
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedMessage = FormatLogMessage(logString, type);
        UpdateDebugText(formattedMessage);
    }

    private string FormatLogMessage(string message, LogType type)
    {
        return type switch
        {
            LogType.Error => $"[ERROR] {message}",
            LogType.Assert => $"[ERROR] {message}",
            LogType.Exception => $"[EXCEPTION] {message}",
            LogType.Warning => $"[WARNING] {message}",
            _ => message
        };
    }

    private void UpdateDebugText(string newMessage)
    {
        if (debugText == null) return;

        List<string> lines = new List<string>(debugText.text.Split('\n'));
        lines.Add(newMessage);

        // Remove oldest lines if over capacity
        while (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
        }

        debugText.text = string.Join("\n", lines);
    }
}