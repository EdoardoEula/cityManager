using UnityEngine;
using TMPro; // Include the TextMeshPro namespace

public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
            // Set the text of the moneyText component
            if (moneyText != null)
            {
                moneyText.text = $"{GameManager.money_available}";
            }
            else
            {
                Debug.LogWarning("Money text component not assigned.");
            }
    }
    
}
