using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class Info_click : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI info_title;
    public TextMeshProUGUI titletocopy;
    public TextMeshProUGUI info_description;
    public GameObject infoPanel;
    private bool isPanelOpen = false;
    public Transform mytransform;

    private DatabaseReference databaseReference;

    void Start()
    {
        myButton.onClick.AddListener(ToggleVisibility);

        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    void ToggleVisibility()
    {
        if (isPanelOpen)
        {
            infoPanel.SetActive(false);
            isPanelOpen = false;
        }
        else
        {
            infoPanel.SetActive(true);
            isPanelOpen = true;

            // Set the position of the info_panel using the transform's position
            infoPanel.transform.position = mytransform.position;

            // Get the choice selected
            string selectedChoice = titletocopy.text;

            // Query the Firebase database for the selected choice
            QueryDatabase(selectedChoice);
        }
    }

    void QueryDatabase(string selectedChoice)
    {
        // Reference to the "infos" node in the database
        DatabaseReference infosRef = databaseReference.Child("infos");

        infosRef.OrderByChild("Choice").EqualTo(selectedChoice).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error querying database: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Iterate through the results (there should be only one match)
                foreach (DataSnapshot infoSnapshot in snapshot.Children)
                {
                    // Extract the "Info" field and set it to info_description
                    string infoText = infoSnapshot.Child("Info").Value.ToString();
                    info_title.text = selectedChoice;
                    info_description.text = infoText;
                }
            }
        });
    }
}
