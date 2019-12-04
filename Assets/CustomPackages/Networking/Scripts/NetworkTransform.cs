using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkTransform : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    void Update()
    {

        TransformMessage transformMessage = new TransformMessage();

        transformMessage.px = transform.position.x;
        transformMessage.py = transform.position.y;
        transformMessage.pz = transform.position.z;
        transformMessage.rx = transform.rotation.x;
        transformMessage.ry = transform.rotation.y;
        transformMessage.rz = transform.rotation.z;

        if (Time.time > nextActionTime ) {
            nextActionTime += period;
            WSConnection.SendMessage(JsonUtility.ToJson(transformMessage));
        }
    }
}

[System.Serializable]
public class TransformMessage
{
    public string action = "positionUpdate";
    public float px;
    public float py;
    public float pz;
    public float rx;
    public float ry;
    public float rz;
}

