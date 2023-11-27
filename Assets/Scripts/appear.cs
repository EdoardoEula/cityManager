using UnityEngine;
using UnityEngine.UI;

public class appear : MonoBehaviour
{
    public Button appearButton; // Reference to the button in the Unity Editor

    void Start()
    {
        gameObject.SetActive(false);

        // Check if the button reference is not null before using it
        if (appearButton != null)
        {
            // Add a listener to the button's click event, calling MakeObjectAppear when clicked
            appearButton.onClick.AddListener(MakeObjectAppear);
        }
        else
        {
            Debug.LogError("Button reference is not set in the Unity Editor.");
        }
    }

    // Function to make the object appear
    public void MakeObjectAppear()
    {
        // Set the object's visibility to true
        gameObject.SetActive(true);
    }
}