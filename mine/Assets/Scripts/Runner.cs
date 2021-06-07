using System;
using UnityEngine;
using Unity.Network;
using System.Collections;
public class Runner : MonoBehaviour
{
    private NetClient _netClient;

    private string _message = "";
    private string _sendMessage = "";
    Vector2 ScrollPos;
    private void Awake()
    {
        _netClient = new NetClient();
        _netClient.OnReceiveMessage += ReceiveMessage;
        _netClient.Start();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
    }
    
    private void OnGUI()
    {   
        GUILayout.BeginArea(new Rect(900, 30, 340, 90));
        GUILayout.TextArea(_message, GUILayout.Width(340), GUILayout.Height(40));
        _sendMessage = GUILayout.TextField(_sendMessage, GUILayout.Width(340), GUILayout.Height(20));
        if (GUILayout.Button("전송하기"))
        {
            if (_sendMessage.Length > 0)
            {
                _message = "관리자에게 보냄 : " + _sendMessage + "\n";
                _netClient.SendMessage(_sendMessage);
                _sendMessage = "";
            }
        }
        
        GUILayout.EndArea();
    }

    private void ReceiveMessage(string message)
    {
        _message = "받은 메세지 : " + message + "\n";
    }

    private void OnApplicationQuit()
    {
        try
        {
            _netClient.Close();
        }
        catch { }
    }
}
