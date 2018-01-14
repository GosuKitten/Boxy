using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    GameManager gameManager;
    LerpMovement lerpMovement;

	// Use this for initialization
	void Start () {
        gameManager = FindObjectOfType<GameManager>();
        lerpMovement = GetComponent<LerpMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        DetectMovement();
	}

    void DetectMovement()
    {
        if (!lerpMovement.isMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow)) { AttemptToMove(Vector3.up); }
            if (Input.GetKey(KeyCode.DownArrow)) { AttemptToMove(Vector3.down); }
            if (Input.GetKey(KeyCode.LeftArrow)) { AttemptToMove(Vector3.left); }
            if (Input.GetKey(KeyCode.RightArrow)) { AttemptToMove(Vector3.right); }
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("Level0");
        }
    }

    void AttemptToMove(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1);
        if (hit.collider == null)
        {
            lerpMovement.MoveBy(direction);
        } 
        else if (hit.collider.tag == "Movable")
        {
            var movableObject = hit.transform.GetComponent<MovableObject>();
            if (movableObject != null)
            {
                if (movableObject.AttemptPush(direction))
                {
                    lerpMovement.MoveBy(direction);
                }
            }
        }
    }
}
