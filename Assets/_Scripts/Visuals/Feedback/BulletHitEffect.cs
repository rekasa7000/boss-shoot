using UnityEngine;
using UnityEngine.Events;

public class HitEffect : MonoBehaviour
{
    public UnityEvent OnHitEffectPlay;

    public void Play()
    {
        OnHitEffectPlay?.Invoke();  
    }
}
