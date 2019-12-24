using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRemotePlayerTransform : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 startRotation;

    public Vector3 newTargetPosition;
    public Vector3 newTargetRotation;

    public Vector3 targetPosition;
    public Vector3 targetRotation;

    private float startTimePosition;
    private float startTimeRotation;
    private float journeyLength;

    [SerializeField]
    public float speed = 0.1F;  // seconds to move.
    void Start()
    {
        newTargetPosition = transform.position;
        newTargetRotation = transform.rotation.eulerAngles;
        targetPosition = transform.position;
        targetRotation = transform.rotation.eulerAngles;
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
            startRotation = transform.rotation.eulerAngles;
            startTimeRotation = Time.time;
        }
        float fractionOfJourney = (Time.time - startTimeRotation) / speed;
        transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, targetRotation, fractionOfJourney));
    }

}
