using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using Firebase.Auth;
using DG.Tweening;

public class menu_settings_logout : MonoBehaviour
{
    public Image menuBack;
    public Button settings_btn;
    public Button logout_btn;
    public Image menuTitle;
    public Button openMenu_btn;
    public Button closeMenu_btn;
    public string logoutScene = "GameStart";
    public string mainScene = "MainScene";
    // Start is called before the first frame update
    void Start()
    {
        menuBack.enabled = false;
        settings_btn.GameObject().SetActive(false);
        logout_btn.GameObject().SetActive(false);
        menuTitle.enabled = false;
        closeMenu_btn.GameObject().SetActive(false);

        openMenu_btn.onClick.AddListener(openMenu);
        closeMenu_btn.onClick.AddListener(closeMenu);
        logout_btn.onClick.AddListener(logOut);
    }

    public void openMenu()
    {
        settings_btn.GameObject().SetActive(true);
        logout_btn.GameObject().SetActive(true);
        menuTitle.enabled = true;
        closeMenu_btn.GameObject().SetActive(true);
        openMenu_btn.enabled = false;
    }

    public void logOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        if (SceneManager.GetSceneByName(mainScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(mainScene);
        }
        
        SceneManager.LoadScene(logoutScene, LoadSceneMode.Single);
        
    }
    
    public void closeMenu()
    {
        menuBack.enabled = false;
        settings_btn.GameObject().SetActive(false);
        logout_btn.GameObject().SetActive(false);
        menuTitle.enabled = false;
        closeMenu_btn.GameObject().SetActive(false);
        openMenu_btn.enabled = true;
    }
}
