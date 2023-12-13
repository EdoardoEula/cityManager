using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManagerRes : MonoBehaviour,IDialogueManager
{
    public TextMeshProUGUI nameText;
    private Queue<string> sentences;

    public TextMeshProUGUI dialogueText;
    public Animator animator;

    private bool skipEnabled = false;
    private bool shouldSkipSentence = false;
    private bool skipButtonClicked = false;
    public Button continueButton; // Reference to your ContinueButton
    [SerializeField]
    public Button[] buttons;
    
    private bool dialogueEnded = false;

    // Reference to your LevelFrameCanvas
    public Canvas levelFrameCanvas;

    void Start()
    {
        sentences = new Queue<string>();
        DisableButtons(); // Call this to disable buttons initially
    }
    public void StartDialogue(CSVReader.Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        Debug.Log("Starting conversation with " + dialogue.character);
        nameText.text = "Emanuel Taurus";
        sentences.Clear();
        sentences.Enqueue(dialogue.sentence);
        DisplayNextSentence();
        skipEnabled = false;
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (dialogueText != null)
        {
            dialogueText.text = "";
            StopAllCoroutines();
            StartCoroutine(TypeOrSkipSentence(sentence));
        }
        else
        {
            Debug.LogError("dialogueText is null. Make sure it's assigned in the Inspector.");
        }

        // Disable the ContinueButton initially
        continueButton.interactable = skipButtonClicked;
    }

    IEnumerator TypeOrSkipSentence(string sentence)
    {
        if (skipEnabled && shouldSkipSentence)
        {
            // If skipping, display the entire sentence immediately
            dialogueText.text = sentence;

            // Enable the ContinueButton after the sentence is displayed
            EnableContinueButton();
        }
        else
        {
            // If not skipping, display the sentence gradually
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;

                // Check if the skip button is clicked during the display
                if (skipButtonClicked)
                {
                    EnableContinueButton();
                    yield break; // Exit the coroutine early
                }

                yield return new WaitForSeconds(0.005f);
            }

            // Enable the ContinueButton after the entire sentence is displayed
            EnableContinueButton();
        }
    }

    void EnableContinueButton()
    {
        // Enable the ContinueButton
        continueButton.interactable = true;
    }
    

    public void SetShouldSkipSentence(bool value)
    {
        shouldSkipSentence = value;
    }

    public void ResetSkipButtonClicked()
    {
        skipButtonClicked = false;
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        EnableButtons(); // Call this to enable buttons when the dialogue ends
        bool dialogueEnded = HasDialogueEnded();

        // Do something with the result, if needed
        if (dialogueEnded)
        {
            // Dialogue has ended, perform additional actions if needed
            Debug.Log("Dialogue has ended!");
        }
    }
    public bool HasDialogueEnded()
    {
        // Check if the dialogue has started
        bool dialogueStarted = sentences.Count > 0; // Assuming sentences is the queue storing the dialogue sentences

        // Check if the dialogue has ended
        bool dialogueEnded = dialogueStarted && sentences.Count == 0;

        return dialogueEnded;
    }
    void DisableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void ToggleSkipEnabled()
    {
        skipEnabled = !skipEnabled;
        shouldSkipSentence = false;

        // Enable the ContinueButton if the skip button is clicked
        continueButton.interactable = skipButtonClicked;
    }
    
}