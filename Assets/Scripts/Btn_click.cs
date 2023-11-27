using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using TMPro;

public class Btn_click : MonoBehaviour
{
    public Canvas canvasQuestionPanel;
    public GameObject infoPanel;
    public Button myButton;
    public Button myButton2; // Add reference to myButton2
    public Button myButton3; // Add reference to myButton3
    
    public Button myButton4; // Add reference to myButton2
    public Button myButton5; // Add reference to myButton3
    public Button myButton6; // Add reference to myButton2
    public TextMeshProUGUI firstchoicetext;
    public TextMeshProUGUI secondchoicetext;
    public TextMeshProUGUI thirdchoicetext;
    public TextMeshProUGUI firstmoneytext;
    public TextMeshProUGUI secondmoneytext;
    public TextMeshProUGUI thirdmoneytext;
    public Image title;
    public string inputText_firstChoice;
    public string inputText_secondChoice;
    public string inputText_thirdChoice;
    public string inputText_firstMoney;
    public string inputText_secondMoney;
    public string inputText_thirdMoney;
    public Sprite firstImageSprite;
    public Sprite secondImageSprite;
    public Sprite thirdImageSprite;
    public Sprite titleSprite;
    public Image firstImage;
    public Image secondImage;
    public Image thirdImage;
    private bool isCanvasVisible = false;
    private bool clicked_mybutton = false;
    
    

    void Start()
    {
        // Add a listener to the button click event
        myButton.onClick.AddListener(ToggleVisibility);
        UpdateButtonState();
    }
    // Toggle the visibility of the canvas when the button is clicked
    void ToggleVisibility()
    {
        isCanvasVisible = !isCanvasVisible;
        clicked_mybutton = !clicked_mybutton;
        
        canvasQuestionPanel.enabled = isCanvasVisible;
        if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
        }
        firstchoicetext.text = inputText_firstChoice;
        secondchoicetext.text = inputText_secondChoice;
        thirdchoicetext.text = inputText_thirdChoice;
        firstmoneytext.text = inputText_firstMoney;
        secondmoneytext.text = inputText_secondMoney;
        thirdmoneytext.text = inputText_thirdMoney;
        InsertImages();
        UpdateButtonState();
    }

    void InsertImages()
    {
        firstImage.sprite = firstImageSprite;
        secondImage.sprite = secondImageSprite;
        thirdImage.sprite = thirdImageSprite;
        title.sprite = titleSprite;

    }
    void UpdateButtonState()
    {
        if (clicked_mybutton)
        {
            // Disable myButton2 and myButton3 if myButton is clicked
            myButton2.interactable = false;
            myButton3.interactable = false;
            myButton4.interactable = false;
            myButton5.interactable = false;
            myButton6.interactable = false;
            SetButtonTransparency(myButton2, 0.5f);
            SetButtonTransparency(myButton3, 0.5f);
            SetButtonTransparency(myButton4, 0.5f);
            SetButtonTransparency(myButton5, 0.5f);
            SetButtonTransparency(myButton6, 0.5f);
        }
        else
        {
            // Enable myButton2 and myButton3 if myButton is not clicked
            myButton2.interactable = true;
            myButton3.interactable = true;
            myButton4.interactable = true;
            myButton5.interactable = true;
            myButton6.interactable = true;
            SetButtonTransparency(myButton2, 1f);
            SetButtonTransparency(myButton3, 1f);
            SetButtonTransparency(myButton4, 1f);
            SetButtonTransparency(myButton5, 1f);
            SetButtonTransparency(myButton6, 1f);
        }
    }
    void SetButtonTransparency(Button button, float transparency)
    {
        Color buttonColor = button.image.color;
        buttonColor.a = transparency;
        button.image.color = buttonColor;
    }
}
