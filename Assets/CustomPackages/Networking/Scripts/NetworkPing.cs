using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkPing : MonoBehaviour
{
    private Action<string> actionPongHeard;

    void Awake () {
        this.actionPongHeard = new Action<string>(this.PongHeard);
    }

    void OnEnable () {
        EventManager.StartListening("PONG", this.actionPongHeard);
    }

    void Update () {
        if (Input.GetKeyDown("p"))
        {
            WSConnection.SendMessage(JsonUtility.ToJson(new Ping()));
        }
    }

    void PongHeard(string data) {
        ResponsePing responsePing = JsonUtility.FromJson<ResponsePing>(data);
        Debug.Log(responsePing.message);
    }
}

[System.Serializable]
public class Ping {
    public string action = "ping";
}

[System.Serializable]
public class ResponsePing {
    public string message;
}
