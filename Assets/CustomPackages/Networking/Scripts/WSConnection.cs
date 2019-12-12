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
    static AsyncQueue<NetworkMessage> queue = new AsyncQueue<NetworkMessage>();
    public static bool init = false;

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
        NetworkMessage msg = GetMessage();
        if (msg != null) {
            Debug.Log(msg.action);
            Debug.Log(msg.data);
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
        var rcvBytes = new byte[128];
        var rcvBuffer = new ArraySegment<byte>(rcvBytes);
        while (true) {
            WebSocketReceiveResult rcvResult = await wsClient.ReceiveAsync(rcvBuffer, cToken);
            byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
            string rcvMsg = Encoding.UTF8.GetString(msgBytes);
            NetworkMessage msg = JsonUtility.FromJson<NetworkMessage>(rcvMsg);
            queue.Enqueue(msg);
            // EventManager.TriggerEvent(msg.action, msg.data);
        }
    }

    async static public void SendMessage(string message) {
        if (init) {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await wsClient.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: WSConnection.cToken);
        }
    }
    public static NetworkMessage GetMessage() {
        return queue.Dequeue();
    }

}

[System.Serializable]
public class NetworkMessage {
    public int code;
    public string action;
    public string data;
}
