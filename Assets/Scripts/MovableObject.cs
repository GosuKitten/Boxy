using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour {

    LerpMovement lerpMovement;
    GameManager gameManager;
    Transform currentGoal;

    public delegate void SatisfyGoal(Transform goal);
    public static event SatisfyGoal OnSatisfyGoal;

    public delegate void UnsatisfyGoal(Transform goal);
    public static event UnsatisfyGoal OnUnsatisfyGoal;

    void Start()
    {
        lerpMovement = GetComponent<LerpMovement>();
        lerpMovement.OnMovementCompleted += LerpMovement_OnMovementCompleted;

        gameManager = GameManager.instance;
    }

    public bool AttemptPush(Vector3 direction)
    {
        if (!lerpMovement.isMoving)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1);
            if (hit.collider == null)
            {
                lerpMovement.MoveBy(direction);

                if (currentGoal != null)
                    if (OnUnsatisfyGoal != null)
                        OnUnsatisfyGoal(currentGoal);

                return true;
            }
        }
        return false;
    }

    private void LerpMovement_OnMovementCompleted(Vector3 currentLocation)
    {
        foreach (Transform t in gameManager.unsatisfiedGoals)
        {
            if (t.position == transform.position)
            {
                currentGoal = t;
                if (OnSatisfyGoal != null)
                    OnSatisfyGoal(t);
                break;
            }
        }
    }
}
