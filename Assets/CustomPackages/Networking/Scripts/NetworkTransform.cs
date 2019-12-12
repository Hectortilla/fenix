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
            WSConnection.SendMessage(transformMessage.action, transformMessage);
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
