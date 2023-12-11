using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMoving : MonoBehaviour
{
    public float increment;
    public float speed;
    private int count = 0;
    private Vector2 targetPos;
    public TextMeshProUGUI countText;
    public float maxY = 5f; // Limite massimo sull'asse Y
    public float minY = -5f; // Limite minimo sull'asse Y

    private void Awake()
    {
        // Nessuna necessità di impostare targetPos qui, viene già inizializzata automaticamente a (0,0)
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

        // Limita la posizione sull'asse Y
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        // Muovi verso la posizione target
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
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Money: " + count.ToString();
    }
}