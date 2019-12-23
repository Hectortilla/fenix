using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRemotePlayerTransform : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 startRotation;

    public Vector3 targetPosition;
    public Vector3 targetRotation;

    public Vector3 finalPosition;
    public Vector3 finalRotation;

    private float startTime;
    private float journeyLength;
    [SerializeField]
    public float speed = 100.0F;  // units per second.
    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation.eulerAngles;
        finalPosition = transform.position;
        finalRotation = transform.rotation.eulerAngles;
    }
    void Update() {
        InterpolateTransform();
    }

    void InterpolateTransform() {
        if (Vector3.Distance(targetPosition, finalPosition) > 0.5f) {
            Debug.Log(1);
            finalPosition = targetPosition;
            startPosition = transform.position;
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPosition, finalPosition);
        }
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, finalPosition, fractionOfJourney);
    }

}
