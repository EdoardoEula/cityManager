using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class SignUpManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button signUpButton;

    private void Start()
    {
        signUpButton.onClick.AddListener(SignUp);
    }

    private void SignUp()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Username and password cannot be empty.");
            return;
        }

        // Call the Firebase signup method
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(username, password)
            .ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Signup was canceled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("Signup encountered an error: " + task.Exception);
                    return;
                }

                // Signup successful
                Debug.LogFormat("User signed up successfully: {0}", username);

                // Get the user ID of the newly created user
                GameManager.currentUser = task.Result.User.UserId;

                // Move the call to SaveDefaultValuesToDatabase inside the continuation block
                SaveDefaultValuesToDatabase(GameManager.currentUser);
            });
    }

    [System.Serializable]
    public class UserData
    {
        public int levelCO2;
        public int moneyAvailable;
        public int personalizationField;
    }

    private void SaveDefaultValuesToDatabase(string userId)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Define default values
        int defaultMoneyAvailable = 5000;
        int defaultLevelCO2 = 100;
        int defaultPersonalizationField = 0;

        // Create a dictionary with default values
        Dictionary<string, object> defaultUserData = new Dictionary<string, object>
        {
            {"moneyAvailable", defaultMoneyAvailable},
            {"levelCO2", defaultLevelCO2},
            {"personalizationField", defaultPersonalizationField}
        };

        // Get a reference to the user node in the database
        DatabaseReference userReference = reference.Child("users").Child(userId);

        // Run a transaction to check and set the default values
        userReference.RunTransaction(mutableData =>
            {
                if (mutableData.Value == null)
                {
                    mutableData.Value = defaultUserData;
                    return TransactionResult.Success(mutableData);
                }

                // If the data already exists, do nothing
                return TransactionResult.Abort();
            })
            .ContinueWithOnMainThread(transactionTask =>
            {
                if (transactionTask.IsFaulted)
                {
                    Debug.LogError("Failed to save default values: " + transactionTask.Exception);
                }
                else if (transactionTask.IsCompleted)
                {
                    Debug.Log("Default values saved successfully");
                }
            });
    }
}

