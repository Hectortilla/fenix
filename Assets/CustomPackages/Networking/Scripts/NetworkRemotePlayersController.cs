using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRemotePlayersController : MonoBehaviour
{
    static Player localPlayer;
    [SerializeField]
    private GameObject remotePlayerPrefab;
    static Dictionary<string, GameObject> remotePlayers = new Dictionary<string, GameObject>();

    // Singleton pattern ------- >
    static NetworkRemotePlayersController _instance;

    public static NetworkRemotePlayersController Instance { get { return _instance; } }

    void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    static NetworkRemotePlayersController() {
        // WSConnection.Init();
    }
    /*async static void Init() {
    }*/
    // < -------------------------
    public static void AddPlayer (Player player) {
        if (player.key != localPlayer.key) {
            _instance.InstantiateRemotePlayer(player);

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
        remotePlayers.Add(rempotePlayer.key, remotePlayerGO);

    }
    public static void MovePlayer (PlayerTransform playerTransform) {
        GameObject remotePlayer = null;
        if(remotePlayers.TryGetValue(playerTransform.key, out remotePlayer))
        {
            GameObject remotePlayerGO = remotePlayers[playerTransform.key];
            remotePlayerGO.transform.position = new Vector3(playerTransform.px, playerTransform.py, playerTransform.pz);
            remotePlayerGO.transform.rotation = Quaternion.Euler(new Vector3(playerTransform.rx, playerTransform.ry, playerTransform.rz));
        }
    }
}
