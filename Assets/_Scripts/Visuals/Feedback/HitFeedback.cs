using DG.Tweening;
using UnityEngine;

/// <summary>
/// Makes the enemy Hit feedback using DoTween
/// http://dotween.demigiant.com/documentation.php
/// </summary>
public class HitFeedback : MonoBehaviour
{
    [SerializeField]
    private Transform affectedTransform;
    [SerializeField]
    private SpriteRenderer affectedRenderer;
    [SerializeField, Range(0,1)]
    private float shakeStrength = 0.1f;
    [SerializeField, Range(0, 1)]
    private float shakeDuration = 0.3f;
    [SerializeField, Range(0, 100)]
    private int shakeVibrato = 30;
    public void Play()
    {
        affectedTransform.DOShakePosition(shakeDuration, shakeStrength, shakeVibrato);
        Color c = Color.white;
        c.a = 0;
        affectedRenderer.color = c;
        affectedRenderer.DOColor(Color.white, 0.1f).SetEase(Ease.InSine);
    }
    private void OnDisable()
    {
        affectedTransform.DOKill();
        affectedRenderer.DOKill();
    }
}
