using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class SocketIoClient : MonoBehaviour
{
    public string serverURL = "http://35.227.49.175:3000";

    protected Socket socket = null;

    public bool PhoneConnected = false;
    public string UserName = "";

    Dictionary<string, string> nextData = null;

    void Destroy()
    {
        DoClose();
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        var p = FindObjectOfType<Player>();

        if (nextData != null)
        {
            // Desperate times call for desperate measures
            if (nextData.ContainsKey("MaxHealth"))
            {
                p.MaxHealth = int.Parse(nextData["MaxHealth"]);
            }
            if (nextData.ContainsKey("Speed"))
            {
                p.Speed = float.Parse(nextData["Speed"]);
            }
            if (nextData.ContainsKey("NumBullets"))
            {
                p.NumBullets = int.Parse(nextData["NumBullets"]);
            }
            if (nextData.ContainsKey("BulletSpread"))
            {
                p.BulletSpread = float.Parse(nextData["BulletSpread"]);
            }
            if (nextData.ContainsKey("BulletCooldown"))
            {
                p.BulletCooldown = float.Parse(nextData["BulletCooldown"]);
            }
            if (nextData.ContainsKey("BulletLength"))
            {
                p.BulletLength = float.Parse(nextData["BulletLength"]);
            }

            // DEBUG Bonus so I can test without dying
            p.Health = p.MaxHealth;

            // Clear it now we've updated stuff
            nextData = null;
        }
    }

    public void DoOpen()
    {
        if (socket == null)
        {
            socket = IO.Socket(serverURL);
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Debug.Log("Socket.IO connected.");
                // Send the join room request
                socket.Emit("unity connect", UserName);
            });

            socket.On("phone connect", () =>
            {
                Debug.Log("A phone connected!");
                // A phone joined!
                PhoneConnected = true;
            });

            socket.On("set stat", (data) =>
            {
                // This must mean a phone is connected.
                PhoneConnected = true;
                nextData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
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