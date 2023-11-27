using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCanvas : MonoBehaviour
{
    public Canvas canvasToDeactivate;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate the canvas in the main scene
        if (canvasToDeactivate != null)
        {
            canvasToDeactivate.gameObject.SetActive(false);
        }
    }
}
