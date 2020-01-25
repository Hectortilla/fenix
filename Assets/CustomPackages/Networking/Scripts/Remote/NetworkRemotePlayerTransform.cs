using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkRemotePlayerTransform : MonoBehaviour
{
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 targetPosition;
    Quaternion targetRotation;

    float startTimePosition;
    float startTimeRotation;

    float interpolateSpeed = 0.01F; // 0.075F // seconds to move.

    void OnValidate() {
        interpolateSpeed = Mathf.Max(interpolateSpeed, 0.01F);
    }

    void Start()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }
    void Update() {
        InterpolatePosition();
        InterpolateRotation();
    }
    public void SetPosition(Vector3 pos){
        if (pos != targetPosition) {
            targetPosition = pos;
            startPosition = transform.position;
            startTimePosition = Time.time;
        }
    }
    public void SetRotation(Quaternion rot){
        if (rot != targetRotation) {
            targetRotation = rot;
            startRotation = transform.rotation;
            startTimeRotation = Time.time;
        }
    }
    void InterpolatePosition() {
        float fractionOfJourney = (Time.time - startTimePosition) / interpolateSpeed;
        transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
    }
    void InterpolateRotation() {
        float fractionOfJourney = (Time.time - startTimeRotation) / interpolateSpeed;
        transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fractionOfJourney);
    }

}
