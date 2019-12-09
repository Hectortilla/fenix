using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkEventManager : MonoBehaviour {
    
    private static NetworkEventManager _instance;

    public static NetworkEventManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Update() {
        NetworkMessage msg = WSConnection.GetMessage(); // Should we make this async?
        if (msg != null) {
            NetworkEventManager.TriggerEvent(msg.action, msg.data);
        }
    }

    private static Dictionary<string, Action<string>> eventDictionary;

    static NetworkEventManager() {
        NetworkEventManager.Init();
    }

    static void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<string>>();
        }
    }

    public static void StartListening(string eventName, Action<string> listener)
    {
        Action<string> thisEvent;
        if (NetworkEventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            NetworkEventManager.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            NetworkEventManager.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<string> listener)
    {
        Action<string> thisEvent;
        if (NetworkEventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            NetworkEventManager.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, string param)
    {
        Action<string> thisEvent = null;
        if (NetworkEventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
            // OR USE  NetworkEventManager.eventDictionary[eventName](eventParam);
        }
    }
}
