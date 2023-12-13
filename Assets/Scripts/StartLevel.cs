using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using Accord.MachineLearning.DecisionTrees;

public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
        //int age = GameManager.age;
        //int eduLev = MapEducationLevel(GameManager.education);
        //int q1 = GameManager.choice_q1;
        //int q2 = GameManager.choice_q2;
        //int q3 = GameManager.choice_q3;

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

        byte[] data = File.ReadAllBytes("Assets/random_forest_model.joblib");
        using (MemoryStream ms = new MemoryStream(data))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            model = (RandomForest)formatter.Deserialize(ms);
        }

        // Example input for prediction
        double[] newInput = { 24, 5, 2, 1, 3 };
        //double[] newInput = { age, eduLev, q1, q2, q3};

        // Make predictions
        int prediction = model.Decide(newInput);

        // Use the prediction as needed
        Debug.Log($"Prediction: {prediction}");
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
