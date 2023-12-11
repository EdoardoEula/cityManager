using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;
    
    private Dictionary<string, float> educationMapping = new Dictionary<string, float>
    {
        {"Elementary School", 5.0f},
        {"Middle School", 8.0f},
        {"High School", 13.0f},
        {"Bachelor's Degree", 18.0f},
        {"Master's Degree", 22.0f},
        {"Doctoral Degree", 25.0f}
    };

    void Start()
    {
            // Set the text of the moneyText component
            if (moneyText != null)
            {
                moneyText.text = $"{GameManager.money_available}";
            }
            else
            {
                Debug.LogWarning("Money text component not assigned.");
            }
    }

    private void Update()
    {
        string userGender = GameManager.gender;
        int gender_Female = (userGender == "Male") ? 1 : 0;
        int gender_Male = (userGender == "Female") ? 1 : 0;
        int gender_Nonbinary = (userGender == "Non-binary") ? 1 : 0;
        int gender_Prefernottosay = (userGender == "Prefer not to say") ? 1 : 0;
        
        educationMapping.TryGetValue(GameManager.education, out float education);
    }
}
