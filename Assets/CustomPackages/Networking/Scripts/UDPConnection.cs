using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Net;

public class UDPConnection : MonoBehaviour
{
    static string serverHost = "localhost";
    static int serverPort = 9000;

    static UdpClient socket = new UdpClient(11000);

    static Queue<IncomingNetworkMessage> queue = new Queue<IncomingNetworkMessage>();
    public static bool init = false;

    static string[] ignoreActions = {"PLAYERS_TRANSFORM"};

    // Singleton pattern ------- >
    static UDPConnection _instance;

    public static UDPConnection Instance { get { return _instance; } }

    void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    static UDPConnection() {
        UDPConnection.Init();
    }
    // < -------------------------
    static void Init() {
        socket.Connect(serverHost, serverPort);
        Thread oThread = new Thread(ReadSocket);
        oThread.Start();
        oThread.IsBackground = true;
        init = true;
    }

    void Start()
    {
        
    }

    static void ReadSocket() {
        do {
            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            // Blocks until a message returns on this socket from a remote host.
            Byte[] receiveBytes = socket.Receive(ref RemoteIpEndPoint); 
            string msg = Encoding.ASCII.GetString(receiveBytes);
            Debug.Log("Incoming UDP ---> " + msg);

        } while (true);
    }
    // Update is called once per frame
    void Update()
    {

    }
    static public void SendMessage(string data) {
        if (init) {
            Byte[] sendBytes = Encoding.ASCII.GetBytes(data);
            socket.Send(sendBytes, sendBytes.Length);
        }
    }
    void OnDestroy()
    {
        socket.Close();
        Debug.Log("OnDestroy1");
    }
}
