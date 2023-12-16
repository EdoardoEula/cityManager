using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using DG.Tweening;


public class Choice_click : MonoBehaviour
{
    public Button mychoicebutton;
    public Button yes_btn;
    public Button No_btn;
    public GameObject areusure_panel;
    public GameObject questionpanel;
    public GameObject World;
    public TextMeshProUGUI choice_invest;
    public TextMeshProUGUI invest_to_copy;
    public TextMeshProUGUI Title;
    public Button mychoicebutton2;
    public Button mychoicebutton3;
    private DatabaseReference databaseReference;
    private int money;
    private int co2;
    private int currentMoney;
    private int currentCO2;
    private int queryDatabaseIsFinished;
    private int retrieveMoneyIsFinished;
    private bool isPanelOpen = false;
    public TMP_Text moneyText;
    public Slider co2Slider;
    public Gradient gradient;

    // Start is called before the first frame update
    async void Start()
    {
        areusure_panel.SetActive(false);
        mychoicebutton.onClick.AddListener(ToggleVisibility);
        yes_btn.onClick.AddListener(async () => await ConfirmChoice());
        No_btn.onClick.AddListener(CancelChoice);
    }

    async void ToggleVisibility()
    {
        if (!isPanelOpen)
        {
            if (questionpanel.activeSelf)
            {
                questionpanel.SetActive(false);
            }

            areusure_panel.SetActive(true);
            choice_invest.text = invest_to_copy.text;
            Debug.Log("Choice to invest: " + choice_invest.text);

            mychoicebutton2.interactable = false;
            mychoicebutton3.interactable = false;
        }
        else
        {
            // The areusure_panel is already open, do nothing or handle differently if needed
        }

        isPanelOpen = true;

        //await QueryDatabase(choice_invest.text);
    }

    async Task ConfirmChoice()
    {
        string currentUser = GameManager.currentUser;
        DatabaseReference choiceRef = FirebaseDatabase.DefaultInstance.RootReference.Child("user_choice").Child(currentUser);
        string objectNameToFind = choice_invest.text;

        await QueryDatabase(objectNameToFind);

        GameManager.money_available = currentMoney - money;
        GameManager.level_co2 = currentCO2 - co2;
        Debug.Log("Current Money" + currentMoney);
        Debug.Log("Choice Money" + money);
        Debug.Log("Updated Money" + GameManager.money_available);

        moneyText.text = GameManager.money_available.ToString();
        UpdateCO2Bar();
        
        // Update user_choice in Firebase
        await choiceRef.Child(objectNameToFind).SetValueAsync("Yes");

        // Disable buttons and set colors
        //string buttonName = Title.text + "_btn";
        //Button titleButton = GameObject.Find(buttonName).GetComponent<Button>();
        //titleButton.interactable = false;
        //Color buttonColor = titleButton.image.color;
        //buttonColor.a = 0.5f;
        //titleButton.image.color = buttonColor;

        // Show the question panel
        questionpanel.SetActive(true);

        // Enable buttons
        mychoicebutton2.interactable = true;
        mychoicebutton3.interactable = true;

        // Close areusure panel
        areusure_panel.SetActive(false);
        isPanelOpen = false;

        // After 1 second, set children of World with the same name as choice_invest visible
        Invoke("ShowWorldChildren", 0.2f);
    }

    void ShowWorldChildren()
    {
        string objectNameToFind = choice_invest.text;
        // Find the child with the same name as choice_invest and set it to visible
        Transform child = World.transform.Find(objectNameToFind);
        if (child != null)
        {
            child.GetChild(0).gameObject.SetActive(false);
            child.GetChild(1).gameObject.SetActive(true);
        }
    }

    void CancelChoice()
    {
        areusure_panel.SetActive(false);

        // If needed, add any additional logic for canceling the choice

        questionpanel.SetActive(true);

        mychoicebutton2.interactable = true;
        mychoicebutton3.interactable = true;

        isPanelOpen = false;
    }

    async Task QueryDatabase(string selectedChoice)
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
                object co2Object = money_co2Snap.Child("CO2").Value;

                if (moneyObject != null && int.TryParse(moneyObject.ToString(), out int moneyValue))
                {
                    money = moneyValue;
                    Debug.Log("Money inside previous function: " + money);
                }
                else
                {
                    Debug.LogError("Failed to convert 'Cost' to int");
                }

                if (co2Object != null && int.TryParse(co2Object.ToString(), out int co2Value))
                {
                    co2 = co2Value;
                    Debug.Log(co2);
                }
                else
                {
                    Debug.LogError("Failed to convert 'CO2' to int");
                }
            }

            await RetrievecurrentMoneyCo2(money, co2);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error querying database: " + ex.Message);
        }
    }

    async Task RetrievecurrentMoneyCo2(int money, int co2)
    {
        string currentUser = GameManager.currentUser;
        Debug.Log(money);
        Debug.Log(co2);
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUser);

        try
        {
            DataSnapshot snapshot = await userRef.GetValueAsync();

            object moneyAvailableObject = snapshot.Child("moneyAvailable").Value;
            object co2Object = snapshot.Child("levelCO2").Value;
            Debug.Log(moneyAvailableObject);
            Debug.Log(co2Object);

            if (moneyAvailableObject != null && int.TryParse(moneyAvailableObject.ToString(), out int moneyAvailable))
            {
                currentMoney = moneyAvailable;
                Debug.Log("Current Money: " + currentMoney);

                int updatedMoney = currentMoney - money;
                Debug.Log("Current Money" + currentMoney);
                Debug.Log("Choice Money" + money);
                Debug.Log("Updated Money" + updatedMoney);
                UpdateMoneyInDatabase(updatedMoney);
            }
            else
            {
                Debug.LogError("Failed to retrieve 'moneyAvailable' from user data or convert it to int");
            }
            
            if (co2Object != null && int.TryParse(co2Object.ToString(), out int co2Available))
            {
                currentCO2 = co2Available;
                Debug.Log("Current CO2: " + currentCO2);

                int updatedCO2 = currentCO2 - co2;
                UpdateCO2InDatabase(updatedCO2);
            }
            else
            {
                Debug.LogError("Failed to retrieve 'moneyAvailable' from user data or convert it to int");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error retrieving user data: " + ex.Message);
        }
    }

    async void UpdateMoneyInDatabase(int updatedMoney)
    {
        string currentUser = GameManager.currentUser;
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUser);
        await userRef.Child("moneyAvailable").SetValueAsync(updatedMoney);
    }
    
    async void UpdateCO2InDatabase(int updatedCO2)
    {
        string currentUser = GameManager.currentUser;
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUser);
        await userRef.Child("levelCO2").SetValueAsync(updatedCO2);
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
    }
}

