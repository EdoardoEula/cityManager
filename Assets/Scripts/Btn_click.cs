using System;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class Btn_click : MonoBehaviour
{
    public Canvas canvasQuestionPanel;
    public GameObject infoPanel;
    public Button myButton;
    public Button myButton2;
    public Button myButton3;
    public Button myButton4;
    public Button myButton5;
    public Button myButton6;
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

    public bool isCanvasVisible = false;
    private bool clicked_mybutton = false;

    void Start()
    {
        myButton.onClick.AddListener(ToggleVisibility);
        UpdateButtonState();
    }

    public async void ToggleVisibility()
    {
        isCanvasVisible = !isCanvasVisible;
        clicked_mybutton = !clicked_mybutton;
        canvasQuestionPanel.enabled = isCanvasVisible;

        if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
        }

        title.text = inputText_title;
        firstchoicetext.text = inputText_firstChoice;
        secondchoicetext.text = inputText_secondChoice;
        thirdchoicetext.text = inputText_thirdChoice;

        string[] choices = { inputText_firstChoice, inputText_secondChoice, inputText_thirdChoice };
        TextMeshProUGUI[] moneyTextArray = { firstmoneytext, secondmoneytext, thirdmoneytext };
        int moneyTextIndex = 0;
        
        for (int i = 0; i < choices.Length; i++)
        {
            string selectedChoice = choices[i];
            Debug.Log("Current Choice: " + selectedChoice);

            DatabaseReference choicesref = FirebaseDatabase.DefaultInstance.RootReference.Child("choice_mon_co2");
            choicesref.OrderByChild("Change").EqualTo(selectedChoice).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error querying database: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("In the database");
                    DataSnapshot snapshot = task.Result;

                    foreach (DataSnapshot money_co2Snap in snapshot.Children)
                    {
                        object moneyObject = money_co2Snap.Child("Cost").Value;
                        string moneyValue = moneyObject.ToString();
                        Debug.Log("MoneyValue: " + moneyValue);
                        Debug.Log("Index: " + moneyTextIndex);
                        Debug.Log("Length Array: " + moneyTextArray.Length);
                        moneyTextArray[moneyTextIndex].text = moneyValue;
                        moneyTextIndex += 1;
                    }
                }
            });
        }

        await InsertImages();
    }
    

    async Task InsertImages()
    {
        firstImage.sprite = firstImageSprite;
        secondImage.sprite = secondImageSprite;
        thirdImage.sprite = thirdImageSprite;
    }

    void UpdateButtonState()
    {
        if (clicked_mybutton)
        {
            DisableButtons(myButton2, myButton3, myButton4, myButton5, myButton6);
        }
        else
        {
            EnableButtons(myButton2, myButton3, myButton4, myButton5, myButton6);
        }
    }

    void EnableButtons(params Button[] buttons)
    {
        string currentUser = GameManager.currentUser;

        if (string.IsNullOrEmpty(currentUser))
        {
            Debug.LogError("Current user is null or empty.");
            return;
        }

        DatabaseReference buttonsRef = FirebaseDatabase.DefaultInstance.RootReference.Child("buttons").Child(currentUser);

        buttonsRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving buttons data from Firebase: " + task.Exception);
                return;
            }

            DataSnapshot buttonsSnapshot = task.Result;

            foreach (Button button in buttons)
            {
                string buttonName = button.name;
                buttonName = buttonName.Replace("_btn", "");

                if (buttonsSnapshot.HasChild(buttonName))
                {
                    string buttonValue = buttonsSnapshot.Child(buttonName).Value.ToString();
                    string buttonObjectName = buttonValue + "_btn";

                    if (buttonObjectName != null)
                    {
                        bool isInteractable = buttonValue.Equals("On", StringComparison.OrdinalIgnoreCase);
                        button.interactable = isInteractable;
                    }
                }
                else
                {
                    Debug.LogWarning($"Button {buttonName} not found in Firebase data.");
                }
            }
        });
    }

    void DisableButtons(params Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }
}
