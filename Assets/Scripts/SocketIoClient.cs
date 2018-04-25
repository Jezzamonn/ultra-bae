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

    // for thread mumbo jumbo
    private Object thisLock = new Object();

    public bool StartPinging = false;
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

        if (p != null && nextData != null)
        {
            lock(thisLock) {
                // between grabbing the lock it got null? That's no good.
                if (nextData == null) {
                    return;
                }

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

        if (StartPinging) {
            lock(thisLock) {
                StartCoroutine(PingServerUntilIGotAPhone());
                StartPinging = false;
            }
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
                lock(thisLock) {
                    PhoneConnected = false;
                    StartPinging = true;
                }
            });

            socket.On("phone connect", () =>
            {
                Debug.Log("A phone connected!");
                // A phone joined!
                lock(thisLock) {
                    PhoneConnected = true;
                }
            });

            socket.On("set stat", (data) =>
            {
                // This must mean a phone is connected.
                lock(thisLock) {
                    PhoneConnected = true;
                    nextData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data.ToString());
                }
            });
        }
    }

    /// <summary>
    /// Pings the server every second until a phone connects.
    /// </summary>
    IEnumerator PingServerUntilIGotAPhone()
    {
        while (!PhoneConnected) {
            if (socket != null)
            {
                Debug.Log(string.Format("Trying to join {0}", UserName));
                socket.Emit("unity connect", UserName);
            }
            yield return new WaitForSeconds(10f);
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