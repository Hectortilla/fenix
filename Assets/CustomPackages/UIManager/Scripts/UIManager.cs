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

    void Start () {
        actionName = new Action<string>(this.ListenerName);
        actionGameKey = new Action<string>(this.ListenerGameKey);
        actionPlayers = new Action<string>(this.ListenerPlayers);
    }
    void OnEnable () {
        EventManager.StartListening("UI:NAME", this.actionName);
        EventManager.StartListening("UI:GAME_KEY", this.actionGameKey);
        EventManager.StartListening("UI:PLAYERS", this.actionPlayers);
    }
    void OnDisable () {
        EventManager.StopListening("UI:NAME", this.actionName);
        EventManager.StopListening("UI:GAME_KEY", this.actionGameKey);
        EventManager.StopListening("UI:PLAYERS", this.actionPlayers);
    }
    void ListenerName(string _name) {
        name.text = "Name: " + _name;
    }
    void ListenerPlayers(string _players) {
        players.text = "Players: " + _players;
    }
    void ListenerGameKey(string _gameKey) {
        gameKey.text = "Game: " + _gameKey;
    }
}
