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
    public float menuWidth = 200f; // Adjust this value based on your menu width

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
        StartCoroutine(SlideInMenu(menuCanvas));
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
        StartCoroutine(SlideOutMenu(menuCanvas));
        closeMenu_btn.gameObject.SetActive(false);
        openMenu_btn.interactable = true;
    }
    
    private IEnumerator SlideInMenu(Canvas canvas)
    {
        canvas.gameObject.SetActive(true);
        RectTransform menuRect = canvas.GetComponent<RectTransform>();
        float startX = -menuWidth;

        LeanTween.moveX(menuRect, 0f, slideDuration).setEase(LeanTweenType.easeOutQuad);
        
        yield return new WaitForSeconds(slideDuration);
        
        menuRect.anchoredPosition = new Vector2(0f, menuRect.anchoredPosition.y);
    }

    private IEnumerator SlideOutMenu(Canvas canvas)
    {
        RectTransform menuRect = canvas.GetComponent<RectTransform>();
        float endX = -menuWidth;

        LeanTween.moveX(menuRect, endX, slideDuration).setEase(LeanTweenType.easeInQuad);

        yield return new WaitForSeconds(slideDuration);

        canvas.gameObject.SetActive(false);
        menuRect.anchoredPosition = new Vector2(endX, menuRect.anchoredPosition.y);
    }
}


