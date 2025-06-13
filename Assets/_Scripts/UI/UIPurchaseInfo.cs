using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPurchaseInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _price;

    [SerializeField]
    private Image _panelImage;

    [SerializeField]
    private Sprite _purchaseReady, _purchaseNotReady;

    public void SetPrice(string price)
        => _price.text = price;

    public void SwapImageReady()
        => _panelImage.sprite = _purchaseReady;

    public void SwapImageNotReady()
        => _panelImage.sprite = _purchaseNotReady;
}