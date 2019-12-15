using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkEntity : MonoBehaviour
{
	Player mySlef;
	
    private Action<string> actionReceivedAuth;
    private Action<string> actionReceivedGamePlayers;
    private Action<string> actionReceivedPlayerJoined;

    void Awake () {
        this.actionReceivedAuth = new Action<string>(this.ReceivedAuth);
        this.actionReceivedGamePlayers = new Action<string>(this.ReceivedGamePlayers);
        this.actionReceivedPlayerJoined = new Action<string>(this.ReceivedPlayerJoined);
    }

    void OnEnable () {
        EventManager.StartListening("AUTH_PLAYER", this.actionReceivedAuth);
        EventManager.StartListening("GAME_PLAYERS", this.actionReceivedGamePlayers);
        EventManager.StartListening("PLAYER_JOINED", this.actionReceivedPlayerJoined);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Auth());
    }
    IEnumerator Auth()
    {
        yield return new WaitUntil(() => WSConnection.init);
        string playerName = Utilities.GenerateName(4, 10);
        WSConnection.SendMessage("auth", new AuthMessage(playerName));
    }
    void ReceivedAuth(string data) {
        NetworkRemotePlayersController.SetLocalPlayer(JsonUtility.FromJson<Player>(data));
    }

    void ReceivedGamePlayers(string data) {
        PlayerList playerList = JsonUtility.FromJson<PlayerList>(data);
        foreach (Player player in playerList.players) {
            NetworkRemotePlayersController.AddPlayer(player);
        }
    }

	void ReceivedPlayerJoined(string data) {
        Debug.Log("ReceivedPlayerJoined");
    	NetworkRemotePlayersController.AddPlayer(JsonUtility.FromJson<Player>(data));
	}
}
