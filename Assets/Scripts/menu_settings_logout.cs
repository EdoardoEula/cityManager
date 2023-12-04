using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using static LeanTween;

public class menu_settings_logout : MonoBehaviour
{
    public Button openMenu_btn;
    public Button closeMenu_btn;
    public Button logout_btn;
    public Canvas menuCanvas;
    public int slideDuration = 1;
    public Transform menuInPosition;
    public Transform menuOutPosition;

    void Start()
    {
        closeMenu_btn.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(false);

        openMenu_btn.onClick.AddListener(OpenMenu);
        closeMenu_btn.onClick.AddListener(CloseMenu);
        logout_btn.onClick.AddListener(LogOut);
    }

    public void OpenMenu()
    {
        StartCoroutine(SlideMenu(menuCanvas, menuInPosition));
        closeMenu_btn.gameObject.SetActive(true);
        openMenu_btn.interactable = false;
    }

    public void LogOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("GameStart", LoadSceneMode.Single);
    }
    
    public void CloseMenu()
    {
        StartCoroutine(SlideMenu(menuCanvas, menuOutPosition));
        closeMenu_btn.gameObject.SetActive(false);
        openMenu_btn.interactable = true;
    }
    
    private IEnumerator SlideMenu(Canvas canvas, Transform targetPosition)
    {
        canvas.gameObject.SetActive(true);
        // Calculate the local position of the targetPosition relative to the canvas
        RectTransform menuRect = canvas.GetComponent<RectTransform>();
        Vector2 targetLocalPosition = new Vector2(menuRect.position.x, menuRect.position.y);

        // Calculate the correct anchored position by adding the canvas size to the targetLocalPosition
        Vector2 correctAnchoredPosition = targetPosition.position;

        LeanTween.move(menuRect, correctAnchoredPosition, slideDuration).setEase(LeanTweenType.easeInOutQuad);

        yield return new WaitForSeconds(slideDuration);

        // Reset the anchored position after the animation to avoid cumulative offsets
        menuRect.anchoredPosition = correctAnchoredPosition;
    }
    
}



