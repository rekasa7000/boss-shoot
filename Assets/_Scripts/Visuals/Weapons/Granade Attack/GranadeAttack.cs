using System;
using System.Collections;
using UnityEngine;

public class GranadeAttack : MonoBehaviour
{
    [SerializeField]
    private Transform _granadeObject;
    [SerializeField]
    private Spline _path;

    [SerializeField]
    private float _movementDelay = 0.5f;

    public event Action OnFinishedMoving;

    private void Start()
    {
        _granadeObject.gameObject.SetActive(false);
    }

    public void PerformAttack()
    {
        StopAllCoroutines();
        StartCoroutine(GranadeThrow());
    }

    private IEnumerator GranadeThrow()
    {
        float currentTime = 0;
        _granadeObject.transform.position = _path.CalculatePosition(0);
        _granadeObject.gameObject.SetActive(true);
        while (currentTime < _movementDelay)
        {
            currentTime += Time.deltaTime;
            _granadeObject.transform.position = _path.CalculatePosition(currentTime / _movementDelay);
            yield return null;
        }
        _granadeObject.gameObject.SetActive(false);
        OnFinishedMoving?.Invoke();
    }

}
