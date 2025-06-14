using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCommodity", menuName = "Commodity")]
public class ShopCommoditySO : ScriptableObject
{
    public ShopCommodityButtons CommodityName;
    public ShopCommodityType CommodityType;
    public int AmountToGain;
    public List<CommodityCost> CommodityCost;
    public Sprite CommodityIcon;
    public ScriptableObject CommodityObject = null;
    public ResourceType ResourceType;

}
