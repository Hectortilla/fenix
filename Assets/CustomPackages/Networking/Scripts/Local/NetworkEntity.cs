using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkEntity : MonoBehaviour
{
	Player mySlef;
	public bool isAuth = false;

    private Action<string> actionReceivedAuth;
    private Action<string> actionReceivedGamePlayers;
    private Action<string> actionReceivedPlayerJoined;
    private Action<string> actionReceivedPlayerLeft;
    private Action<string> actionReceivedJoinedGame;

    void Awake () {
        this.actionReceivedAuth = new Action<string>(this.ReceivedAuth);
        this.actionReceivedGamePlayers = new Action<string>(this.ReceivedGamePlayers);
        this.actionReceivedPlayerJoined = new Action<string>(this.ReceivedPlayerJoined);
        this.actionReceivedPlayerLeft = new Action<string>(this.ReceivedPlayerLeft);
        this.actionReceivedJoinedGame = new Action<string>(this.ReceivedJoinedGame);
    }

    void OnEnable () {
        EventManager.StartListening("AUTH_PLAYER", this.actionReceivedAuth);
        EventManager.StartListening("GAME_PLAYERS", this.actionReceivedGamePlayers);
        EventManager.StartListening("PLAYER_JOINED", this.actionReceivedPlayerJoined);
        EventManager.StartListening("PLAYER_LEFT", this.actionReceivedPlayerLeft);
        EventManager.StartListening("JOINED_GAME", this.actionReceivedJoinedGame);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Auth());
    }
    IEnumerator Auth()
    {
        yield return new WaitUntil(() => UDPConnection.init);
        string playerName = NameGenerator.GenerateName(4, 10);
        UDPConnection.Send(new AuthMessage(playerName));
    }
    void ReceivedAuth(string data) {
        NetworkRemotePlayersController.SetLocalPlayer(JsonUtility.FromJson<Player>(data));
        isAuth = true;
    }

    void ReceivedGamePlayers(string data) {
        PlayerList playerList = JsonUtility.FromJson<PlayerList>(data);
        foreach (Player player in playerList.players) {
            NetworkRemotePlayersController.AddPlayer(player);
        }
    }

	void ReceivedPlayerJoined(string data) {
	    NetworkRemotePlayersController.AddPlayer(JsonUtility.FromJson<Player>(data));
	}

    void ReceivedPlayerLeft(string data) {
    	NetworkRemotePlayersController.RemovePlayer(JsonUtility.FromJson<Player>(data));
	}
    void ReceivedJoinedGame(string data) {
    	EventManager.TriggerEvent("UI:GAME_KEY", JsonUtility.FromJson<Game>(data).key);
	}
}
