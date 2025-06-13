using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles the visualization in a way that it is separate from the main Incremental game logic
/// </summary>
public class VisualsController : MonoBehaviour
{
    [SerializeField]
    private List<Soldier> _soldiers = new List<Soldier>();

    [SerializeField]
    private Enemy _enemyBasicPrefab;

    private Enemy _currentEnemy;

    [SerializeField]
    private Transform _enemySpawnPoint, _enemySpawnPointStart;

    [SerializeField]
    private float _movementDelay = 0.2f;

    private Coroutine _movementCoroutine;

    int _enemyCount = 0;

    public UnityEvent OnEnemyKilled, OnEnemyLoseLife;

    public void InitializeVisual(GameData gameData)
    {
        
        if (_currentEnemy == null)
        {
            _enemyCount = 1;
            SpawnNewEnemy();
        }
        SetSoldierCount(0, gameData.ItemCount[0]);

    }

    private void SpawnNewEnemy()
    {
        if(_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
        }
        _currentEnemy = Instantiate(_enemyBasicPrefab, _enemySpawnPoint.position, Quaternion.identity, _enemySpawnPointStart);
        _movementCoroutine = StartCoroutine(MoveEnemyForward());
        _currentEnemy.OnEnemyDie += SpawnNewEnemy;
        _currentEnemy.OnEnemyDie += EnemyKilledFeedback;
        _currentEnemy.OnEnemyLoseLife += EnemyLoseLifeFeedback;
        _currentEnemy.SetCount(_enemyCount);
        _enemyCount++;
    }

    private void EnemyLoseLifeFeedback()
        => OnEnemyLoseLife?.Invoke();

    private void EnemyKilledFeedback()
        => OnEnemyKilled?.Invoke();

    private IEnumerator MoveEnemyForward()
    {
        float elapsedTime = 0;

        while (elapsedTime < _movementDelay)
        {
            _currentEnemy.transform.position = Vector3.Lerp(_enemySpawnPointStart.position, _enemySpawnPoint.position, (elapsedTime / _movementDelay));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _currentEnemy.transform.position = _enemySpawnPoint.position;
        _movementCoroutine = null;
    }

    public void UpdateVisuals(int index ,GameData gameData)
    {
        SetSoldierCount(index, gameData.ItemCount[index]);
    }

    public void PerformAction(int index, GameData gameData)
    {
        if (_soldiers.Count > index)
        {
            _soldiers[index].Shoot();
            _soldiers[index].OnHit += () => HitEnemy(index, gameData.ItemCount[index]);
        }
           
    }

    private void SetSoldierCount(int index, int count)
    {
        if (_soldiers.Count > index)
        {
            _soldiers[index].SetCount(count);
            if(count >= 1)
            {
                _soldiers[index].gameObject.SetActive(true);
            }
        }
            
    }

    public void HitEnemy(int index, int solidersCount)
    {
        _currentEnemy.GetHit(solidersCount, GetHitType(index));
        _soldiers[index].ResetOnHit();
    }

    private HitType GetHitType(int index)
        => index switch
        {
            0 => HitType.Gun,
            1 => HitType.Sniper,
            _ => HitType.Explosion
        };

    /// <summary>
    /// Enemies have theire Health and Count increased to give an impression of progress.
    /// We need to be able to save and load this data
    /// </summary>
    /// <returns></returns>
    public string GetSaveData()
    {
        if(_currentEnemy != null && _currentEnemy.CurrentCount > 0)
            return JsonUtility.ToJson(new VisualData
            {
                EnemyCount = _currentEnemy.CurrentCount,
                EnemyHealth = _currentEnemy.Health
            });
        return String.Empty;
    }

    /// <summary>
    /// Enemies have theire Health and Count increased to give an impression of progress.
    /// We need to be able to save and load this data
    /// </summary>
    /// <param name="stringData"></param>
    public void LoadData(string stringData)
    {
        if(String.IsNullOrEmpty(stringData))
            return;
        VisualData loadedData = JsonUtility.FromJson<VisualData>(stringData);
        if(_currentEnemy == null)
        {
            SpawnNewEnemy();
            _currentEnemy.OnEnemyDie += SpawnNewEnemy;
        }
        StopCoroutine(_movementCoroutine);
        _movementCoroutine = null;
        _currentEnemy.transform.position = _enemySpawnPoint.position;
        if (loadedData.EnemyCount > 0) 
        {
            _currentEnemy.SetCount(loadedData.EnemyCount);
            _enemyCount = loadedData.EnemyCount;
            if (loadedData.EnemyHealth > 0)
            {
                _currentEnemy.SetHealth(loadedData.EnemyHealth);
            }

        }
    }
}

/// <summary>
/// Helper data sctructore for saving the Visual data
/// </summary>
[Serializable]
public struct VisualData
{
    public int EnemyCount;
    public int EnemyHealth;
}

