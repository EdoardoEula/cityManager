using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInteractionfarm : MonoBehaviour
{
    public CharacterMovementfarm otherCharacterfarm;
    public GameObject iconArrowfarm;
    public GameObject farmIndicationsCanvas;
    private bool isClicked = false;
    private CSVReader.DialogueList dialogueList;
    private string currentCharacter = "Entrepreneur";
    private string currentLevel = "Farm";

    private int currentSentenceIndex = 1;
    private bool isSkipButtonClickable = true;
    
    public Button continueButton;
    public Button skipButton;
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
        iconArrowfarm.SetActive(true);
        
        if (farmIndicationsCanvas != null)
        {
            farmIndicationsCanvas.GetComponent<Canvas>().enabled = false;
        }

        continueButton.onClick.AddListener(OnContinueButtonClick);
        skipButton.onClick.AddListener(OnSkipButtonClick);
       
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
            FindObjectOfType<DialogueManager>().ToggleSkipEnabled();
            FindObjectOfType<DialogueManager>().SetShouldSkipSentence(true);
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
            FindObjectOfType<DialogueManager>().EndDialogue();
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

       if (farmIndicationsCanvas != null)
       {
           farmIndicationsCanvas.GetComponent<Canvas>().enabled = true;
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
           DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
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
           FindObjectOfType<DialogueManager>().EndDialogue();
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
       FindObjectOfType<DialogueManager>().EndDialogue();
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
       return dialogueList.dialogue[currentSentenceIndex-1]; 
   }
    
    

    void OnMouseDown()
    {
        otherCharacterfarm.CallCharacterfarm();  // Chiamata al metodo nel CharacterMovement
        
        
        // Nascondi l'icona quando il personaggio viene cliccato
        if (iconArrowfarm != null)
        {
            iconArrowfarm.SetActive(false);
            DisplayNextSentence();
            StartCoroutine(DisappearAndActivateCanvas());
        }
        
    }
}