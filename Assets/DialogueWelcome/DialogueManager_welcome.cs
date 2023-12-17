using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
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

                yield return new WaitForSeconds(0.000000000000000000001f);
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
        int age = GameManager.age;
        int eduLev = MapEducationLevel(GameManager.education);
        int q1 = GameManager.choice_q1;
        int q2 = GameManager.choice_q2;
        int q3 = GameManager.choice_q3;
        int total_points;
            
        total_points = q1 + q2 + q3;

        if (age < 30)
        {
            total_points += 3;
        } else if (age < 40)
        {
            total_points += 2;
        }
        else
        {
            total_points += 1;
        }

        if (eduLev >= 18)
        {
            total_points += 3;
        }
        else if (eduLev == 13)
        {
            total_points += 2;
        } else if (eduLev < 13)
        {
            total_points += 1;
        }
        
        Debug.Log(total_points);

        if (total_points >= 5 && total_points < 8)
        {
            GameManager.personalization = 1;
        } else if (total_points < 12)
        {
            GameManager.personalization = 3;
        }
        else
        {
            GameManager.personalization = 2;
        }
            
        SavePersonalizationToDatabase(GameManager.currentUser, GameManager.personalization);
        
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
    
    private void SavePersonalizationToDatabase(string userId, int? personalizationValue)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Get a reference to the user node in the database
        DatabaseReference userReference = reference.Child("users").Child(userId).Child("personalizationField");

        userReference.SetValueAsync(personalizationValue)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save personalization value: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Personalization value saved successfully");
                }
            });
    }

    private int MapEducationLevel(string educationLevel)
    {
        switch (educationLevel.ToLower())
        {
            case "elementary school":
                return 5;
            case "middle school":
                return 8;
            case "high school":
                return 13;
            case "bachelor's degree":
                return 18;
            case "master's degree":
                return 22;
            case "doctoral degree":
                return 25;
            default:
                return -1;
        }
    }
    
}
