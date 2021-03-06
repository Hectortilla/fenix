﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class NetworkMessage
{
    abstract public string action { get; }
}

[System.Serializable]
public class DisconnectMessage : NetworkMessage {
    public override string action
    {
        get { return "disconnect"; }
    }
}

// Outgoing

[System.Serializable]
public class AuthMessage : NetworkMessage {
    public override string action
    {
        get { return "auth"; }
    }
    public string name;

    public AuthMessage (string _name) {
    	name = _name;
    }
}

[System.Serializable]
public class Ping : NetworkMessage {
    public override string action
    {
        get { return "ping"; }
    }
}


[System.Serializable]
public class TransformMessage : NetworkMessage {
    public override string action
    {
        get { return "move"; }
    }

    public float px;
    public float py;
    public float pz;

    public float rx;
    public float ry;
    public float rz;

    public TransformMessage(float _px, float _py, float _pz, float _rx, float _ry, float _rz) {
        px = _px;
        py = _py;
        pz = _pz;
        rx = _rx;
        ry = _ry;
        rz = _rz;
    }
}


// Incoming

[System.Serializable]
public class ResponsePing {
    public string message;
}

// ->
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
// <-


// ->
[System.Serializable]
public class PlayerTransform {
    public string key;

    public float px;
    public float py;
    public float pz;

    public float rx;
    public float ry;
    public float rz;
}

// <-

[System.Serializable]
public class Game {
    public string key;
}

// CONNECTION


[System.Serializable]
public class IncomingNetworkMessage {
    public int code;
    public string action;
    public string data;
}


[System.Serializable]
public class OutgoingNetworkMessage {
    public string action;
    public string data;

    public OutgoingNetworkMessage(string _action, string _data){
        action = _action;
        data = _data;
    }
}
