using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMoving : MonoBehaviour
{
    public float increment;
    public float speed;
    private Vector2 targetPos;

    private void Awake()
    {
        // No need to set targetPos here, it's automatically initialized to (0,0)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void MoveUp()
    {
        targetPos += new Vector2(0, increment);
    }

    public void MoveDown()
    {
        targetPos -= new Vector2(0, increment);
    }
}

  
