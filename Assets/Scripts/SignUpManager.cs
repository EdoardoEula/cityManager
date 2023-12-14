using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class SignUpManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField nameInput;
    public TMP_InputField surnameInput;
    public TMP_InputField ageInput;
    public TMP_Dropdown genderDropdown;
    public TMP_Dropdown educationDropdown;
    public TMP_Text signUpResult;
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

        if (!IsValidEmail(username))
        {
            Debug.LogError("Invalid email format.");
            return;
        }

        StartCoroutine(SignUpRoutine(username, password));
    }

    private IEnumerator SignUpRoutine(string username, string password)
    {
        var signUpTask = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(username, password);

        yield return new WaitUntil(() => signUpTask.IsCompleted);

        if (signUpTask.IsFaulted)
        {
            signUpResult.text = "Signup encountered an error: " + signUpTask.Exception;
            Debug.LogError("Signup encountered an error: " + signUpTask.Exception);
            yield break;
        }

        // Signup successful
        Debug.LogFormat("User signed up successfully: {0}", username);

        // Get the user ID of the newly created user
        GameManager.currentUser = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        // Move the call to SaveDefaultValuesToDatabase inside the continuation block
        SaveDefaultValuesToDatabase(GameManager.currentUser);

        // Continue with sign-in and scene switch
        yield return SignInAndSwitchScene(username, password);
    }

    private IEnumerator SignInAndSwitchScene(string username, string password)
    {
        var signInTask = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(username, password);

        yield return new WaitUntil(() => signInTask.IsCompleted);

        if (signInTask.IsFaulted)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + signInTask.Exception);
            yield break;
        }

        AuthResult result = signInTask.Result;
        Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);

        GameManager.currentUser = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        if (FirebaseAuth.DefaultInstance.CurrentUser != null &&
            FirebaseAuth.DefaultInstance.CurrentUser.Email.Equals(username))
        {
            // Save personalization value and switch to the main scene
            yield return SignInRoutine(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        }
    }

    private IEnumerator SignInRoutine(string userId)
    {
        yield return new WaitUntil(() => FirebaseAuth.DefaultInstance.CurrentUser != null);

        // Save personalization value
        RetrieveUserData(userId);

        yield return new WaitUntil(() => GameManager.personalization != null);

        if (GameManager.personalization == 0)
        {
            SwitchScene("SignUpScene", "CharacterChoice");
        }
        else
        {
            SwitchScene("SignUpScene", "MainScene");
        }
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

    private void SaveDefaultValuesToDatabase(string userId)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        // Define default values
        int defaultMoneyAvailable = 5000;
        int defaultLevelCO2 = 100;
        int defaultPersonalizationField = 0;

        // Get the values from the input fields
        string firstName = nameInput.text;
        string surname = surnameInput.text;
        int age = ConvertToInt(ageInput.text);
        GameManager.age = age;
        GameManager.gender = genderDropdown.options[genderDropdown.value].text;
        GameManager.education = educationDropdown.options[educationDropdown.value].text;

        // Create a dictionary with default and additional values
        Dictionary<string, object> defaultUserData = new Dictionary<string, object>
        {
            {"moneyAvailable", defaultMoneyAvailable},
            {"levelCO2", defaultLevelCO2},
            {"personalizationField", defaultPersonalizationField},
            {"name", firstName},
            {"surname", surname},
            {"age", GameManager.age},
            {"gender", GameManager.gender},
            {"educationalLevel", GameManager.education},
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
        
        string defaultChoice = "No";

        Dictionary<string, object> defaultUserChoicesData = new Dictionary<string, object>
        {
            { "Lab-grown meat", defaultChoice },
            { "Open-land farming", defaultChoice },
            { "Intensive breeding", defaultChoice },
            { "Water collection", defaultChoice },
            { "Intensive greenhouse", defaultChoice },
            { "Renewable energy", defaultChoice },
            { "Deforestation", defaultChoice },
            { "Organic Farming", defaultChoice },
            { "Crop rotation", defaultChoice },
            { "Fossil fuel use", defaultChoice },
            { "Wastewater", defaultChoice },
            { "Renewable sources", defaultChoice },
            { "Recycling programs", defaultChoice },
            { "Waste incineration", defaultChoice },
            { "Advanced technologies", defaultChoice },
            { "Efficient vehicles", defaultChoice },
            { "Sustainable transports", defaultChoice },
            { "Fossil fuel", defaultChoice },
            { "Tree planting", defaultChoice },
            { "Gas Emissions", defaultChoice },
            { "Public transports", defaultChoice },
            { "Lightening systems", defaultChoice },
            { "Heating systems", defaultChoice },
            { "Fossil fuels use", defaultChoice },
            { "Biking", defaultChoice },
            { "Minimize Gas emissions", defaultChoice },
            { "Fossil fuel dependency", defaultChoice },
            { "Park", defaultChoice },
            { "Plants", defaultChoice },
            { "Parking1", defaultChoice },
            { "Bins", defaultChoice },
            { "Drinking water", defaultChoice },
            { "Resource Depletion", defaultChoice },
            { "Workshops", defaultChoice },
            { "Financial incentives", defaultChoice },
            { "Work-related travel", defaultChoice },
            { "Charging stations", defaultChoice },
            { "Planting trees", defaultChoice },
            { "Incentives for gasoline", defaultChoice },
            { "Limiting water", defaultChoice },
            { "Fossil Fuel-Based Energy", defaultChoice },
            { "Parks", defaultChoice },
            { "Cycling paths", defaultChoice },
            { "Parking2", defaultChoice },
            { "First necessities", defaultChoice },
            { "Malls", defaultChoice },
            { "Pedestrian Area", defaultChoice },
            { "Incineration", defaultChoice },
            { "Garbages", defaultChoice },
            { "E-waste", defaultChoice },
            { "Metro", defaultChoice },
            { "Paved Surfaces", defaultChoice },
            { "Electric Vehicles", defaultChoice },
        };

// Get a reference to the user_choices node in the database
        DatabaseReference userChoicesReference = reference.Child("user_choice").Child(userId);

// Run a transaction to check and set the default values for user_choices
        userChoicesReference.RunTransaction(mutableData =>
            {
                if (mutableData.Value == null)
                {
                    mutableData.Value = defaultUserChoicesData;
                    return TransactionResult.Success(mutableData);
                }

                // If the data already exists, do nothing
                return TransactionResult.Abort();
            })
            .ContinueWithOnMainThread(transactionTask =>
            {
                if (transactionTask.IsFaulted)
                {
                    Debug.LogError("Failed to save default user choices: " + transactionTask.Exception);
                }
                else if (transactionTask.IsCompleted)
                {
                    Debug.Log("Default user choices saved successfully");
                }
            });
    }
    

    public void SwitchScene(string startscene, string nextscene)
    {
        if (SceneManager.GetSceneByName(startscene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(startscene);
        }

        SceneManager.LoadScene(nextscene, LoadSceneMode.Single);
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

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}


