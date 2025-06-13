using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    public UnityEvent OnUse;
    public event Action OnHit;

    protected abstract void UseWeapon();

    public void Use()
    {
        UseWeapon();
        OnUse?.Invoke();    
    }

    protected void TriggerHit() 
        => OnHit?.Invoke();
}
