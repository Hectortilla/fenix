using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;
using System.Text;


// https://itq.nl/net-4-5-websocket-client-without-a-browser/
public static class WSConnection
{
    static string serverHost = "ws://localhost:9000/";
    static ClientWebSocket wsClient = new ClientWebSocket();
    static CancellationToken cToken = new CancellationTokenSource().Token;
    private static bool init = false;

    static WSConnection() {
        WSConnection.Init();
    }

    async static void Init() {
        await wsClient.ConnectAsync(new Uri(serverHost), WSConnection.cToken);

        Task.Factory.StartNew(
            async () => {
                var rcvBytes = new byte[128];
                var rcvBuffer = new ArraySegment<byte>(rcvBytes);
                while (true) {
                    WebSocketReceiveResult rcvResult = await wsClient.ReceiveAsync(rcvBuffer, WSConnection.cToken);
                    byte[] msgBytes = rcvBuffer.Skip(rcvBuffer.Offset).Take(rcvResult.Count).ToArray();
                    string rcvMsg = Encoding.UTF8.GetString(msgBytes);
                    dynamic stuff = JsonConvert.DeserializeObject("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }");
                    EventManager.TriggerEvent("pong", "whatever");
                }
            }, WSConnection.cToken, TaskCreationOptions.LongRunning, TaskScheduler.Default
        );
        init = true;
    }
    async public static void SendMessage(string message) {
        if (WSConnection.init) {
            byte[] sendBytes = Encoding.UTF8.GetBytes(message);
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await WSConnection.wsClient.SendAsync(sendBuffer, WebSocketMessageType.Text, endOfMessage: true, cancellationToken: WSConnection.cToken);
        }
    }

}
