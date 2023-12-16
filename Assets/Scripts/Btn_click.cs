using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Sprite firstImageSprite;
    public Sprite secondImageSprite;
    public Sprite thirdImageSprite;
    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;

    private bool isCanvasVisible = false;
    private bool clicked_mybutton = false;
    

    DatabaseReference databaseReference;

    void Start()
    {
        myButton.onClick.AddListener(ToggleVisibility);
    }

    void ToggleVisibility()
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
        
        Debug.Log(firstchoicetext.text);
        
        TextMeshProUGUI[] choiceTexts = new TextMeshProUGUI[] { firstchoicetext, secondchoicetext, thirdchoicetext };
        Debug.Log(choiceTexts);

// Define an array to hold your money texts
        TextMeshProUGUI[] moneyTexts = new TextMeshProUGUI[] { firstmoneytext, secondmoneytext, thirdmoneytext };

// Iterate through choices
        for (int i = 0; i < choiceTexts.Length; i++)
        {
            string selectedChoice = choiceTexts[i].text;

            DatabaseReference choicesref = FirebaseDatabase.DefaultInstance.RootReference.Child("choice_mon_co2");
            choicesref.OrderByChild("Change").EqualTo(selectedChoice).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error querying database: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // Iterate through the results (there should be only one match)
                    foreach (DataSnapshot money_co2Snap in snapshot.Children)
                    {
                        // Extract the "Info" field and set it to info_description
                        object moneyObject = money_co2Snap.Child("Cost").Value;
                        if (moneyObject != null)
                        {
                            moneyTexts[i].text = moneyObject.ToString();
                            Debug.Log("Money inside previous function: " + moneyTexts[i]);
                        }
                    }
                }
            });
        }
        InsertImages();
        UpdateButtonState();
    }

    void InsertImages()
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
        foreach (var button in buttons)
        {
            button.interactable = true;
            SetButtonTransparency(button, 1f);
        }
    }

    void DisableButtons(params Button[] buttons)
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
            SetButtonTransparency(button, 0.5f);
        }
    }

    void SetButtonTransparency(Button button, float transparency)
    {
        Color buttonColor = button.image.color;
        buttonColor.a = transparency;
        button.image.color = buttonColor;
    }
}
