﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkEntity : MonoBehaviour
{
	Player mySlef;
	
    private Action<string> actionReceivedAuth;
    private Action<string> actionReceivedGamePlayers;
    private Action<string> actionReceivedPlayerJoined;
    private Action<string> actionReceivedPlayerLeft;
    private Action<string> actionReceivedGameJoined;

    void Awake () {
        this.actionReceivedAuth = new Action<string>(this.ReceivedAuth);
        this.actionReceivedGamePlayers = new Action<string>(this.ReceivedGamePlayers);
        this.actionReceivedPlayerJoined = new Action<string>(this.ReceivedPlayerJoined);
        this.actionReceivedPlayerLeft = new Action<string>(this.ReceivedPlayerLeft);
        this.actionReceivedGameJoined = new Action<string>(this.ReceivedGameJoined);
    }

    void OnEnable () {
        EventManager.StartListening("AUTH_PLAYER", this.actionReceivedAuth);
        EventManager.StartListening("GAME_PLAYERS", this.actionReceivedGamePlayers);
        EventManager.StartListening("PLAYER_JOINED", this.actionReceivedPlayerJoined);
        EventManager.StartListening("PLAYER_LEFT", this.actionReceivedPlayerLeft);
        EventManager.StartListening("GAME_JOINED", this.actionReceivedGameJoined);
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
    	NetworkRemotePlayersController.AddPlayer(JsonUtility.FromJson<Player>(data));
	}

    void ReceivedPlayerLeft(string data) {
    	NetworkRemotePlayersController.RemovePlayer(JsonUtility.FromJson<Player>(data));
	}
    void ReceivedGameJoined(string data) {
    	EventManager.TriggerEvent("UI:GAME_KEY", JsonUtility.FromJson<Game>(data).key);
	}
}
