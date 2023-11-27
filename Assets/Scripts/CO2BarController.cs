using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CO2BarController : MonoBehaviour
{
    public Slider co2Slider;
    public Gradient gradient;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCO2Bar();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for changes in GameManager.level_co2
        if (GameManager.level_co2 != co2Slider.value)
        {
            // Update the CO2 bar
            UpdateCO2Bar();
        }
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
