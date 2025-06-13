using System;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public double ItemIncome(int itemCount, int bonusMultiplier) => ItemBaseIncome * itemCount * bonusMultiplier;
    //A way to set the base price of purchasing item and the upgrade cost in one method
    public double ItemUpgradePrice(int itemCount)
        => itemCount switch
        {
            0 => ItemStartCost,
            _ => ItemStartCost * Math.Pow(ItemCostMultiFactor, (itemCount + 1))
        };

    //Those parameters are editable though the inspector. Originally i have planed to load this data from a file (for example xls)
    [field: SerializeField]
    public double ItemBaseIncome { get; set; } = 1.67;
    
    [field: SerializeField]
    public double ItemStartCost { get; private set; } = 3.738;
    [field: SerializeField]
    public double ItemCostMultiFactor { get; private set; } = 1.07;
    
    //We set the max count as a limit before we increase the bonus multplier for the score
    public int MaxCount(int bonusMultiplier, int maxCountHelper) =>
        bonusMultiplier switch
        {
            1 => 25,
            2 => 50,
            _ => maxCountHelper
        };

    //Parameter used to caluclate if we should increase the bonus multiplier
    public int BonusMaxCountThreshold => 4;
    //Parameter used to caluclate if we should increase the bonus multiplier
    public int MaxCountIncrement => 100;
    
    [field: SerializeField]
    public float Delay { get; set; } = 0.6f;
    [field: SerializeField]
    public float ManagerPrice { get; set; } = 1000;
    [field: SerializeField]
    public Sprite ItemImage { get; set; }
}
