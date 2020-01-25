using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRemotePlayersController : Singleton<NetworkRemotePlayersController>
{
    static Player localPlayer;
    [SerializeField]
    private GameObject remotePlayerPrefab;
    static Dictionary<string, GameObject> remotePlayers = new Dictionary<string, GameObject>();
    static Dictionary<string, Vector3> remotePlayersPositions = new Dictionary<string, Vector3>();
    static Dictionary<string, Vector3> remotePlayersRotations = new Dictionary<string, Vector3>();

    public static void AddPlayer (Player player) {
        if (player.key != localPlayer.key) {
            instance.InstantiateRemotePlayer(player);
        }
        EventManager.TriggerEvent("UI:PLAYERS", (remotePlayers.Count + 1).ToString());
    }
    
    public static void RemovePlayer (Player player) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(player.key, out remotePlayer))
        {
            remotePlayers.Remove(player.key);
            Destroy(remotePlayer);
        }
        EventManager.TriggerEvent("UI:PLAYERS", (remotePlayers.Count + 1).ToString());
    }

    public static void SetLocalPlayer (Player _localPlayer) {
        localPlayer = _localPlayer;
        EventManager.TriggerEvent("UI:NAME", localPlayer.name);
    }

    public void InstantiateRemotePlayer (Player rempotePlayer) {
        GameObject remotePlayerGO = Instantiate(remotePlayerPrefab);
        remotePlayerGO.AddComponent<NetworkRemotePlayerTransform>();
        remotePlayers.Add(rempotePlayer.key, remotePlayerGO);

    }
    /*
    public static void MovePlayer (PlayerTransform playerTransform) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(playerTransform.key, out remotePlayer))
        {
            GameObject remotePlayerGO = remotePlayers[playerTransform.key];
            remotePlayerGO.transform.position = new Vector3(playerTransform.px, playerTransform.py, playerTransform.pz);
            remotePlayerGO.transform.rotation = Quaternion.Euler(new Vector3(playerTransform.rx, playerTransform.ry, playerTransform.rz));
        }
    }
    */
    public static void SetRemotePlayerTransform (PlayerTransform playerTransform) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(playerTransform.key, out remotePlayer))
        {
            NetworkRemotePlayerTransform networkRemotePlayerTransform = remotePlayer.GetComponent<NetworkRemotePlayerTransform>();
            networkRemotePlayerTransform.newTargetPosition = new Vector3(playerTransform.px, playerTransform.py, playerTransform.pz);
            networkRemotePlayerTransform.newTargetRotation = Quaternion.Euler(new Vector3(playerTransform.rx, playerTransform.ry, playerTransform.rz));
        }
    }
}
