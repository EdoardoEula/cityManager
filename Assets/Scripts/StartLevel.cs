using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
public class StartLevel : MonoBehaviour
{
    public TMP_Text moneyText;

    void Start()
    {
            // Set the text of the moneyText component
            if (moneyText != null)
            {
                moneyText.text = $"{GameManager.money_available}";
            }
            else
            {
                Debug.LogWarning("Money text component not assigned.");
            }
    }

    private void Update()
    {
    }
}
