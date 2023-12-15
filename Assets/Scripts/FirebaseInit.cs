using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class FirebaseInit : MonoBehaviour
{
    public InitializationFailedEvent OnInitializationFailed = new InitializationFailedEvent();
    private string nextScene = "GameStart";
    private string currentScene = "Splash";

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                OnInitializationFailed.Invoke(task.Exception);
            }
            else
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
                
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    FirebaseApp app = FirebaseApp.DefaultInstance;
                    DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                });
                
                Debug.Log("Firebase Initialised");
                
            }
        });
    }

    [Serializable]
    public class InitializationFailedEvent : UnityEvent<Exception>
    {
    }
}