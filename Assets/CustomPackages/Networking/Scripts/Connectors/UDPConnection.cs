using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Net;

public class UDPConnection : Singleton<UDPConnection> {
    static string serverHost = "localhost";
    static int serverPort = 9000;
    public static bool init = false;

    static UdpClient socket = new UdpClient(0);  // free port
    static Queue<IncomingNetworkMessage> queue = new Queue<IncomingNetworkMessage>();

    static string[] ignoreActions = {"PLAYER_TRANSFORM"};

    void Awake() {
        base.Awake();
        UDPConnection.Init();
    }

    static void Init() {
        socket.Connect(serverHost, serverPort);
        Thread oThread = new Thread(ReadSocket);
        oThread.Start();
        oThread.IsBackground = true;
        init = true;
    }

    void Update() {
        IncomingNetworkMessage msg = GetMessage();
        while(msg != null) {
            // if (Array.IndexOf(ignoreActions, msg.action) == -1) {
            //     Debug.Log("Triggering ---> " + msg.action + " --- DATA:  " + msg.data);
            // }
            EventManager.TriggerEvent(msg.action, msg.data);
            msg = GetMessage();
        }
    }
    public static IncomingNetworkMessage GetMessage() {
        if (queue.Count != 0)
            return queue.Dequeue();
        return null;
    }

    static void ReadSocket() {
        do {
            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = socket.Receive(ref RemoteIpEndPoint); 

            string rcvMsg = Encoding.ASCII.GetString(receiveBytes);
            IncomingNetworkMessage msg = JsonUtility.FromJson<IncomingNetworkMessage>(rcvMsg);
            if (Array.IndexOf(ignoreActions, msg.action) == -1) {
                Debug.Log("Incoming ---> " + msg.action + " --- DATA:  " + msg.data);
            }
            queue.Enqueue(msg);

        } while (true);
    }
    
    public static void Send(NetworkMessage data) {
        if (init) {
            Byte[] sendBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(new OutgoingNetworkMessage(data.action, JsonUtility.ToJson(data))));
            socket.Send(sendBytes, sendBytes.Length);
        }
    }

    void OnDestroy()
    {
        Send(new DisconnectMessage());
        socket.Close();
    }
}
