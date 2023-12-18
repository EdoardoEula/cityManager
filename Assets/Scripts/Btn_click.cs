using System;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Btn_click : MonoBehaviour
{
    public Canvas canvasQuestionPanel;
    public GameObject infoPanel;
    public Button myButton;
    public Button closeButton;
    public Button[] otherButtons;
    public string inputText_firstChoice;
    public string inputText_secondChoice;
    public string inputText_thirdChoice;
    public string inputText_title;
    public TextMeshProUGUI firstchoicetext;
    public TextMeshProUGUI secondchoicetext;
    public TextMeshProUGUI thirdchoicetext;
    public TextMeshProUGUI firstmoneytext;
    public TextMeshProUGUI secondmoneytext;
    public TextMeshProUGUI thirdmoneytext;
    public TextMeshProUGUI title;
    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;
    public Sprite firstImageSprite;
    public Sprite secondImageSprite;
    public Sprite thirdImageSprite;

    private string[] choices;
    private string[] moneyTextArray;
    private string moneyValue;

    private bool isCanvasVisible = false;
    private bool clickedMyButton = false;

    private void Start()
    {
        myButton.onClick.AddListener(OpenQuestionPanel);
        closeButton.onClick.AddListener(CloseQuestionPanel);
        UpdateButtonState();
    }

    private void CloseQuestionPanel()
    {
        canvasQuestionPanel.enabled = false;
        if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
        }

        ResetUIElements();
    }

    private void ResetUIElements()
    {
        firstchoicetext.text = "";
        secondchoicetext.text = "";
        thirdchoicetext.text = "";

        firstmoneytext.text = "";
        secondmoneytext.text = "";
        thirdmoneytext.text = "";
    }

    private async void OpenQuestionPanel()
    {
        try
        {
            
            if (infoPanel.activeSelf)
            {
                infoPanel.SetActive(false);
            }
            
            title.text = inputText_title;
            firstchoicetext.text = inputText_firstChoice;
            secondchoicetext.text = inputText_secondChoice;
            thirdchoicetext.text = inputText_thirdChoice;
            
            string[] choices = { inputText_firstChoice, inputText_secondChoice, inputText_thirdChoice };
            int moneyInd = 0;

            await QueryDatabaseMoney(inputText_firstChoice, firstmoneytext);
            await QueryDatabaseMoney(inputText_secondChoice, secondmoneytext);
            await QueryDatabaseMoney(inputText_thirdChoice, thirdmoneytext);
            
            await InsertImages();
            
            canvasQuestionPanel.enabled = true;
            
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in OpenQuestionPanel: {e.Message}");
        }
    }
    
    async Task QueryDatabaseMoney(string selectedChoice, TMP_Text money_choice)
    {
        // Reference to the "infos" node in the database
        DatabaseReference monCo2Ref = FirebaseDatabase.DefaultInstance.RootReference.Child("choice_mon_co2");
        try
        {
            DataSnapshot snapshot = await monCo2Ref.OrderByChild("Change").EqualTo(selectedChoice).GetValueAsync();

            // Iterate through the results (there should be only one match)
            foreach (DataSnapshot money_co2Snap in snapshot.Children)
            {
                // Extract the "Info" field and set it to info_description
                object moneyObject = money_co2Snap.Child("Cost").Value;

                if (moneyObject != null && int.TryParse(moneyObject.ToString(), out int moneyValue))
                {
                    money_choice.text = moneyValue.ToString();
                }
                else
                {
                    Debug.LogError("Failed to convert 'Cost' to int");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error querying database: " + ex.Message);
        }
    }

    async Task InsertImages()
    {
        firstImage.sprite = firstImageSprite;
        secondImage.sprite = secondImageSprite;
        thirdImage.sprite = thirdImageSprite;
    }

    private void UpdateButtonState()
    {
        if (clickedMyButton)
        {
            DisableButtons(otherButtons);
        }
        else
        {
            EnableButtons(otherButtons);
        }
    }

    private void EnableButtons(Button[] buttons)
    {
        string currentUser = GameManager.currentUser;

        if (string.IsNullOrEmpty(currentUser))
        {
            Debug.LogError("Current user is null or empty.");
            return;
        }

        DatabaseReference buttonsRef = FirebaseDatabase.DefaultInstance.RootReference.Child("buttons").Child(currentUser);

        buttonsRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving buttons data from Firebase: " + task.Exception);
                return;
            }

            DataSnapshot buttonsSnapshot = task.Result;

            foreach (Button button in buttons)
            {
                string buttonName = button.name.Replace("_btn", "");

                if (buttonsSnapshot.HasChild(buttonName))
                {
                    string buttonValue = buttonsSnapshot.Child(buttonName).Value.ToString();
                    string buttonObjectName = buttonValue + "_btn";

                    if (buttonObjectName != null)
                    {
                        bool isInteractable = buttonValue.Equals("On", StringComparison.OrdinalIgnoreCase);
                        SetInteractableOnMainThread(button, isInteractable);
                    }
                }
                else
                {
                    Debug.LogWarning($"Button {buttonName} not found in Firebase data.");
                }
            }
        });
    }

    private void SetInteractableOnMainThread(Button button, bool isInteractable)
    {
        // Use Unity's main thread dispatcher to set interactable on the main thread
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            button.interactable = isInteractable;
        });
    }


    private void DisableButtons(Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }
}
