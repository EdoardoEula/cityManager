using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
        

        if (GameManager.personalization == 0)
        {
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
            SaveDefaultValuesToDatabase(GameManager.currentUser);
        }
        
        
        if (moneyText != null)
        {
            moneyText.text = $"{GameManager.money_available}";
        }
        else
        {
            Debug.Log("Money text component not assigned.");
        }
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

    private void SaveDefaultValuesToDatabase(string userId)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

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
