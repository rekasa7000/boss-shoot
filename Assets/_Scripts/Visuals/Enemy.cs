using System;
using UnityEngine;

/// <summary>
/// Enemy representation
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private AgentHealthUI _enemyHealthUI;
    [SerializeField]
    private HitEffect _bulletHitEffect, _granadeHitEffect, _sniperHitEffect;
    [SerializeField]
    private HitFeedback _hitFeedback;
    [SerializeField]
    private int _health = 200, _maxHealth = 200;

    public event Action OnEnemyDie, OnEnemyLoseLife;

    [SerializeField]
    private AgentCountUI _counUI;
    private int _currentCount = 1;

    public int CurrentCount => _currentCount;

    public int Health  => _health;

    private void Start()
    {
        _enemyHealthUI.SetValue(_health / (float)_maxHealth);   
        _counUI.SetValue(_currentCount);
    }
    public void GetHit(int damage, HitType hitType)
    {
        int unitHealth = _health + _maxHealth * (_currentCount - 1);
        unitHealth -= damage;
        if (unitHealth <= 0)
        {
            Destroy(gameObject);
            OnEnemyDie?.Invoke();
            return;
        }
        int newCount = unitHealth / _maxHealth + 1;
        if (_currentCount > newCount)
            OnEnemyLoseLife?.Invoke();
        SetCount(newCount);
        this._health = unitHealth % _maxHealth;
        _hitFeedback.Play();
        _enemyHealthUI.SetValue(_health / (float)_maxHealth);
        if(hitType == HitType.Gun)
        {
            _bulletHitEffect.Play();
        }
        else if(hitType == HitType.Sniper) 
        {
            _sniperHitEffect.Play();
        }
        else
        {
            _granadeHitEffect.Play();
        }
        
    }

    public void SetCount(int val)
    {
        _currentCount = val;
        _counUI.SetValue(Mathf.Clamp(val, 0, 100));
    }

    public void SetHealth(int health)
    { 
        _health = health;
        _enemyHealthUI.SetValue(_health / (float)_maxHealth);
    }

    public void IncreaseMaxHealth(int additionalHealth)
    {
        _maxHealth += additionalHealth;
        SetHealth(_maxHealth);
    }
}   

public enum HitType
{
    Gun,
    Sniper,
    Explosion
}