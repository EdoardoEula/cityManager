using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnCollision : MonoBehaviour
{
    public float speed;
    void UpDate()
    {
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }
    // This method is called when another collider enters the trigger of this object
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Projectile"))
        {
            Debug.Log("collided with the player");
            Destroy(gameObject);
        }
        
        
    }
}
