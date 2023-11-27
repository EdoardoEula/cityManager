using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Choice_click : MonoBehaviour
{
    public Button mychoicebutton;
    public GameObject areusure_panel;
    private bool isPanelOpen = false;
    public GameObject questionpanel; 
    public Button No_btn;
    public Button mychoicebutton2;
    public Button mychoicebutton3;
    public TextMeshProUGUI choice_invest; 
    public TextMeshProUGUI invest_to_copy;
    
    // Start is called before the first frame update
    void Start()
    {
        areusure_panel.SetActive(false);
        mychoicebutton.onClick.AddListener(ToggleVisibility);
        No_btn.onClick.AddListener(CloseAreYouSurePanel);
    }

    void ToggleVisibility()
    {
        if (!isPanelOpen)
        {
            // Close questionpanel if it is open
            if (questionpanel.activeSelf)
            {
                questionpanel.SetActive(false);
            }

            areusure_panel.SetActive(true);
            choice_invest.text = invest_to_copy.text; // Copy text from invest_to_copy to choice_invest

            // Set mychoicebutton2 and mychoicebutton3 inactive
            mychoicebutton2.interactable = false;
            mychoicebutton3.interactable = false;
        }
        else
        {
            // The areusure_panel is already open, do nothing or handle differently if needed
        }

        isPanelOpen = true;
    }

    void CloseAreYouSurePanel()
    {
        areusure_panel.SetActive(false);

        // Open questionpanel
        questionpanel.SetActive(true);

        // Set mychoicebutton2 and mychoicebutton3 active
        mychoicebutton2.interactable = true;
        mychoicebutton3.interactable = true;

        isPanelOpen = false;
    }
}
