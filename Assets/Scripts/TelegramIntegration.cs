using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TelegramIntegration : MonoBehaviour
{
    //    // Вызываем при необходимости отправить результат
    //    public void SendScore(int score)
    //    {
    //        #if UNITY_WEBGL && !UNITY_EDITOR
    //                SendToTelegram(score);
    //        #endif
    //    }

    //    [System.Runtime.InteropServices.DllImport("__Internal")]
    //    private static extern void SendToTelegram(int score);
    public void SendScore(int score)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalCall("SendToTelegram", score);
    #else
            Debug.Log($"WebGL score (test): {score}");
    #endif
        }

}