// Updated DialogueManager script
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

using UnityEngine.UI;
using Unity.Barracuda;
using UnityEngine.SceneManagement;

public class DialogueManager_welcome : MonoBehaviour
{
    
    private Queue<string> sentences;

    public TextMeshProUGUI dialogueText;

    private bool skipEnabled = false;
    private bool shouldSkipSentence = false;
    private bool skipButtonClicked = false;
    public Button continueButton;

    
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(CSVReader_welcome.Dialogue dialogue)
    {
        Debug.Log("Starting conversation...");
        sentences.Clear();
        sentences.Enqueue(dialogue.sentence);  // Assuming your Dialogue class has a 'sentence' property
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
        string nextScene = "MainScene";
        string currentScene = "CharacterChoice";
        
        if (SceneManager.GetSceneByName(currentScene).isLoaded)
        {
            Debug.Log(currentScene + " is loaded. Unloading...");
            SceneManager.UnloadSceneAsync(currentScene);
        }
        else
        {
            Debug.Log(currentScene + " is not loaded.");
        }
        Debug.Log("Loading " + nextScene);
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        
    }

    public void ToggleSkipEnabled()
    {
        skipEnabled = !skipEnabled;
        shouldSkipSentence = false;

        // Enable the ContinueButton if the skip button is clicked
        continueButton.interactable = skipButtonClicked;
    }
    
}
