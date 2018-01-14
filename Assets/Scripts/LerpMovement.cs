using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Transform))]
public class LerpMovement : MonoBehaviour {

    [SerializeField]
    AnimationCurve lerpCurve;

    [SerializeField]
    float lerpTime = 1;
    float currentLerpTime;
    float progress;
    float curvedProgress;
    public bool isMoving;
    Vector3 startPosition;
    Vector3 endPosition;

    public delegate void MovementCompleted(Vector3 currentLocation);
    public event MovementCompleted OnMovementCompleted;

    public void MoveTo(Vector3 destination)
    {
        currentLerpTime = 0;
        isMoving = true;
        startPosition = transform.position;
        endPosition = destination;
    }

    public void MoveBy(Vector3 vector)
    {
        currentLerpTime = 0;
        isMoving = true;
        startPosition = transform.position;
        endPosition = transform.position + vector;
    }

	// Update is called once per frame
	void Update () {
        if (isMoving)
        {
            if (currentLerpTime < lerpTime)
            {
                currentLerpTime += Time.deltaTime;
                progress = Mathf.Min(currentLerpTime / lerpTime, 1);
                curvedProgress = lerpCurve.Evaluate(progress);
                transform.position = Vector3.Lerp(startPosition, endPosition, curvedProgress);
            }
            else
            {
                isMoving = false;
                transform.position = Vector3.Lerp(startPosition, endPosition, curvedProgress);
                if (OnMovementCompleted != null)
                    OnMovementCompleted(transform.position);
            }
        }
	}
}
