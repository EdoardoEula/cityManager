using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMovement : MonoBehaviour

{ 
    public float speed;
    void Update()
    {
        transform.Translate(Vector3.left*speed* Time.deltaTime);

        if (transform.position.x <-28f)
        {
            transform.position = new Vector2(33.06f, transform.position.y);
        }
    }
}
