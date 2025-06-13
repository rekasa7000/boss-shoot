using TMPro;
using UnityEngine;

public class AgentCountUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;

    public void SetValue(int val) => countText.text = "lvl " + val.ToString();
}
