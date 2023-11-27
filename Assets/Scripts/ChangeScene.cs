using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public string nextScene = "";
    public string currentScene = "";

    public Button playButton;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(ChangeSceneFunction);
    }

    public void ChangeSceneFunction()
	{
   		Debug.Log("ChangeSceneFunction called");
    
    	if (SceneManager.GetSceneByName(currentScene).isLoaded)
    	{
        	Debug.Log(currentScene + " is loaded. Unloading...");
        	SceneManager.UnloadSceneAsync(currentScene);
    	}
    	else
    	{
        	Debug.Log(currentScene + " is not loaded.");
    	}
		Debug.Log("Loading " + nextScene);
		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}


}
