using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NetworkEntity))]
public class NetworkTransform : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    NetworkEntity networkEntity;

    void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
    }

    void Update()
    {

        TransformMessage transformMessage = new TransformMessage(
            transform.position.x,
            transform.position.y,
            transform.position.z,
            transform.rotation.x,
            transform.rotation.y,
            transform.rotation.z
        );

        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            WSConnection.SendMessage(JsonUtility.ToJson(transformMessage));
        }
    }
    void PlayerMoved(string data) {
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

[System.Serializable]
public class TransformMessage
{
    public string action = "move";
    public float px;
    public float py;
    public float pz;

    public float rx;
    public float ry;
    public float rz;

    public TransformMessage(float _px, float _py, float _pz, float _rx, float _ry, float _rz) {
        px = _px;
        py = _py;
        pz = _pz;
        rx = _rx;
        ry = _ry;
        rz = _rz;
    }
}

