using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;

public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;
    public GameObject World;
    public GameObject UI;
    public AudioSource backgroundMusic;

    private void Awake()
    {
        backgroundMusic.Play();
    }

    void Start()
    {
        if (moneyText != null)
        {
            moneyText.text = $"{GameManager.money_available}";
        }
        else
        {
            Debug.Log("Money text component not assigned.");
        }

        ProcessUserChoices();
    }

    private void ProcessUserChoices()
    {
        // Retrieve currentUser public variable from the script GameManager
        string currentUser = GameManager.currentUser;
        Debug.Log(currentUser);
        // Check if currentUser is not null or empty
        if (!string.IsNullOrEmpty(currentUser))
        {
            // Create a reference to the 'user_choices' node using currentUser as the primary key
            DatabaseReference userChoicesRef = FirebaseDatabase.DefaultInstance.RootReference.Child("user_choice").Child(currentUser);

            // Retrieve data from Firebase
            userChoicesRef.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle error
                    Debug.LogError("Error retrieving user choices from Firebase: " + task.Exception);
                }

                DataSnapshot userChoicesSnapshot = task.Result;
                Debug.Log(userChoicesSnapshot);

                // Iterate through the children of the 'user_choices' node
                foreach (DataSnapshot choiceSnapshot in userChoicesSnapshot.Children)
                {
                    string choiceName = choiceSnapshot.Key;
                    string choiceValue = choiceSnapshot.Value.ToString();
                    Debug.Log($"{choiceName}: {choiceValue}");

                    // Find the corresponding GameObject in the 'World';
                    Transform choiceObject = World.transform.Find(choiceName);

                    if (choiceObject != null)
                    {
                        Debug.Log("Game Object not null");
                        Debug.Log(choiceObject.childCount);
                        // Iterate through the grandchildren of the choiceObject

                        choiceObject.GetChild(0).gameObject.SetActive(choiceValue == "No");
                        choiceObject.GetChild(1).gameObject.SetActive(choiceValue == "Yes");
                    }
                }
            });
        }
    }
}
