using UnityEngine;
using DG.Tweening;

public class Reset_animation : MonoBehaviour
{
    private void OnEnable()
    {
        // Stop and reset LeanTween animations
        LeanTween.cancel(gameObject);

        // Stop and reset DOTTween animations
        DOTween.Kill(gameObject);
    }
}
