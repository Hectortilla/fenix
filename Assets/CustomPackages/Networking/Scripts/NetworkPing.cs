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
            WSConnection.SendMessage("ping", new Ping());
        }
    }

    void PongHeard(string data) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(
            UnityEngine.Random.Range(-3.0f, 3.0f),
            UnityEngine.Random.Range(-3.0f, 3.0f),
            UnityEngine.Random.Range(-3.0f, 3.0f)
        );
        // ResponsePing responsePing = JsonUtility.FromJson<ResponsePing>(data);
        // Debug.Log(responsePing.message);
    }
}
