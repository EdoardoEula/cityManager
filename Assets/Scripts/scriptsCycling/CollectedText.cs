using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public Text collectedText; // Reference to the UI Text element
    private int collectedCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            // Assuming "Collectible" is the tag of the objects you want to collect
            Collect(other.gameObject); // Pass the game object to the Collect method
        }
    }

    private void Collect(GameObject collectibleObject)
    {
        collectedCount++;
        UpdateCollectedText();

        // Destroy the collectible object after a short delay
        Destroy(collectibleObject, 0.1f);
    }

    private void UpdateCollectedText()
    {
        if (collectedText != null)
        {
            collectedText.text = "Collected: " + collectedCount;
        }
    }
}

