// Updated DialogueManager script
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    private Queue<string> sentences;

    public TextMeshProUGUI dialogueText;
    public Animator animator;

    private bool skipEnabled = false;
    private bool shouldSkipSentence = false;
    private bool skipButtonClicked = false;
    public Button continueButton; // Reference to your ContinueButton

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(CSVReader.Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        Debug.Log("Starting conversation with " + dialogue.character);
        nameText.text = "Susan White";
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

    void EnableSkipButton()
    {
        // Implement your logic to enable the skip button here
        FindObjectOfType<CharacterInteractionfarm>().EnableSkipButton();
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
    }

    public void ToggleSkipEnabled()
    {
        skipEnabled = !skipEnabled;
        shouldSkipSentence = false;

        // Enable the ContinueButton if the skip button is clicked
        continueButton.interactable = skipButtonClicked;
    }
    
}
