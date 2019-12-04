using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {

    private static Dictionary<string, Action<object>> eventDictionary;

    static EventManager() {
        EventManager.Init();
    }

    static void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<object>>();
        }
    }

    public static void StartListening(string eventName, Action<object> listener)
    {
        Action<object> thisEvent;
        if (EventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            EventManager.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            EventManager.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<object> listener)
    {
        Action<object> thisEvent;
        if (EventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            EventManager.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(string eventName, object param)
    {
        Action<object> thisEvent = null;
        if (EventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(param);
            // OR USE  EventManager.eventDictionary[eventName](eventParam);
        }
    }
}
