using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionindustry : MonoBehaviour
{
    
    public CharacterMovementindustry otherCharacterindustry;
    public GameObject iconArrowindustry;
    void Start()
    {
        // Assicurati che l'icona sia attiva all'inizio (se necessario)
        iconArrowindustry.SetActive(true);
    }

    void OnMouseDown()
    {
        otherCharacterindustry.CallCharacterindustry();
        
        if (iconArrowindustry != null)
        {
            iconArrowindustry.SetActive(false);
        }
    }
}