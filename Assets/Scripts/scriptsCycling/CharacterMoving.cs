using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMoving : MonoBehaviour
{
    public float increment;
    public float speed;
    private int count=0;
    private Vector2 targetPos;
    public TextMeshProUGUI countText;

    private void Awake()
    {
        // No need to set targetPos here, it's automatically initialized to (0,0)
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("MoveDown");
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("MoveUp");
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
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Collectible")) 
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }
    void SetCountText()
    {
        // Update the count text with the current count.
        countText.text = " " + count.ToString();
    }
}

  
