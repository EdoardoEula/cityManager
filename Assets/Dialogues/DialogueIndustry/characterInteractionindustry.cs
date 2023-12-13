using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteractionindustry : MonoBehaviour
{
    
    public CharacterMovementindustry otherCharacterindustry;
    public GameObject iconArrowindustry;
    public GameObject canvasIndustry;
    private CSVReader.DialogueList dialogueList;
    private int? character = GameManager.personalization;  // Set the default character value to change with GameManager.personalization
    private string currentCharacter;
    private string currentLevel = "Industries";
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
        iconArrowindustry.SetActive(true);
        if (canvasIndustry != null)
        {
            canvasIndustry.GetComponent<Canvas>().enabled = false;
        }

        isStopped = true;

        switch (character)
        {
            case 1:
                currentSentenceIndex = 14;
                currentCharacter = "Entrepreneur";
                break;
            case 2:
                currentSentenceIndex = 18;  // Modify this line based on the starting index for the Environmentalist
                currentCharacter = "Environmentalist";
                break;
            case 3:
                currentSentenceIndex = 22;  // Modify this line based on the starting index for the Equilibrist
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
            FindObjectOfType<DialogueManagerInd>().SetShouldSkipSentence(false);
            FindObjectOfType<DialogueManagerInd>().ResetSkipButtonClicked();
            TriggerDialogue();
        }
        else
        {
            FindObjectOfType<DialogueManagerInd>().EndDialogue();
        }
    }

void OnSkipButtonClick()
    {
        if (isSkipButtonClickable)
        {
            isSkipButtonClickable = false;
            FindObjectOfType<DialogueManagerInd>().ToggleSkipEnabled();
            FindObjectOfType<DialogueManagerInd>().SetShouldSkipSentence(true);
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
            FindObjectOfType<DialogueManagerInd>().EndDialogue();
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

        if (canvasIndustry != null)
        {
            canvasIndustry.GetComponent<Canvas>().enabled = true;
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
            DialogueManagerInd dialogueManager = FindObjectOfType<DialogueManagerInd>();
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
            FindObjectOfType<DialogueManagerInd>().EndDialogue();
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
        FindObjectOfType<DialogueManagerInd>().EndDialogue();
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
        otherCharacterindustry.CallCharacterindustry();
        
        if (iconArrowindustry != null)
        {
            iconArrowindustry.SetActive(false);
        }
    }
    public void StartDialogueAndEnableCanvas()
    {
        // Trigger the dialogue and enable the canvas
        DisplayNextSentence();
        StartCoroutine(DisappearAndActivateCanvas());
    }
}
