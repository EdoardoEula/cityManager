using System.Collections;
using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public interface IDialogueManager
{
    bool HasDialogueEnded();
}

public class ChangeCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform moveTo;
    public Button transitionButton;
    public Canvas canvasToFadeOut;
    public Canvas canvasToFadeIn;
    public float fadeDuration = 1.5f;
    public float movementDuration = 2f;
    public float fadeInDelay = 1f;
    [SerializeField] private Button[] buttons; // Array of buttons to handle
    [SerializeField] private GameObject dialogueManagerObject;
    private IDialogueManager dialogueManager; // Reference to the interface

    public Button farmLandButton;
    private int farmLandClickCount = 0;

    void Start()
    {
        // Check if required components are assigned
        if (mainCamera == null || moveTo == null)
        {
            Debug.LogError("Main Camera or MoveTo Transform not assigned in the Unity Editor.");
            return;
        }

        // Check if Canvas components are assigned
        if (canvasToFadeOut == null || canvasToFadeIn == null)
        {
            Debug.LogError("Canvas not assigned in the Unity Editor.");
            return;
        }

        // Set the canvasToFadeIn to be initially non-visible
        //canvasToFadeIn.gameObject.SetActive(false);

        // Check if the Transition Button is assigned
        if (transitionButton != null)
        {
            // Add listeners for button click events
            transitionButton.onClick.AddListener(() => TransitionFadeOutCanvas(canvasToFadeOut));
            transitionButton.onClick.AddListener(() => StartCoroutine(FadeInCanvasWithDelay(canvasToFadeIn)));
            transitionButton.onClick.AddListener(TransitionToMoveToCamera);
        }
        else
        {
            Debug.LogWarning("Transition Button not assigned in the Unity Editor.");
        }

        // Check if the FarmLand Button is assigned
        if (farmLandButton != null)
        {
            farmLandButton.onClick.AddListener(OnFarmLandButtonClick);
        }
        else
        {
            Debug.LogWarning("FarmLand Button not assigned in the Unity Editor.");
        }

        // Assign the dialogue manager if it's not assigned in the editor
        if (dialogueManagerObject != null)
        {
            dialogueManager = dialogueManagerObject.GetComponent<IDialogueManager>();
        }
    }
    void DisableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }


    private void OnFarmLandButtonClick()
    {
        farmLandClickCount++;

        if (farmLandClickCount > 1)
        {
            EnableButtons();
        }
        else
        {
            DisableButtons();
        }
    }

    
    void EnableButtons()
    {
        // Retrieve currentUser public variable from the script GameManager
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
                Debug.Log(buttonName);

                buttonName = buttonName.Replace("_btn", "");
                Debug.Log(buttonName + "without btn");


                // Check if the button name exists in the Firebase data
                if (buttonsSnapshot.HasChild(buttonName))
                {
                    string buttonValue = buttonsSnapshot.Child(buttonName).Value.ToString();
                    Debug.Log($"{buttonName}: {buttonValue}");

                    // Append '_btn' to the button name
                    string buttonObjectName = buttonValue + "_btn";
                    
                    if (buttonObjectName != null)
                    {
                        Debug.Log("Button Object found");
                        
                        // Set button interactable based on the value from Firebase
                        bool isInteractable = buttonValue.Equals("On", StringComparison.OrdinalIgnoreCase);
                        button.interactable = isInteractable;
                    }
                    else
                    {
                        Debug.Log("Button Object not found");
                    }
                }
                else
                {
                    // Handle the case where the button name is not found in Firebase data
                    Debug.LogWarning($"Button {buttonName} not found in Firebase data.");
                }
            }
        });
    }

    private void TransitionFadeOutCanvas(Canvas canvas)
    {
        StartCoroutine(FadeOutCanvas(canvas));
    }

    private IEnumerator FadeOutCanvas(Canvas canvas)
    {
        float elapsedTime = 0f;
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f; // Ensure the final alpha is 0
        canvasGroup.interactable = false; // Disable interactions while invisible
        canvasGroup.blocksRaycasts = false; // Disable raycasts while invisible
    }

    private IEnumerator FadeInCanvasWithDelay(Canvas canvas)
    {
        yield return new WaitForSeconds(fadeInDelay); // Add a pause before fade-in

        StartCoroutine(FadeInCanvas(canvas));
    }

    private IEnumerator FadeInCanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(true); // Activate the canvas
        float elapsedTime = 0f;
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = canvas.gameObject.AddComponent<CanvasGroup>();
        }

        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f; // Ensure the final alpha is 1
        canvasGroup.interactable = true; // Enable interactions when visible
        canvasGroup.blocksRaycasts = true; // Enable raycasts when visible

        // Check if the dialogue has ended before making buttons interactable
        if (dialogueManager != null)
        {
            bool dialogueEnded = dialogueManager.HasDialogueEnded();
            Debug.Log($"Dialogue Ended: {dialogueEnded}");

            // Increment the FarmLand button click count
            if (farmLandButton != null && dialogueEnded)
            {
                farmLandClickCount++;
            }

            // Make specific buttons non-interactable based on FarmLand button click count
            if (farmLandClickCount < 2)
            {
                DisableButtons();
            }
        }
        else
        {
            Debug.LogWarning("DialogueManager is null.");
        }
    }

    private IEnumerator MoveToTransformCoroutine(Transform targetTransform, float duration)
    {
        Vector3 startingPosition = mainCamera.transform.position;
        Quaternion startingRotation = mainCamera.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            mainCamera.transform.position = Vector3.Lerp(startingPosition, targetTransform.position, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startingRotation, targetTransform.rotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = targetTransform.position; // Ensure the final position is exact
        mainCamera.transform.rotation = targetTransform.rotation; // Ensure the final rotation is exact
    }

    private void TransitionToMoveToCamera()
    {
        StartCoroutine(MoveToTransformCoroutine(moveTo, movementDuration));

        // Check if the dialogue has ended before resetting button interactability
        if (dialogueManager != null && dialogueManager.HasDialogueEnded())
        {
            EnableButtons();
        }
    }
}