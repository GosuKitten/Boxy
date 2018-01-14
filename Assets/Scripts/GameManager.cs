using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public List<Transform> unsatisfiedGoals;
    public List<Transform> satisfiedGoals;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        LevelLoader.OnLevelSpawned += LevelLoader_OnLevelSpawned;
        MovableObject.OnSatisfyGoal += MovableObject_OnSatisfyGoal;
        MovableObject.OnUnsatisfyGoal += MovableObject_OnUnsatisfyGoal;
    }

    private void LevelLoader_OnLevelSpawned()
    {
        unsatisfiedGoals = new List<Transform>();
        satisfiedGoals = new List<Transform>();

        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        foreach (GameObject go in goals)
        {
            unsatisfiedGoals.Add(go.transform);
        }
    }

    private void MovableObject_OnSatisfyGoal(Transform goal)
    {
        unsatisfiedGoals.Remove(goal);
        goal.SendMessage("Satisfy");
    }

    private void MovableObject_OnUnsatisfyGoal(Transform goal)
    {
        unsatisfiedGoals.Add(goal);
        goal.SendMessage("Unsatisfy");
    }
}
