using System;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    /// <summary>
    /// Reference to the GameData passed by the GameManager
    /// </summary>
    GameData _currentGameData;

    // I want to use events to keep the classes unaware of each other
    public event Action<int, bool> OnModifyManagerAvailability, OnToggleItemActivationState;
    public event Action<int, float> OnStartWorkOnItem;
    public event Action<int> OnActivateItem, OnAutomateItem;
    public event Action<int, GameData> OnUpdateData, OnPerformAction;

    /// <summary>
    /// Handles clicking of the Manager purchas button per each Item (index)
    /// </summary>
    /// <param name="index"></param>
    public void HandleManagerPurchased(int index)
    {
        if (_currentGameData.Managers[index])
            return;
        _currentGameData.Money -= _currentGameData.ItemDataList[index].ManagerPrice;
        _currentGameData.Managers[index] = true;
        Debug.Log($"Purchased a manager for {index}");
        ActivateManagerFor(index);
    }

    /// <summary>
    /// Activates the automation of clicking the button - to implement managers
    /// </summary>
    /// <param name="index"></param>
    private void ActivateManagerFor(int index)
    {
        AutomateTask(index);
        SendDataUpdate();
    }

    /// <summary>
    /// Performs the work of "clicking the button" automatically if we have purchasesd the manager
    /// </summary>
    /// <param name="index"></param>
    public void HandleManager(int index)
    {
        if (_currentGameData.Managers[index])
        {
            AutomateTask(index);
        }
    }

    /// <summary>
    /// Performs the work of "clicking the button" automatically
    /// </summary>
    /// <param name="index"></param>
    private void AutomateTask(int index)
    {
        IncreaseScore(index);
        HandleStartItemProgress(index);
    }

    /// <summary>
    /// Logic to unlock the item (purchase it) before we can use it to make money
    /// </summary>
    /// <param name="index"></param>
    public void PurchaseItemFirstTime(int index)
    {
        _currentGameData.Money -= _currentGameData.ItemDataList[index].ItemUpgradePrice(_currentGameData.ItemCount[index]);
        _currentGameData.ItemCount[index] = 1;
        ActivateItem(index);
    }

    /// <summary>
    /// Activates the Item that was purchased so that we can click it
    /// </summary>
    /// <param name="i"></param>
    private void ActivateItem(int i)
    {
        OnActivateItem?.Invoke(i);
        SendDataUpdate();
    }

    /// <summary>
    /// Adds money to the data and sends the update event
    /// </summary>
    /// <param name="index"></param>
    public void IncreaseScore(int index)
    {
        _currentGameData.Money += _currentGameData.ItemDataList[index].ItemIncome(_currentGameData.ItemCount[index], _currentGameData.ItemBonusMultiplayer[index]);
        SendDataUpdate();
    }

    /// <summary>
    /// Runs the work needed to produce money for a specific item
    /// </summary>
    /// <param name="index"></param>
    public void HandleStartItemProgress(int index)
    {
        OnPerformAction?.Invoke(index, _currentGameData);
        OnStartWorkOnItem?.Invoke(index,_currentGameData.ItemDataList[index].Delay);
    }

    /// <summary>
    /// Handle Upgrading the item by spending the money to increas the income and count
    /// </summary>
    /// <param name="index"></param>
    public void HandleUpgrade(int index)
    {
        _currentGameData.Money -= _currentGameData.ItemDataList[index].ItemUpgradePrice(_currentGameData.ItemCount[index]);
        _currentGameData.ItemCount[index] += 1;
        SendDataUpdate();

    }

    /// <summary>
    /// Handles Loading the Game Data and processing it to send updates to other scripts.
    /// </summary>
    /// <param name="gameDataSave"></param>
    public void LoadGame(string gameDataSave)
    {
        if (String.IsNullOrEmpty(gameDataSave))
            return;
        _currentGameData.SetData(gameDataSave);
        for (int i = 0; i < _currentGameData.ItemDataList.Count; i++)
        {
            if (_currentGameData.ItemCount[i] > 0)
            {
                ActivateItem(i);
            }
            if (_currentGameData.Managers[i])
                ActivateManagerFor(i);
        }
        SendDataUpdate();
    }

    /// <summary>
    /// Prepares the game data when we start the game.
    /// </summary>
    /// <param name="gameData"></param>
    public void PrepareGameData(GameData gameData)
    {
        _currentGameData = gameData;
        for (int i = 0; i < gameData.ItemDataList.Count; i++)
        {
            _currentGameData.ItemCount.Add(i == 0 ? 1 : 0);
            _currentGameData.ItemBonusMultiplayer.Add(1);
            _currentGameData.ItemMaxCountHelper.Add(0);
            _currentGameData.Managers.Add(false);
        }
        SendDataUpdate();
        OnActivateItem?.Invoke(0);
    }

    /// <summary>
    /// Sends update about data changes as an event
    /// </summary>
    private void SendDataUpdate()
    {
        for (int i = 0; i < _currentGameData.ItemDataList.Count; i++)
        {
            UnlockOtherItems(i);
            UnlockManagers(i);
            CheckBonusMultiplier(i);
            OnUpdateData?.Invoke(i, _currentGameData);
        }
    }

    /// <summary>
    /// Logic to unlock managers for purchase
    /// </summary>
    /// <param name="index"></param>
    private void UnlockManagers(int index)
    {
        if (_currentGameData.Managers[index] == false)
        {
            bool val =
                _currentGameData.ItemDataList[index].ManagerPrice < _currentGameData.Money;
            OnModifyManagerAvailability?.Invoke(index, val);
        }
    }

    /// <summary>
    /// Logic to unlock other items for purchase when we have enough money
    /// </summary>
    /// <param name="index"></param>
    private void UnlockOtherItems(int index)
    {
        if (_currentGameData.ItemCount[index] == 0)
        {
            bool val =
                _currentGameData.ItemDataList[index].ItemUpgradePrice(_currentGameData.ItemCount[index])
                < _currentGameData.Money;
            OnToggleItemActivationState?.Invoke(index, val);
        }
    }

    /// <summary>
    /// Handles Bonus Multiplier so that we have a better balancing 
    /// https://www.gamedeveloper.com/design/the-math-of-idle-games-part-i
    /// </summary>
    /// <param name="index"></param>
    private void CheckBonusMultiplier(int index)
    {
        if (_currentGameData.ItemCount[index]
                    >= _currentGameData.ItemDataList[index].MaxCount(_currentGameData.ItemBonusMultiplayer[index], _currentGameData.ItemMaxCountHelper[index]))
        {
            _currentGameData.ItemBonusMultiplayer[index] *= 2;
            if (_currentGameData.ItemBonusMultiplayer[index] >= _currentGameData.ItemDataList[index].BonusMaxCountThreshold)
                _currentGameData.ItemMaxCountHelper[index] += _currentGameData.ItemDataList[index].MaxCountIncrement;
        }
    }
}

