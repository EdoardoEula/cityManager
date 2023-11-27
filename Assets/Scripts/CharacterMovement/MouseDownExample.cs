using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MouseHoverAndClickExample : MonoBehaviour
{
    public GameObject SM_Chr_Farmer_Female_01;
    public GameObject CharacterButton;
    public GameObject farmIndicationsCanvas;  // Reference to the FarmIndications canvas

    private Renderer characterRenderer;
    public Material ClickedMat;
    private Material originalMaterial;

    private bool isClicked = false;

    void Start()
    {
        characterRenderer = SM_Chr_Farmer_Female_01.GetComponent<Renderer>();

        if (IsObjectValid(SM_Chr_Farmer_Female_01))
        {
            originalMaterial = characterRenderer.material;
        }
        else
        {
            Debug.LogError("Renderer component not found on the character GameObject.");
        }

        // Disable the FarmIndications canvas at the beginning
        if (farmIndicationsCanvas != null)
        {
            farmIndicationsCanvas.GetComponent<Canvas>().enabled = false;
        }
    }

    void OnMouseEnter()
    {
        if (IsObjectValid(SM_Chr_Farmer_Female_01))
        {
            ChangeColor();
        }
    }

    void OnMouseExit()
    {
        if (IsObjectValid(SM_Chr_Farmer_Female_01))
        {
            ResetColor();
        }
    }

    void OnMouseDown()
    {
        isClicked = !isClicked;

        if (IsObjectValid(SM_Chr_Farmer_Female_01))
        {
            if (isClicked)
            {
                ChangeColor();

                // StartCoroutine to wait for the next frame before handling disappearance
                StartCoroutine(DisappearObject(SM_Chr_Farmer_Female_01));
                StartCoroutine(DisappearObject(CharacterButton));

                // Enable the FarmIndications canvas when the object disappears
                if (farmIndicationsCanvas != null)
                {
                    farmIndicationsCanvas.GetComponent<Canvas>().enabled = true;
                }
            }
            else
            {
                ResetColor();
            }
        }
    }

    IEnumerator DisappearObject(GameObject obj)
    {
        yield return null; // Wait for the next frame

        if (IsObjectValid(obj))
        {
            obj.SetActive(false);
            Destroy(obj);
        }
        else
        {
            Debug.LogError("Object is null or inactive. Cannot disappear.");
        }
    }

    bool IsObjectValid(GameObject obj)
    {
        return obj != null && obj.activeSelf;
    }

    void ChangeColor()
    {
        if (IsObjectValid(SM_Chr_Farmer_Female_01))
        {
            characterRenderer.material = ClickedMat;
        }
        else
        {
            Debug.LogError("Renderer component not found on the character GameObject.");
        }
    }

    void ResetColor()
    {
        if (IsObjectValid(SM_Chr_Farmer_Female_01))
        {
            characterRenderer.material = originalMaterial;
        }
        else
        {
            Debug.LogError("Renderer component not found on the character GameObject.");
        }
    }
}
