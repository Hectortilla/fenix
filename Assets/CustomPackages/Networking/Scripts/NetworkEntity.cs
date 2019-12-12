using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetworkEntity : MonoBehaviour
{
	string name;
	
	List<Player> players;

    private Action<string> actionReceivedAuth;
    private Action<string> actionReceivedGamePlayers;
    private Action<string> actionReceivedPlayerJoined;

    void Awake () {
        this.actionReceivedAuth = new Action<string>(this.ReceivedAuth);
        this.actionReceivedGamePlayers = new Action<string>(this.ReceivedGamePlayers);
        this.actionReceivedPlayerJoined = new Action<string>(this.ReceivedPlayerJoined);
    }

    void OnEnable () {
        EventManager.StartListening("AUTH", this.actionReceivedAuth);
        EventManager.StartListening("GAME_PLAYERS", this.actionReceivedGamePlayers);
        EventManager.StartListening("PLAYER_JOINED", this.actionReceivedPlayerJoined);
    }
    // Start is called before the first frame update
    void Start()
    {
        name = Utilities.GenerateName(4, 10);
        StartCoroutine(Auth());
    }
    IEnumerator Auth()
    {
        yield return new WaitUntil(() => WSConnection.init);
        WSConnection.SendMessage(JsonUtility.ToJson(new AuthMessage(name)));
    }
    void ReceivedAuth(string data) {
        Debug.Log("Authenticated as " + name + "!");
    }

    void ReceivedGamePlayers(string data) {
    	players = JsonUtility.FromJson<PlayerList>(data).players;
    }

	void ReceivedPlayerJoined(string data) {
    	players.Add(JsonUtility.FromJson<Player>(data));

	}
}

[System.Serializable]
public class AuthMessage
{
    public string action = "auth";
    public string name;
    public AuthMessage (string _name) {
    	name = _name;
    }
}

[System.Serializable]
public class Player
{
    public string key;
    public string name;
}


[System.Serializable]
public class PlayerList
{
	public List<Player> players;
}
