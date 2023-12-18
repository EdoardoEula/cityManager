using UnityEngine;
using DG.Tweening;

public class ClearTweensOnSceneLoad : MonoBehaviour
{
    private void Start()
    {
        // Clear all active DOTTween tweens
        DOTween.Clear();
    }
}