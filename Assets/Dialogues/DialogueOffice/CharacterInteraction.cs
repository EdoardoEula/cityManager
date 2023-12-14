using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteraction : MonoBehaviour
{
    public CharacterMovement otherCharacteruffici;
    public GameObject iconArrowuffici;
    public GameObject canvasOffice;
    private CSVReader.DialogueList dialogueList;
    private int? character = GameManager.personalization;  // Set the default character value to change with GameManager.personalization
    private string currentCharacter;
    private string currentLevel = "Offices";
    private int currentSentenceIndex;
    private bool isSkipButtonClickable = true;

    public Button continueButton;
    public Button skipButton;
    public bool isStopped = false; // Add this line to declare the isStopping variable
    public Button farmLandButton;  // Add a reference to your FarmLand button

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
        iconArrowuffici.SetActive(true);
        if (canvasOffice != null)
        {
            canvasOffice.GetComponent<Canvas>().enabled = false;
        }

        isStopped = true;

        
        continueButton.onClick.AddListener(OnContinueButtonClick);
        skipButton.onClick.AddListener(OnSkipButtonClick);
        farmLandButton.onClick.AddListener(OnFarmLandButtonClick);  // Add this line to listen for the FarmLand button click

    }
    void OnContinueButtonClick()
    {
        currentSentenceIndex++;

        if (CurrentDialogueExists())
        {
            FindObjectOfType<DialogueManagerOff>().SetShouldSkipSentence(false);
            FindObjectOfType<DialogueManagerOff>().ResetSkipButtonClicked();
            TriggerDialogue();
        }
        else
        {
            FindObjectOfType<DialogueManagerOff>().EndDialogue();
        }
    }

    void OnFarmLandButtonClick()
    {


        switch (character)
        {
            case 1:
                currentSentenceIndex = 26;
                currentCharacter = "Entrepreneur";
                break;
            case 2:
                currentSentenceIndex = 30; // Modify this line based on the starting index for the Environmentalist
                currentCharacter = "Environmentalist";
                break;
            case 3:
                currentSentenceIndex = 34; // Modify this line based on the starting index for the Equilibrist
                currentCharacter = "Equilibrist";
                break;
            default:
                // Handle the default case or return if needed
                return;
        }

        Debug.Log($"Character: {character}, Current Character: {currentCharacter}, Index: {currentSentenceIndex}");
    }

    void OnSkipButtonClick()
    {
        if (isSkipButtonClickable)
        {
            isSkipButtonClickable = false;
            FindObjectOfType<DialogueManagerOff>().ToggleSkipEnabled();
            FindObjectOfType<DialogueManagerOff>().SetShouldSkipSentence(true);
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
            FindObjectOfType<DialogueManagerOff>().EndDialogue();
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

        if (canvasOffice != null)
        {
            canvasOffice.GetComponent<Canvas>().enabled = true;
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
            DialogueManagerOff dialogueManager = FindObjectOfType<DialogueManagerOff>();
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
            FindObjectOfType<DialogueManagerOff>().EndDialogue();
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
        FindObjectOfType<DialogueManagerOff>().EndDialogue();
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
        otherCharacteruffici.CallCharacteruffici();  // Chiamata al metodo nel CharacterMovement
        

        // Nascondi l'icona quando il personaggio viene cliccato
        if (iconArrowuffici != null)
        {
            iconArrowuffici.SetActive(false);
        }
        
    }
    public void StartDialogueAndEnableCanvas()
    {
        // Trigger the dialogue and enable the canvas
        DisplayNextSentence();
        StartCoroutine(DisappearAndActivateCanvas());
    }
}