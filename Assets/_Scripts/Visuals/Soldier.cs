using System;
using UnityEngine;
using UnityEngine.Events;

public class Soldier : MonoBehaviour
{
    [SerializeField]
    private AgentAnimation _anim;

    [SerializeField]
    private AnimationClip _attackAnimation;

    public event Action OnHit;
    [SerializeField]
    private AgentCountUI _count;

    public AgentCountUI Count => _count;

    [SerializeField]
    private Weapon _weapon;

    public UnityEvent OnSoldierAction;

    private void Start()
    {
        _weapon.OnHit += () => OnHit?.Invoke();
    }
    public void Shoot()
    {
        _anim.OnAction += () => {
            OnSoldierAction?.Invoke();
            _weapon.Use();
            };
        _anim.PlayAnimation(_attackAnimation.name);
    }

    public void SetCount(int val)
    {
        _count.SetValue(Mathf.Clamp(val, 0, 99));
    }

    public void ResetOnHit()
        => OnHit = null;
}
