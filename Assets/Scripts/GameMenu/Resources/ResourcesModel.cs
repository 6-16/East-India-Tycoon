using System;
using System.Collections.Generic;

public class ResourcesModel
{
    public Dictionary<ResourceType, int> PlayerResources = new();

    public event Action<ResourceType, int> ResourceAmountChanged;



    public ResourcesModel()
    {
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            PlayerResources[resourceType] = 0;
        }
    }



    public void ChangeResourceAmount(ResourceType resourceType, int amount, bool isIncrementing)
    {
        PlayerResources[resourceType] += isIncrementing ? amount : -amount;

        ResourceChanged(resourceType, PlayerResources[resourceType], amount);
    }

    public bool HasEnoughResources(List<CommodityCost> costs)
    {
        foreach (var cost in costs)
        {
            if (!PlayerResources.ContainsKey(cost.ResourceType) || PlayerResources[cost.ResourceType] < cost.Amount)
                return false;
        }
        return true;
    }

    public void DeductResources(List<CommodityCost> costs)
    {
        foreach (var cost in costs)
        {
            ChangeResourceAmount(cost.ResourceType, cost.Amount, false);
        }
    }

    private void ResourceChanged(ResourceType resourceType, int currentAmount, int gainedAmount) => ResourceAmountChanged?.Invoke(resourceType, currentAmount);
}
