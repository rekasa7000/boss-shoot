using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerController : MonoBehaviour
{
    [SerializeField]
    private GameObject _buyManagersPanel;
    [SerializeField]
    private GameObject _buyManagerButtonPrefab;

    private List<UISquadLeadersButton> _buyButtons = new();

    public event Action<int> OnManagerPurchased;

    [SerializeField]
    List<UISquadLeadersButton> buttons;

    [SerializeField]
    private VerticalLayoutGroup _managersLayoutGroup;

    private void Awake()
    {
        _buyManagersPanel.SetActive(false);
    }
    public void AddButton(int index, float price)
    {
        GameObject buttonObject = Instantiate(_buyManagerButtonPrefab);
        UISquadLeadersButton buyButton = buttonObject.GetComponent<UISquadLeadersButton>();
        //UISquadLeadersButton buyButton = buttons[index];
        buyButton.SetValue(price.ToString() + $"₱");
        int i = index;
        buyButton.OnClicked += () => OnManagerPurchased?.Invoke(i);
        buyButton.ToggleActive(false);
        _buyButtons.Add(buyButton);
        buttonObject.transform.SetParent(_buyManagersPanel.transform);
        

    }
    public void ToggleManagerPanel()
    { 
        _buyManagersPanel.SetActive(!_buyManagersPanel.activeSelf);
        if (_buyManagersPanel.activeSelf == false)
            return;
    }

    public void ToggleButton(int index, bool val)
        => _buyButtons[index].ToggleActive(val);

    internal void SetButtonPurchased(int index, bool val)
    {
        if(val)
            _buyButtons[index].SetPurchasedImage();
    }
}
