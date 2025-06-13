using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Handles Ui progress bar
/// </summary>
public class ProgressBar : MonoBehaviour
{
    public UnityEvent OnProgressBarFinished;

    [SerializeField]
    private Image _progressBarImage;

    public void RunProgressBar(float delay)
        =>StartCoroutine(MakeProgress(delay));

    private IEnumerator MakeProgress(float delay)
    {
        float timePassed = 0;
        while(timePassed < delay)
        {
            timePassed += Time.deltaTime;
            float progress = Mathf.Clamp01(timePassed / delay);
            _progressBarImage.fillAmount = progress;
            yield return null;
        }
        _progressBarImage.fillAmount = 0;
        OnProgressBarFinished?.Invoke();
    }

    public void ResetProgress()
    {
        StopAllCoroutines();
        _progressBarImage.fillAmount = 0;
    }
}
