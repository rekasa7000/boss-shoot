using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressButon : MonoBehaviour
{
    private bool _isEnabled = true; 
    public bool IsEnabled
    {
        set
        {
            _isEnabled = value;
            _button.interactable = _isEnabled;
        }
        get { return _isEnabled; }
    }

    [SerializeField]
    private Image _panelImage;
    [SerializeField]
    private Sprite _inactiveSprite, _purchaseSprite, _defaultSprite;
    [SerializeField]
    private Button _button;  


    public UnityEvent OnButtonClicked;

    public void ButtonClicked()
    {
        if(_isEnabled == false)
            return;
        OnButtonClicked?.Invoke();
    }

    public void SwapSpriteToInactive()
        => _panelImage.sprite = _inactiveSprite;
    public void SwapSpriteToDefault()
        => _panelImage.sprite = _defaultSprite;
    public void SwapSpriteToPrchasable()
        => _panelImage.sprite = _purchaseSprite;
}
