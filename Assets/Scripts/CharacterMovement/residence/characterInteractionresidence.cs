using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionresidence : MonoBehaviour
{
    
    public CharacterMovementresidence otherCharacterresidenze;
    public GameObject iconArrowresidence;
    void Start()
    {
        // Assicurati che l'icona sia attiva all'inizio (se necessario)
        iconArrowresidence.SetActive(true);
    }

    void OnMouseDown()
    {
        otherCharacterresidenze.CallCharacterresidence();
        
        if (iconArrowresidence != null)
        {
            iconArrowresidence.SetActive(false);
        }
    }
}