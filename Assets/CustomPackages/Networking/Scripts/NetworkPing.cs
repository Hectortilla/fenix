using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkPing : MonoBehaviour
{
    private Action<object> actionPongHeard;

    void Awake () {
        this.actionPongHeard = new Action<object>(this.PongHeard);
    }

    void OnEnable () {
        EventManager.StartListening("PONG", this.actionPongHeard);
    }

    void Update () {
        if (Input.GetKeyDown("p"))
        {
            WSConnection.SendMessage(JsonUtility.ToJson(new Ping()));
        }
        // WSConnection.SendMessage(JsonUtility.ToJson(new Ping()));
    }

    void PongHeard(object data) {
        Debug.Log(data);
    }
}

[System.Serializable]
public class Ping {
    public string action = "ping";
}
