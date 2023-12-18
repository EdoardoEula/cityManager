using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;

public class CO2BarController : MonoBehaviour
{
    public Slider co2Slider;
    public Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        RetrieveUserData(GameManager.currentUser);
        UpdateCO2Bar();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for changes in GameManager.level_co2
        if (GameManager.level_co2 != co2Slider.value)
        {
            // Update the CO2 bar
            UpdateCO2Bar();
        }
    }

    void UpdateCO2Bar()
    {
        // Calculate the normalized value between 0 and 1 based on the gradient
        float normalizedValue = Mathf.InverseLerp(co2Slider.minValue, co2Slider.maxValue, GameManager.level_co2);

        // Evaluate the color from the gradient based on the normalized value
        Color color = gradient.Evaluate(normalizedValue);

        // Apply the color to the slider's fill area
        co2Slider.fillRect.GetComponent<Image>().color = color;

        // Animate the slider value change using DOTween
        co2Slider.DOValue(GameManager.level_co2, 0.8f);
        //co2Slider.value = GameManager.level_co2;
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
    private int ConvertToInt(object value)
    {
        return value != null ? System.Convert.ToInt32(value) : 0;
    }
}
