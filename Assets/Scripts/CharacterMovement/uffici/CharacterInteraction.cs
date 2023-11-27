using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    public CharacterMovement otherCharacteruffici;
    public GameObject iconArrowuffici;
    void Start()
    {
        // Assicurati che l'icona sia attiva all'inizio (se necessario)
        iconArrowuffici.SetActive(true);
       
    }

    void OnMouseDown()
    {
        otherCharacteruffici.CallCharacteruffici();  // Chiamata al metodo nel CharacterMovement
        

        // Nascondi l'icona quando il personaggio viene cliccato
        if (iconArrowuffici != null)
        {
            iconArrowuffici.SetActive(false);
        }
        
    }
}
