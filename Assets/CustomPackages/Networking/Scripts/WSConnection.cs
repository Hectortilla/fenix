using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
// https://itq.nl/net-4-5-websocket-client-without-a-browser/
public class WSConnection : MonoBehaviour {
    
    static string serverHost = "ws://localhost:9000/";
    static ClientWebSocket socket = new ClientWebSocket();

    static Queue<IncomingNetworkMessage> queue = new Queue<IncomingNetworkMessage>();
    public static bool init = false;

    static string[] ignoreActions = {"PLAYERS_TRANSFORM"};

    // Singleton pattern ------- >
    static WSConnection _instance;

    public static WSConnection Instance { get { return _instance; } }

    void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    static WSConnection() {
        WSConnection.Init();
    }
    // < -------------------------

    void Update() {
        IncomingNetworkMessage msg = GetMessage();
        while(msg != null) {
            if (Array.IndexOf(ignoreActions, msg.action) == -1) {
                Debug.Log("Triggering ---> " + msg.action + " --- DATA:  " + msg.data);
            }
            EventManager.TriggerEvent(msg.action, msg.data);
            msg = GetMessage();
        }
    }

    async static void Init() {
        await socket.ConnectAsync(new Uri(serverHost), CancellationToken.None);
        Thread oThread = new Thread(ReadWS);
        oThread.Start();
        oThread.IsBackground = true;
        init = true;
    }

    async static void ReadWS() {
        var buffer = new ArraySegment<byte>(new byte[2048]);

        do {
            WebSocketReceiveResult result;
            using (var ms = new MemoryStream()) {
                do {
                    result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                    break;

                ms.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(ms, Encoding.UTF8)) {

                    string rcvMsg = await reader.ReadToEndAsync();
                    IncomingNetworkMessage msg = JsonUtility.FromJson<IncomingNetworkMessage>(rcvMsg);
                    if (Array.IndexOf(ignoreActions, msg.action) == -1) {
                        Debug.Log("Incoming ---> " + msg.action + " --- DATA:  " + msg.data);
                    }
                    queue.Enqueue(msg);
                }
            }

        } while (true);
    }

    async static public void SendMessage(string action, object data) {
        if (init) {
            byte[] sendBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new OutgoingNetworkMessage(action, JsonUtility.ToJson(data))));
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await socket.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
        }
    }
    public static IncomingNetworkMessage GetMessage() {
        if (queue.Count != 0)
            return queue.Dequeue();
        return null;
    }

}

[System.Serializable]
public class IncomingNetworkMessage {
    public int code;
    public string action;
    public string data;
}


[System.Serializable]
public class OutgoingNetworkMessage {
    public string action;
    public string data;

    public OutgoingNetworkMessage(string _action, string _data){
        action = _action;
        data = _data;
    }
}
