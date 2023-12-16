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
    private int queryDatabaseIsFinished;
    private int retrieveMoneyIsFinished;
    private bool isPanelOpen = false;

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

        int updatedMoney = currentMoney - money;
        Debug.Log("Current Money" + currentMoney);
        Debug.Log("Choice Money" + money);
        Debug.Log("Updated Money" + updatedMoney);

        // Update user_choice in Firebase
        await choiceRef.Child(objectNameToFind).SetValueAsync("Yes");

        // Disable buttons and set colors
        // string buttonName = Title.text + "_btn";
        // Button titleButton = GameObject.Find(buttonName).GetComponent<Button>();
        // titleButton.interactable = false;
        // Color buttonColor = titleButton.image.color;
        // buttonColor.a = 0.5f;
        // titleButton.image.color = buttonColor;

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

            await RetrievecurrentMoney(money);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error querying database: " + ex.Message);
        }
    }

    async Task RetrievecurrentMoney(int money)
    {
        string currentUser = GameManager.currentUser;
        Debug.Log(money);
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(currentUser);

        try
        {
            DataSnapshot snapshot = await userRef.GetValueAsync();

            object moneyAvailableObject = snapshot.Child("moneyAvailable").Value;
            Debug.Log(moneyAvailableObject);

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
}

