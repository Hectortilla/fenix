using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


// https://itq.nl/net-4-5-websocket-client-without-a-browser/
public class WSConnection : MonoBehaviour {
    
    static string serverHost = "ws://localhost:9000/";
    static ClientWebSocket wsClient = new ClientWebSocket();
    static CancellationToken cToken = new CancellationTokenSource().Token;
    static AsyncQueue<IncomingNetworkMessage> queue = new AsyncQueue<IncomingNetworkMessage>();
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
        if (msg != null) {
            EventManager.TriggerEvent(msg.action, msg.data);
        }
    }

    async static void Init() {
        await wsClient.ConnectAsync(new Uri(serverHost), cToken);
        Thread oThread = new Thread(ReadWS);
        oThread.Start();
        oThread.IsBackground = true;
        init = true;
    }

    async static void ReadWS() {
        var rcvBytes = new byte[256];
        var rcvBuffer = new ArraySegment<byte>(rcvBytes);
        while (true) {
            WebSocketReceiveResult rcvResult = await wsClient.ReceiveAsync(rcvBuffer, cToken);
            byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
            string rcvMsg = Encoding.UTF8.GetString(msgBytes);
            IncomingNetworkMessage msg = JsonUtility.FromJson<IncomingNetworkMessage>(rcvMsg);
            if (Array.IndexOf(ignoreActions, msg.action) == -1) {
                Debug.Log("Incoming ---> " + msg.action + " --- DATA:  " + msg.data);
            }
            queue.Enqueue(msg);
            // EventManager.TriggerEvent(msg.action, msg.data);
        }
    }

    async static public void SendMessage(string action, object data) {
        if (init) {
            byte[] sendBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new OutgoingNetworkMessage(action, JsonUtility.ToJson(data))));
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await wsClient.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: WSConnection.cToken);
        }
    }
    public static IncomingNetworkMessage GetMessage() {
        return queue.Dequeue();
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
