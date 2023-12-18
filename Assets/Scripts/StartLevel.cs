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
    private static StartLevel instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // If an instance already exists, destroy this new instance
            Destroy(gameObject);
            return;
        }

        // Set the instance to this script
        instance = this;

        // Keep this GameObject alive when loading new scenes
        DontDestroyOnLoad(gameObject);

        backgroundMusic.Play();
    }

    void Start()
    {
        Debug.Log("Start Level");
        RetrieveUserData(GameManager.currentUser);
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
    
    private void RetrieveUserData(string userId)
    {
        DatabaseReference userReference = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId);

        userReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve user data: " + task.Exception);
                return;
            }

            DataSnapshot userDataSnapshot = task.Result;

            if (userDataSnapshot != null && userDataSnapshot.Exists)
            {
                // Convert the JSON data to a dictionary
                Dictionary<string, object> userDataDict = userDataSnapshot.Value as Dictionary<string, object>;

                if (userDataDict != null)
                {
                    // Access individual values
                    GameManager.money_available = ConvertToInt(userDataDict["moneyAvailable"]);
                    GameManager.level_co2 = ConvertToInt(userDataDict["levelCO2"]);
                    GameManager.personalization = ConvertToInt(userDataDict["personalizationField"]);

                    // Now you can use these values as needed
                    Debug.LogFormat("User Data - Money Available: {0}, LevelCO2: {1}, Personalization Field: {2}",
                        GameManager.money_available, GameManager.level_co2, GameManager.personalization);
                }
            }
            else
            {
                Debug.LogWarning("User data not found for user ID: " + userId);
            }
        });
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

                    // Find the corresponding GameObject in the 'World';
                    Transform choiceObject = World.transform.Find(choiceName);

                    if (choiceObject != null)
                    {
                        // Iterate through the grandchildren of the choiceObject

                        choiceObject.GetChild(0).gameObject.SetActive(choiceValue == "No");
                        choiceObject.GetChild(1).gameObject.SetActive(choiceValue == "Yes");
                    }
                }
            });
        }
    }
    private int ConvertToInt(object value)
    {
        return value != null ? System.Convert.ToInt32(value) : 0;
    }
}
