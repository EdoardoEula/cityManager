using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button play;

    public Canvas minigame;
    // Start is called before the first frame update
    void Start()
    {
        minigame.gameObject.SetActive(false);
        play.onClick.AddListener(() => OpenMinigame());
    }

    void OpenMinigame()
    {
        minigame.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
