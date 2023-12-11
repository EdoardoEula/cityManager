using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterInteractionresidence : MonoBehaviour
{
    
    public CharacterMovementresidence otherCharacterresidenze;
    public GameObject iconArrowresidence;
    public GameObject canvasRes;
    private CSVReader.DialogueList dialogueList;
    private int character = 2;  // Set the default character value to change with GameManager.personalization
    private string currentCharacter;
    private string currentLevel = "Residential";
    private int currentSentenceIndex;
    private bool isSkipButtonClickable = true;

    public Button continueButton;
    public Button skipButton;
    public bool isStopped = false; // Add this line to declare the isStopping variable

    void Start()
    {
        CSVReader csvReader = FindObjectOfType<CSVReader>();
        if (csvReader != null)
        {
            dialogueList = csvReader.myDialogueList;
        }
        else
        {
            Debug.LogError("CSVReader component not found in the scene.");
        }

        // Assicurati che l'icona sia attiva all'inizio (se necessario)
        iconArrowresidence.SetActive(true);
        if (canvasRes != null)
        {
            canvasRes.GetComponent<Canvas>().enabled = false;
        }

        isStopped = true;

        switch (character)
        {
            case 1:
                currentSentenceIndex = 38;
                currentCharacter = "Entrepreneur";
                break;
            case 2:
                currentSentenceIndex = 42;  // Modify this line based on the starting index for the Environmentalist
                currentCharacter = "Environmentalist";
                break;
            case 3:
                currentSentenceIndex = 46;  // Modify this line based on the starting index for the Equilibrist
                currentCharacter = "Equilibrist";
                break;
            default:
                // Handle the default case or return if needed
                return;
        }
        
        Debug.Log($"Character: {character}, Current Character: {currentCharacter}, Index: {currentSentenceIndex}");

        continueButton.onClick.AddListener(OnContinueButtonClick);
        skipButton.onClick.AddListener(OnSkipButtonClick);
    }
     void OnContinueButtonClick()
    {
        currentSentenceIndex++;

        if (CurrentDialogueExists())
        {
            FindObjectOfType<DialogueManagerRes>().SetShouldSkipSentence(false);
            FindObjectOfType<DialogueManagerRes>().ResetSkipButtonClicked();
            TriggerDialogue();
        }
        else
        {
            FindObjectOfType<DialogueManagerRes>().EndDialogue();
        }
    }

void OnSkipButtonClick()
    {
        if (isSkipButtonClickable)
        {
            isSkipButtonClickable = false;
            FindObjectOfType<DialogueManagerRes>().ToggleSkipEnabled();
            FindObjectOfType<DialogueManagerRes>().SetShouldSkipSentence(true);
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
            FindObjectOfType<DialogueManagerRes>().EndDialogue();
        }
    }

    public void EnableSkipButton()
    {
        isSkipButtonClickable = true; // Re-enable the skip button
    }

    IEnumerator DisappearAndActivateCanvas()
    {
        yield return null;
        yield return new WaitForSeconds(1.0f);

        if (canvasRes != null)
        {
            canvasRes.GetComponent<Canvas>().enabled = true;
        }
    }

    bool IsObjectValid(GameObject obj)
    {
        return obj != null && obj.activeSelf;
    }


    void TriggerDialogue()
    {
        // Trigger the dialogue
        if (dialogueList != null && CurrentDialogueExists())
        {
            DialogueManagerRes dialogueManager = FindObjectOfType<DialogueManagerRes>();
            if (dialogueManager != null)
            {
                dialogueManager.StartDialogue(GetCurrentDialogue());
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
            FindObjectOfType<DialogueManagerRes>().EndDialogue();
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
        FindObjectOfType<DialogueManagerRes>().EndDialogue();
    }

    bool CurrentDialogueExists()
    {
        return dialogueList != null &&
               currentSentenceIndex <= dialogueList.dialogue.Length &&
               dialogueList.dialogue[currentSentenceIndex - 1].character == currentCharacter &&
               dialogueList.dialogue[currentSentenceIndex - 1].level == currentLevel;
    }
    CSVReader.Dialogue GetCurrentDialogue()
    {
        return dialogueList.dialogue[currentSentenceIndex - 1];
    }


    void OnMouseDown()
    {
        otherCharacterresidenze.CallCharacterresidence();
        
        if (iconArrowresidence != null)
        {
            iconArrowresidence.SetActive(false);
        }
    }
    public void StartDialogueAndEnableCanvas()
    {
        // Trigger the dialogue and enable the canvas
        DisplayNextSentence();
        StartCoroutine(DisappearAndActivateCanvas());
    }
}
