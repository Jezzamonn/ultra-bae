﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class SocketIoClient : MonoBehaviour
{
    public string serverURL = "http://35.227.49.175:3000";

    protected Socket socket = null;

    void Destroy()
    {
        DoClose();
    }

    // Use this for initialization
    void Start()
    {
        DoOpen();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DoOpen()
    {
        if (socket == null)
        {
            socket = IO.Socket(serverURL);
            socket.On(Socket.EVENT_CONNECT, () => {
                // Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
                Debug.Log("Socket.IO connected.");
                // Send the join room request
                socket.Emit("unity connect", "jezzamon");
            });
            socket.On("set stat", (data) => {
                string str = data.ToString();

                Debug.Log(str);
                // TODO: Update player stats
            });
        }
    }

    void DoClose()
    {
        if (socket != null)
        {
            socket.Disconnect();
            socket = null;
        }
    }

    void SendChat(string str)
    {
        if (socket != null)
        {
            socket.Emit("chat message", str);
        }
    }
}