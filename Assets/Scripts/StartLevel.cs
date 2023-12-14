using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
        
        if (moneyText != null)
        {
            moneyText.text = $"{GameManager.money_available}";
        }
        else
        {
            Debug.Log("Money text component not assigned.");
        }
    }
}
