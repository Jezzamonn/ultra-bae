using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class SocketIoClient : MonoBehaviour
{
    public string serverURL = "http://35.227.49.175:3000";

    Player _player;
    protected Socket socket = null;

    void Destroy()
    {
        DoClose();
    }

    // Use this for initialization
    void Start()
    {
        DoOpen();
        _player = FindObjectOfType<Player>();
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
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                // Access to Unity UI is not allowed in a background thread, so let's put into a shared variable
                Debug.Log("Socket.IO connected.");
                // Send the join room request
                socket.Emit("unity connect", "jezzamon");
            });
            socket.On("set stat", (data) =>
            {
                // Find the player

                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());

                // Desperate times call for desperate measures
                if (values.ContainsKey("MaxHealth"))
                {
                    _player.MaxHealth = int.Parse(values["MaxHealth"]);
                }
                if (values.ContainsKey("Speed"))
                {
                    _player.Speed = float.Parse(values["Speed"]);
                }
                if (values.ContainsKey("NumBullets"))
                {
                    _player.NumBullets = int.Parse(values["NumBullets"]);
                }
                if (values.ContainsKey("BulletSpread"))
                {
                    _player.BulletSpread = float.Parse(values["BulletSpread"]);
                }
                if (values.ContainsKey("BulletCooldown"))
                {
                    _player.BulletCooldown = float.Parse(values["BulletCooldown"]);
                }
                if (values.ContainsKey("BulletLength"))
                {
                    _player.BulletLength = float.Parse(values["BulletLength"]);
                }

                // DEBUG Bonus so I can test without dying
                _player.Health = _player.MaxHealth;
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