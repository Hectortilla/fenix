using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRemotePlayerTransform : MonoBehaviour
{
    public Vector3 newTargetPosition;
    public Quaternion newTargetRotation;
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 targetPosition;
    Quaternion targetRotation;

    private float startTimePosition;
    private float startTimeRotation;
    private float journeyLength;

    [SerializeField]
    public float speed = 0.075F;  // seconds to move.
    void Start()
    {
        newTargetPosition = transform.position;
        newTargetRotation = transform.rotation;
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }
    void Update() {
        InterpolatePosition();
        InterpolateRotation();
    }

    void InterpolatePosition() {
        if (newTargetPosition != targetPosition) {
            targetPosition = newTargetPosition;
            startPosition = transform.position;
            startTimePosition = Time.time;
        }
        float fractionOfJourney = (Time.time - startTimePosition) / speed;
        transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
    }
    void InterpolateRotation() {
        if (newTargetRotation != targetRotation) {
            targetRotation = newTargetRotation;
            startRotation = transform.rotation;
            startTimeRotation = Time.time;
        }
        float fractionOfJourney = (Time.time - startTimeRotation) / speed;

        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fractionOfJourney);
        // transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, fractionOfJourney));
    }

}
