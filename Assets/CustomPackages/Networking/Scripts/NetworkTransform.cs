using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(NetworkEntity))]
public class NetworkTransform : MonoBehaviour
{
    private Action<string> actionReceivedPlayerTransform;

    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    NetworkEntity networkEntity;

    void Awake () {
        this.actionReceivedPlayerTransform = new Action<string>(this.ReceivedPlayersTransform);
    }

    void OnEnable () {
        EventManager.StartListening("PLAYER_TRANSFORM", this.actionReceivedPlayerTransform);
    }

    void Start()
    {
        networkEntity = GetComponent<NetworkEntity>();
    }

    void FixedUpdate()
    {
        if (!networkEntity.isAuth) return;

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
            UDPConnection.Send(transformMessage);
        }
    }
    void ReceivedPlayersTransform(string data) {
        PlayerTransform playerTransform = JsonUtility.FromJson<PlayerTransform>(data);
        NetworkRemotePlayersController.SetRemotePlayerTransform(playerTransform);
    }
}
