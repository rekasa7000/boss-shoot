using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    [SerializeField]
    private ProgressBar _progressBar;
    [SerializeField]
    private ProgressButon _progressButton;
    [SerializeField]
    private TextMeshProUGUI _itemScore;
    [SerializeField]
    private Button _buyButton;
    [SerializeField]
    private TextMeshProUGUI _buyButtonText;
    [SerializeField]
    private TextMeshProUGUI _itemCount;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private UIPurchaseInfo _purchaseInfo;
    [SerializeField]
    private GameObject _buyPanel;

    public event Action OnProgressButtonClicked, OnWorkFinished, OnBuyButtonClicked, OnFirstActivation;

    public bool isWorking => _progressButton.IsEnabled == false;
    private void Awake()
    {
        _progressButton.OnButtonClicked.AddListener(HandleFirstClick);
        _progressBar.OnProgressBarFinished.AddListener(HandleProgressBarFinished);
        _buyButton.onClick.AddListener(HandleBuyButton);
        ToggleBuyButton(false);
        _progressButton.IsEnabled = false;
        ToggleIncome(false);
        _progressButton.SwapSpriteToInactive();
    }

    public void Prepare(Sprite icon)
    {
        _itemImage.sprite = icon;
    }

    public void ActivateButton()
    {
        ToggleIncome(true);
        _purchaseInfo.gameObject.SetActive(false);
        _progressBar.gameObject.SetActive(true);
        _buyPanel.SetActive(true);
        _progressButton.IsEnabled = true;
        _progressButton.SwapSpriteToDefault();
        _progressButton.OnButtonClicked.RemoveAllListeners();
        _progressButton.OnButtonClicked.AddListener(HandleProgressButtonClick);
    }

    internal void ResetEvents()
    {
        OnProgressButtonClicked = null;
        OnWorkFinished = null;
        OnBuyButtonClicked = null;
        OnFirstActivation = null;
    }

    public void ToggleActivation(bool val)
    {
        if (val)
        {
            _progressButton.IsEnabled = true;
            _progressButton.SwapSpriteToPrchasable();
            _purchaseInfo.SwapImageReady();
        }
        else
        {
            _progressButton.IsEnabled = false;
            _progressButton.SwapSpriteToInactive();
            _purchaseInfo.SwapImageNotReady();
        }
        
    }

    public void AutomateButton()
    {
        _progressButton.IsEnabled = false;
        OnWorkFinished = null;
        _progressButton.SwapSpriteToDefault();
        _progressBar.ResetProgress();
    }

    private void HandleFirstClick()
    {
        OnFirstActivation?.Invoke();
        ActivateButton();
    }

    private void HandleProgressButtonClick()
    {
        OnProgressButtonClicked?.Invoke();
    }

    private void HandleBuyButton()
        => OnBuyButtonClicked?.Invoke();

    private string setLabelFromKey(int key)
    {
        if (key == 1)
        {
            return "Purchase Sniper";
        } else if (key == 2) {
            return "Purchase Grenadier";
        }
        return "Purchase Soldier";
    }
    public void SetBuyPrice(double price, int key)
    {
        if (_purchaseInfo.isActiveAndEnabled)
        {
            _purchaseInfo.SetPrice($"{price.ToString("F2")} ₱");
            return;
        }
        _buyButtonText.text = $"{price.ToString("F2")} ₱";
    }

    public void SetItemCount(int count, int maxCount)
        => _itemCount.text = $"{count} / {maxCount}";

    public void ToggleBuyButton(bool val)
        => _buyButton.interactable = val;

    public void StartWork(float delay)
    {
        _progressButton.IsEnabled = false;
        _progressButton.SwapSpriteToInactive();
        _progressBar.RunProgressBar(delay);
    }

    public void ToggleIncome(bool val)
        => _itemScore.gameObject.SetActive(val);
    public void SetIncome(double score)
    {
        _itemScore.text = $"{score.ToString("F2")} ₱";
    }

    private void HandleProgressBarFinished()
    {
        _progressButton.IsEnabled = true;
        _progressButton.SwapSpriteToDefault();
        OnWorkFinished?.Invoke();
    }

    private void OnDisable()
    {
        _progressButton.OnButtonClicked.RemoveListener(HandleProgressButtonClick);
        _progressBar.OnProgressBarFinished.RemoveListener(HandleProgressBarFinished);
        _buyButton.onClick.RemoveListener(HandleBuyButton);
    }
}
