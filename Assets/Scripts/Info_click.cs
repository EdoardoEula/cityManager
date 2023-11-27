using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Info_click : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI info_title;   // Reference to the Text component for drag and drop
    public TextMeshProUGUI titletocopy;
    public TextMeshProUGUI info_description;
    public string description_input;
    public GameObject infoPanel; // Reference to the info_panel GameObject
    private bool isPanelOpen = false;
    public Transform mytransform;
        
    
    // Start is called before the first frame update
    void Start()
    {
        myButton.onClick.AddListener(ToggleVisibility);
    }

    // Update is called once per frame
    void ToggleVisibility()
    {
        // Check if the panel is currently open
        if (isPanelOpen)
        {
            // If open, close the panel and update the flag
            infoPanel.SetActive(false);
            isPanelOpen = false;
        }
        else
        {
            // If closed, open the panel and update the flag
            infoPanel.SetActive(true);
            isPanelOpen = true;

            // Set the position of the info_panel using the transform's position
            // You can modify the position values based on your specific requirements
            infoPanel.transform.position = mytransform.position;
            info_title.text = titletocopy.text;
            info_description.text = description_input;
        }
    }
}
