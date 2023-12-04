using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Choice_click : MonoBehaviour
{
    public Button mychoicebutton;
    public Button yes_btn;
    public Button No_btn;
    public GameObject areusure_panel;
    public GameObject questionpanel;
    public GameObject parentGameObject;
    public TextMeshProUGUI choice_invest;
    public TextMeshProUGUI invest_to_copy;
    public TextMeshProUGUI Title;
    public Button mychoicebutton2;
    public Button mychoicebutton3;

    private bool isPanelOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        areusure_panel.SetActive(false);
        foreach (Transform child in parentGameObject.transform)
        {
            child.gameObject.SetActive(false);
        }

        mychoicebutton.onClick.AddListener(ToggleVisibility);
        yes_btn.onClick.AddListener(ConfirmChoice);
        No_btn.onClick.AddListener(CancelChoice);
    }

    void ToggleVisibility()
    {
        if (!isPanelOpen)
        {
            if (questionpanel.activeSelf)
            {
                questionpanel.SetActive(false);
            }

            areusure_panel.SetActive(true);
            choice_invest.text = invest_to_copy.text;
            Debug.Log("Choice to invest: " + choice_invest.text);

            mychoicebutton2.interactable = false;
            mychoicebutton3.interactable = false;
        }
        else
        {
            // The areusure_panel is already open, do nothing or handle differently if needed
        }

        isPanelOpen = true;
    }

    void ConfirmChoice()
    {
        string objectNameToFind = choice_invest.text;
        GameObject inactiveObject = GameObject.Find(objectNameToFind);

        if (inactiveObject != null)
        {
            Debug.Log(objectNameToFind + " found, even though it's inactive");
            inactiveObject.SetActive(true); // Set it to active if needed
        }
        else
        {
            Debug.LogError(objectNameToFind + " not found");
        }

        areusure_panel.SetActive(false);

        string buttonName = Title.text + "_btn";

        Button titleButton = GameObject.Find(buttonName).GetComponent<Button>();
        titleButton.interactable = false;
        Color buttonColor = titleButton.image.color;
        buttonColor.a = 0.5f;
        titleButton.image.color = buttonColor;

        questionpanel.SetActive(true);

        mychoicebutton2.interactable = true;
        mychoicebutton3.interactable = true;

        isPanelOpen = false;
    }

    void CancelChoice()
    {
        areusure_panel.SetActive(false);

        // If needed, add any additional logic for canceling the choice

        questionpanel.SetActive(true);

        mychoicebutton2.interactable = true;
        mychoicebutton3.interactable = true;

        isPanelOpen = false;
    }
}
