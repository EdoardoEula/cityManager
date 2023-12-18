using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;


public class SignInFirebase : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public Button signInButton;
    public TMP_Text textPwd;

    private void Awake()
    {
        passwordInput.contentType = TMP_InputField.ContentType.Password;
    }

    private void Start()
    {
        signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            textPwd.text = "Email and password cannot be empty";
            return;
        }

        // Call the Firebase sign-in method
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    textPwd.text = "Sign-in canceled.";
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    textPwd.text = "Wrong Credentials"; // Display an appropriate error message
                    return;
                }

                // Sign-in successful
                AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);

                GameManager.currentUser = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

                if (FirebaseAuth.DefaultInstance.CurrentUser != null &&
                    FirebaseAuth.DefaultInstance.CurrentUser.Email.Equals(email))
                {
                    // Save personalization value and switch to the main scene
                    StartCoroutine(SignInRoutine(FirebaseAuth.DefaultInstance.CurrentUser.UserId));
                }
            });
    }

    private IEnumerator SignInRoutine(string userId)
    {
        yield return new WaitUntil(() => FirebaseAuth.DefaultInstance.CurrentUser != null);

        // Save personalization value
        RetrieveUserData(userId);
        
        yield return new WaitUntil(() => GameManager.personalization != null);
        
        Debug.Log("GameManager.personalization: " + GameManager.personalization);
        if (GameManager.personalization == 0)
        {
            SwitchScene("GameStart", "CharacterChoice");
        }
        else
        {
            SwitchScene("GameStart", "MainScene");
        }
    }


    public void SwitchScene(string startscene, string nextscene)
    {

        if (SceneManager.GetSceneByName(startscene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(startscene);
        }

        SceneManager.LoadScene(nextscene, LoadSceneMode.Single);
    }

    private void RetrieveUserData(string userId)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DatabaseReference userReference = reference.Child("users").Child(userId);

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

    // Helper function to convert object to int
    private int ConvertToInt(object value)
    {
        return value != null ? System.Convert.ToInt32(value) : 0;
    }
    private string ConvertToString(object value)
    {
        return value != null ? value.ToString() : string.Empty;
    }
        
        
}

