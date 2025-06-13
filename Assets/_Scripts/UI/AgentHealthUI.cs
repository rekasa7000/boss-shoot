using UnityEngine;
using UnityEngine.UI;

public class AgentHealthUI : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    public void SetValue(float value01)
    {
        healthImage.fillAmount = value01;
    }
}
