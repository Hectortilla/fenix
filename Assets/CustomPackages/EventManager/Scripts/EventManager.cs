using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    // Singleton pattern ------- >    
    private static EventManager _instance;

    public static EventManager Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    private static Dictionary<string, Action<string>> eventDictionary;
    static EventManager() {
        EventManager.Init();
    }
    // < -------------------------

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
        if (EventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            EventManager.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            EventManager.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<string> listener)
    {
        Action<string> thisEvent;
        if (EventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            EventManager.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, string param)
    {
        Action<string> thisEvent = null;
        if (EventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
            // OR USE  EventManager.eventDictionary[eventName](eventParam);
        }
    }
}
