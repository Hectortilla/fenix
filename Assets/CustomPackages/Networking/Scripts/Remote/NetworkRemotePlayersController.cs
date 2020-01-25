using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkRemotePlayersController : Singleton<NetworkRemotePlayersController>
{
    static Player localPlayer;
    [SerializeField]
    GameObject remotePlayerPrefab;
    static Dictionary<string, GameObject> remotePlayers = new Dictionary<string, GameObject>();

    Action<string> actionReceivedGamePlayers = new Action<string>(ReceivedGamePlayers);
    Action<string> actionReceivedPlayerJoined = new Action<string>(ReceivedPlayerJoined);
    Action<string> actionReceivedPlayerLeft = new Action<string>(ReceivedPlayerLeft);
    Action<string> actionReceivedPlayerTransform = new Action<string>(ReceivedPlayerTransform);

    void OnEnable() {
        EventManager.StartListening("GAME_PLAYERS", actionReceivedGamePlayers);
        EventManager.StartListening("PLAYER_JOINED", actionReceivedPlayerJoined);
        EventManager.StartListening("PLAYER_LEFT", actionReceivedPlayerLeft);
        EventManager.StartListening("PLAYER_TRANSFORM", actionReceivedPlayerTransform);
    }
    void OnDisable() {
        EventManager.StopListening("GAME_PLAYERS", actionReceivedGamePlayers);
        EventManager.StopListening("PLAYER_JOINED", actionReceivedPlayerJoined);
        EventManager.StopListening("PLAYER_LEFT", actionReceivedPlayerLeft);
        EventManager.StopListening("PLAYER_TRANSFORM", actionReceivedPlayerTransform);
    }

    // --- Public --- 
    public static void SetLocalPlayer (Player _localPlayer) {
        localPlayer = _localPlayer;
        EventManager.TriggerEvent("UI:NAME", localPlayer.name);
    }
    // --- Network --- 
    static void ReceivedGamePlayers(string data) {
        PlayerList playerList = JsonUtility.FromJson<PlayerList>(data);
        foreach (Player player in playerList.players) {
            AddPlayer(player);
        }
    }

	static void ReceivedPlayerJoined(string data) {
	    AddPlayer(JsonUtility.FromJson<Player>(data));
	}

    static void ReceivedPlayerLeft(string data) {
    	RemovePlayer(JsonUtility.FromJson<Player>(data));
	}

    static void ReceivedPlayerTransform(string data) {
        PlayerTransform playerTransform = JsonUtility.FromJson<PlayerTransform>(data);
        SetRemotePlayerTransform(playerTransform);
    }

    // --- Private --- 
    static void AddPlayer (Player player) {
        if (player.key != localPlayer.key) {
            GameObject go = instance.InstantiateRemotePlayer(player);
            remotePlayers.Add(player.key, go);
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

    GameObject InstantiateRemotePlayer (Player rempotePlayer) {
        GameObject go = Instantiate(remotePlayerPrefab);
        go.AddComponent<NetworkRemotePlayerTransform>();
        return go;
    }
    static void SetRemotePlayerTransform (PlayerTransform playerTransform) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(playerTransform.key, out remotePlayer))
        {
            NetworkRemotePlayerTransform networkRemotePlayerTransform = remotePlayer.GetComponent<NetworkRemotePlayerTransform>();
            networkRemotePlayerTransform.SetPosition(new Vector3(playerTransform.px, playerTransform.py, playerTransform.pz));
            networkRemotePlayerTransform.SetRotation(Quaternion.Euler(new Vector3(playerTransform.rx, playerTransform.ry, playerTransform.rz)));
        }
    }
}
