using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    public float durataGioco = 30.0f;
    private float tempoIniziale;
    private bool giocoFinito = false;
    public Image iconaGameOver;
    public GameObject Ciclista_0;  // Riferimento al GameObject del giocatore

    void Start()
    {
        if (iconaGameOver != null)
        {
            iconaGameOver.enabled = false;
        }

        tempoIniziale = Time.time;
    }

    void Update()
    {
        if (giocoFinito)
        {
            return;
        }

        if (Time.time - tempoIniziale >= durataGioco)
        {
            Time.timeScale = 0f;
            Debug.Log("Times'up!");

            if (iconaGameOver != null)
            {
                iconaGameOver.enabled = true;
            }

            // Disattiva il componente CharacterController del giocatore (o altri componenti responsabili del movimento)
            if (Ciclista_0 != null)
            {
                CharacterController characterController = Ciclista_0.GetComponent<CharacterController>();
                if (characterController != null)
                {
                    characterController.enabled = false;
                }

                // Oppure, se usi altri metodi di movimento, disattivali di conseguenza
                // player.GetComponent<AltriComponentiDiMovimento>().enabled = false;
            }

            giocoFinito = true;
        }
    }
}
