using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkEntity : MonoBehaviour
{
	public bool isAuth = false;

    private Action<string> actionReceivedAuth;
    private Action<string> actionReceivedJoinedGame;

    void Awake () {
        this.actionReceivedAuth = new Action<string>(this.ReceivedAuth);
        this.actionReceivedJoinedGame = new Action<string>(this.ReceivedJoinedGame);
    }

    void OnEnable () {
        EventManager.StartListening("AUTH_PLAYER", this.actionReceivedAuth);
        EventManager.StartListening("JOINED_GAME", this.actionReceivedJoinedGame);
    }

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

    void ReceivedJoinedGame(string data) {
    	EventManager.TriggerEvent("UI:GAME_KEY", JsonUtility.FromJson<Game>(data).key);
	}
}
