using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    bool satisfied = false;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    Sprite[] sprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Satisfy()
    {
        satisfied = true;
        spriteRenderer.sprite = sprites[1];
    }

    void Unsatisfy()
    {
        satisfied = false;
        spriteRenderer.sprite = sprites[0];
    }
}
