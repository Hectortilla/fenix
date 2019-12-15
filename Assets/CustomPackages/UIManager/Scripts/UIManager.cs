using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	Text name;
	[SerializeField]
	Text gameKey;
	[SerializeField]
	Text players;
    
    Action<string> actionName;
    Action<string> actionGameKey;
    Action<string> actionPlayers;

    void Awake () {
        this.actionName = new Action<string>(this.listenerName);
        this.actionGameKey = new Action<string>(this.listenerGameKey);
        this.actionPlayers = new Action<string>(this.listenerPlayers);
    }

    void OnEnable () {
        EventManager.StartListening("UI:NAME", this.actionName);
        EventManager.StartListening("UI:GAME_KEY", this.actionGameKey);
        EventManager.StartListening("UI:PLAYERS", this.actionPlayers);
    }
    void listenerName(string _name) {
        name.text = "Name: " + _name;
    }
    void listenerPlayers(string _players) {
        players.text = "Players: " + _players;
    }
    void listenerGameKey(string _gameKey) {
        gameKey.text = "Game: " + _gameKey;
    }
}
