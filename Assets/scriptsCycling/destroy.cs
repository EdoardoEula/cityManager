using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisappearOnCollision : MonoBehaviour
{
    public float speed;
    private int count = 0;
    public TextMeshProUGUI countText;
    void UpDate()
    {
        //transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < -20 && gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
    // This method is called when another collider enters the trigger of this object
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Projectile"))
        {
            Debug.Log("collided with the player");
            if(gameObject != null)
                Destroy(gameObject);
            count = count + 1;
            GameManager.money_available++;
            SetCountText();
        }
        
        
    }
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = "Money: " + count.ToString();

        
    }
}
