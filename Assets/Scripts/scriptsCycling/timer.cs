using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    public float totalTime = 60f;  // Total time in seconds
    private float currentTime;     // Current time left
    public TextMeshProUGUI timerText;         // Reference to the UI Text element to display the timer

    private void Start()
    {
        currentTime = totalTime;
        UpdateTimerText();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;  // Decrease the timer over time
            UpdateTimerText();
        }
        else
        {
            // Time is up, implement your logic here (e.g., end the game)
            Debug.Log("Time's up!");
        }
    }

    private void UpdateTimerText()
    {
        // Update the UI Text to display the current time
        if (timerText != null)
        {
            timerText.text = " " + Mathf.CeilToInt(currentTime).ToString();
        }
    }
}