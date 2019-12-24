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
    public float speed = 0.1F;  // seconds to move.
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
        if (targetPosition != finalPosition) {
            finalPosition = targetPosition;
            startPosition = transform.position;
            startTime = Time.time;
        }
        float fractionOfJourney = (Time.time - startTime) / speed;
        transform.position = Vector3.Lerp(startPosition, finalPosition, fractionOfJourney);
    }

}
