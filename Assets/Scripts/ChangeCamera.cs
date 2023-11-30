using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform moveTo;
    public Button transitionButton;
    public Canvas canvasToFadeOut;
    public Canvas canvasToFadeIn;
    public float fadeDuration = 1.5f;
    public float movementDuration = 2f;
    public float fadeInDelay = 1f; // Add a delay before fade-in

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
    }

    private void TransitionFadeOutCanvas(Canvas canvas)
    {
        StartCoroutine(FadeOutCanvas(canvas));
    }

    private void TransitionToMoveToCamera()
    {
        StartCoroutine(MoveToTransformCoroutine(moveTo, movementDuration));
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
}
