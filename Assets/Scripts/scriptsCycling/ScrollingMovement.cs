using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingMovement : MonoBehaviour

{ 
    public float speed;
    void Update()
    {
        transform.Translate(Vector3.left*speed* Time.deltaTime);

        if (transform.position.x <-25f)
        {
            transform.position = new Vector2(30.06f, transform.position.y);
        }
    }
}
