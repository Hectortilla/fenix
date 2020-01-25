﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkRemotePlayersController : Singleton<NetworkRemotePlayersController>
{
    static Player localPlayer;
    [SerializeField]
    GameObject remotePlayerPrefab;
    static Dictionary<string, GameObject> remotePlayers = new Dictionary<string, GameObject>();

    static Action<string> actionReceivedGamePlayers;
    static Action<string> actionReceivedPlayerJoined;
    static Action<string> actionReceivedPlayerLeft;
    static Action<string> actionReceivedPlayerTransform;

    public NetworkRemotePlayersController () {
        actionReceivedGamePlayers = new Action<string>(ReceivedGamePlayers);
        actionReceivedPlayerJoined = new Action<string>(ReceivedPlayerJoined);
        actionReceivedPlayerLeft = new Action<string>(ReceivedPlayerLeft);
        actionReceivedPlayerTransform = new Action<string>(ReceivedPlayersTransform);

        EventManager.StartListening("GAME_PLAYERS", actionReceivedGamePlayers);
        EventManager.StartListening("PLAYER_JOINED", actionReceivedPlayerJoined);
        EventManager.StartListening("PLAYER_LEFT", actionReceivedPlayerLeft);
        EventManager.StartListening("PLAYER_TRANSFORM", actionReceivedPlayerTransform);
    }

    // --- Public --- 
    public static void SetLocalPlayer (Player _localPlayer) {
        localPlayer = _localPlayer;
        EventManager.TriggerEvent("UI:NAME", localPlayer.name);
    }
    // --- Network --- 
    void ReceivedGamePlayers(string data) {
        PlayerList playerList = JsonUtility.FromJson<PlayerList>(data);
        foreach (Player player in playerList.players) {
            AddPlayer(player);
        }
    }

	void ReceivedPlayerJoined(string data) {
	    AddPlayer(JsonUtility.FromJson<Player>(data));
	}

    void ReceivedPlayerLeft(string data) {
    	RemovePlayer(JsonUtility.FromJson<Player>(data));
	}

    void ReceivedPlayersTransform(string data) {
        PlayerTransform playerTransform = JsonUtility.FromJson<PlayerTransform>(data);
        SetRemotePlayerTransform(playerTransform);
    }

    // --- Private --- 
    static void AddPlayer (Player player) {
        if (player.key != localPlayer.key) {
            instance.InstantiateRemotePlayer(player);
        }
        EventManager.TriggerEvent("UI:PLAYERS", (remotePlayers.Count + 1).ToString());
    }
    
    static void RemovePlayer (Player player) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(player.key, out remotePlayer))
        {
            remotePlayers.Remove(player.key);
            Destroy(remotePlayer);
        }
        EventManager.TriggerEvent("UI:PLAYERS", (remotePlayers.Count + 1).ToString());
    }

    void InstantiateRemotePlayer (Player rempotePlayer) {
        GameObject remotePlayerGO = Instantiate(remotePlayerPrefab);
        remotePlayerGO.AddComponent<NetworkRemotePlayerTransform>();
        remotePlayers.Add(rempotePlayer.key, remotePlayerGO);

    }
    static void SetRemotePlayerTransform (PlayerTransform playerTransform) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(playerTransform.key, out remotePlayer))
        {
            NetworkRemotePlayerTransform networkRemotePlayerTransform = remotePlayer.GetComponent<NetworkRemotePlayerTransform>();
            networkRemotePlayerTransform.newTargetPosition = new Vector3(playerTransform.px, playerTransform.py, playerTransform.pz);
            networkRemotePlayerTransform.newTargetRotation = Quaternion.Euler(new Vector3(playerTransform.rx, playerTransform.ry, playerTransform.rz));
        }
    }
}
