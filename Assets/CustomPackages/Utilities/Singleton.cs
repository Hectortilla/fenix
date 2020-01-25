using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    public static T instance { get; private set; }

    protected virtual void Awake() {
        if (instance != null && instance != this)
        {
            Debug.LogError(
                "Duplicate singleton subclass of type " +
                typeof(T) + "! eliminating " + name + " while preserving " +
                instance.name
            );

            Destroy(gameObject);
        } else {
            instance = this as T;
        }
    }
    public virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
