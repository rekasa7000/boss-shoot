using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coreText;

    public void SetScore(double score)
    {
        coreText.text = $"{score.ToString("F2")} ₱";
    }
}

