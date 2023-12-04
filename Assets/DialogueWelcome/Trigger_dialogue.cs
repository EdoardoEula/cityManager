using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine;
using UnityEngine.UI;

public class Trigger_dialogue : MonoBehaviour
{
    public Button continueButton;
    public Button skipButton;
    public Button btn_1;
    public Button btn_2;
    public Button btn_3;

    private CSVReader_welcome.DialogueList dialogueList;
    private int currentSentenceIndex = 1;
    private bool isSkipButtonClickable = true;

    // Start is called before the first frame update
    void Start()
    {
        CSVReader_welcome csvReader = FindObjectOfType<CSVReader_welcome>();
        if (csvReader != null)
        {
            dialogueList = csvReader.myDialogueList;
        }
        else
        {
            Debug.LogError("CSVReader component not found in the scene.");
        }

        // Set currentSentenceIndex to 0 to start from the beginning
        currentSentenceIndex = 0;

        btn_1.GameObject().SetActive(false);
        btn_2.GameObject().SetActive(false);
        btn_3.GameObject().SetActive(false);

        TriggerDialogue();

        continueButton.onClick.AddListener(OnContinueButtonClick);
        skipButton.onClick.AddListener(OnSkipButtonClick);
    }

    void SetButtonAlpha(Button button, float alpha)
    {
        if (button != null)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, alpha);
            }
        }
    }

    void OnContinueButtonClick()
    {
        currentSentenceIndex++;

        if (CurrentDialogueExists())
        {
            FindObjectOfType<DialogueManager>().SetShouldSkipSentence(false);
            FindObjectOfType<DialogueManager>().ResetSkipButtonClicked();
            TriggerDialogue();
        }
        else
        {
            FindObjectOfType<DialogueManager>().EndDialogue();
        }
    }

    void OnSkipButtonClick()
    {
        if (isSkipButtonClickable)
        {
            isSkipButtonClickable = false;
            FindObjectOfType<DialogueManager_welcome>().ToggleSkipEnabled();
            FindObjectOfType<DialogueManager_welcome>().SetShouldSkipSentence(true);
            DisplayCurrentSentence();
            StartCoroutine(EnableSkipButtonAfterDelay());
        }
    }

    IEnumerator EnableSkipButtonAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isSkipButtonClickable = true;
        EnableContinueButton();
    }

    void EnableContinueButton()
    {
        // Enable the ContinueButton after the delay
        continueButton.interactable = true;
    }

    void DisplayCurrentSentence()
    {
        if (CurrentDialogueExists())
        {
            TriggerDialogue();
        }
        else
        {
            FindObjectOfType<DialogueManager_welcome>().EndDialogue();
        }
    }

    public void EnableSkipButton()
    {
        isSkipButtonClickable = true; // Re-enable the skip button
    }

    void TriggerDialogue()
    {
        // Trigger the dialogue
        if (dialogueList != null && CurrentDialogueExists())
        {
            DialogueManager_welcome dialogueManager = FindObjectOfType<DialogueManager_welcome>();
            if (dialogueManager != null)
            {
                CSVReader_welcome.Dialogue currentDialogue = GetCurrentDialogue();
                dialogueManager.StartDialogue(currentDialogue);
                QuestionButtons(); // Call QuestionButtons after triggering the dialogue
            }
            else
            {
                Debug.LogError("DialogueManager not found in the scene.");
            }
        }
        else
        {
            Debug.Log("No more dialogue data.");
            // If there are no more sentences, end the dialogue
            FindObjectOfType<DialogueManager_welcome>().EndDialogue();
        }
    }

    void QuestionButtons()
    {
        Image buttonImage1 = btn_1.GetComponent<Image>();
        Image buttonImage2 = btn_2.GetComponent<Image>();
        Image buttonImage3 = btn_3.GetComponent<Image>();
        CSVReader_welcome.Dialogue currentDialogue = GetCurrentDialogue();
        Debug.Log(currentDialogue.label);
        if (currentDialogue.label == 1)
        {
            Debug.Log("Label is 1. Showing buttons with fade effect.");
            btn_1.GameObject().SetActive(true);
            btn_2.GameObject().SetActive(true);
            btn_3.GameObject().SetActive(true);
            
        }
        else
        {
            Debug.Log("Label is not 1. Hiding buttons.");
            btn_1.GameObject().SetActive(false);
            btn_2.GameObject().SetActive(false);
            btn_3.GameObject().SetActive(false);
            
        }
        btn_1.onClick.AddListener(() =>updateChoice(currentDialogue.numsentence, 1));
        btn_2.onClick.AddListener(() =>updateChoice(currentDialogue.numsentence, 2));
        btn_3.onClick.AddListener(() =>updateChoice(currentDialogue.numsentence, 3));
    }

    void updateChoice(string currentQuestion, int btn_number)
    {
        switch (currentQuestion)
        {
            case "4":
                GameManager.choice_q1 = btn_number;
                return;
            case "5":
                GameManager.choice_q2 = btn_number;
                return;
            case "6":
                GameManager.choice_q3 = btn_number;
                return;
        }
    }
    
    void DisplayNextSentence()
    {
        // Check if there are more sentences for the current character, level, and sentence index
        while (CurrentDialogueExists())
        {
            // Trigger the next dialogue
            TriggerDialogue();
            return;
        }

        // If there are no more sentences, end the dialogue
        FindObjectOfType<DialogueManager_welcome>().EndDialogue();
    }

    bool CurrentDialogueExists()
    {
        return dialogueList != null &&
               currentSentenceIndex <= dialogueList.dialogue.Length;
    }

    CSVReader_welcome.Dialogue GetCurrentDialogue()
    {
        return dialogueList.dialogue[currentSentenceIndex];
    }
}