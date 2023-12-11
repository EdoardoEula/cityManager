using System.IO;
using Accord.IO;
using UnityEngine;
using TMPro;
using Accord.MachineLearning.DecisionTrees;

public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
        int age = GameManager.age;
        int eduLev = MapEducationLevel(GameManager.education);
        int q1 = GameManager.choice_q1;
        int q2 = GameManager.choice_q2;
        int q3 = GameManager.choice_q3;
        int genderMale = (GameManager.gender == "Male") ? 1 : 0;
        int genderFemale = (GameManager.gender == "Female") ? 1 : 0;
        int genderNonbinary = (GameManager.gender == "Non-binary") ? 1 : 0;
        int genderPreferNotToSay = (GameManager.gender == "Prefer not to say") ? 1 : 0;

        // Set the text of the moneyText component
        if (moneyText != null)
        {
            moneyText.text = $"{GameManager.money_available}";
        }
        else
        {
            Debug.Log("Money text component not assigned.");
        }

        // Load the trained model
        RandomForest model;

        using (FileStream fs = File.OpenRead("Assets/random_forest_model.joblib"))
        {
            model = Serializer.Load<RandomForest>(fs);
        }

        // Example input for prediction
        double[] newInput = {age, eduLev, q1, q2, q3, genderFemale, genderMale, genderNonbinary, genderPreferNotToSay};

        // Make predictions
        int prediction = model.Decide(newInput);

        // Use the prediction as needed
        Debug.Log($"Prediction: {prediction}");
    }
    
    private int MapEducationLevel(string educationLevel)
    {
        switch (educationLevel.ToLower())
        {
            case "Elementary School":
                return 5;
            case "Middle School":
                return 8;
            case "High School":
                return 13;
            case "Bachelor's Degree":
                return 18;
            case "Master's Degree":
                return 22;
            case "Doctoral Degree":
                return 25;
            default:
                return -1;
        }
    }
}