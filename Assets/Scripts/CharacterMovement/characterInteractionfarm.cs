using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionfarm : MonoBehaviour
{
    public CharacterMovementfarm otherCharacterfarm;
    public GameObject iconArrowfarm;
    void Start()
    {
        // Assicurati che l'icona sia attiva all'inizio (se necessario)
        iconArrowfarm.SetActive(true);
       
    }

    void OnMouseDown()
    {
        otherCharacterfarm.CallCharacterfarm();  // Chiamata al metodo nel CharacterMovement
        

        // Nascondi l'icona quando il personaggio viene cliccato
        if (iconArrowfarm != null)
        {
            iconArrowfarm.SetActive(false);
        }
        
    }
}