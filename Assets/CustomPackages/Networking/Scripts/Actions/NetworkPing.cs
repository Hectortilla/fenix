using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkPing : MonoBehaviour
{
    private Action<string> actionReceivedPong = new Action<string>(ReceivedPong);

    void OnEnable () {
        EventManager.StartListening("PONG", this.actionReceivedPong);
    }
    void OnDisable () {
        EventManager.StopListening("PONG", this.actionReceivedPong);
    }

    void Update () {
        if (Input.GetKeyDown("p"))
        {
            UDPConnection.Send(new Ping());
        }
    }

    static void ReceivedPong(string data) {
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
