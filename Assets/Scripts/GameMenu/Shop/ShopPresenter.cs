using System;
using UnityEngine;
using System.Collections.Generic;

public class ShopPresenter : IDisposable
{
    private ShopView _shopView;
    private ResourcesModel _resourcesModel;

    // UI Events
    public event Action ClosePanelEvent;
    // Logic Events
    public event Action<ShopCommoditySO> OnBuildingBought;
    public event Action<ShopCommoditySO> OnShipBought;


    public ShopPresenter(ShopView view, ResourcesModel resourcesModel)
    {
        _shopView = view;
        _resourcesModel = resourcesModel;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _shopView.OnNextPageButtonPressed += (pages, current) => OnShopPageChanged(pages, current, +1);
        _shopView.OnPreviousPageButtonPressed += (pages, current) => OnShopPageChanged(pages, current, -1);
        _shopView.OnClosePanelButtonPressed += ClosePanel;
        _shopView.OnCommodityButtonPressed += BuyCommodity;
    }

    private void UnSubscribe()
    {
        _shopView.OnNextPageButtonPressed -= (pages, current) => OnShopPageChanged(pages, current, +1);
        _shopView.OnPreviousPageButtonPressed -= (pages, current) => OnShopPageChanged(pages, current, -1);
        _shopView.OnClosePanelButtonPressed -= ClosePanel;
        _shopView.OnCommodityButtonPressed -= BuyCommodity;
    }

    private void OnShopPageChanged(List<GameObject> pagesList, GameObject lastActivePage, int direction)
    {
        var newActivePage = GetPage(pagesList, lastActivePage, direction);
        ActivateOnly(newActivePage, pagesList);
        _shopView.SetCurrentActivePage(newActivePage);
    }

    private GameObject GetPage(List<GameObject> pages, GameObject current, int direction)
    {
        if (pages == null || pages.Count == 0) return null;

        int currentIndex = pages.IndexOf(current);
        int newIndex = (currentIndex + direction + pages.Count) % pages.Count;

        return pages[newIndex];
    }

    private void ActivateOnly(GameObject activePage, List<GameObject> allPages)
    {
        foreach (var page in allPages)
            page.SetActive(false);

        activePage.SetActive(true);
    }

    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ClosePanelEvent?.Invoke();
    }

    private void BuyCommodity(ShopCommodityButtons pressedShopButton, ShopCommoditySO commoditySO)
    {
        var costs = commoditySO.CommodityCost;

        if (!_resourcesModel.HasEnoughResources(costs))
        {
            Debug.LogWarning("Not enough resources to make this purchase.");
            return;
        }

        _resourcesModel.DeductResources(costs);

        switch (commoditySO.CommodityType)
        {
            case ShopCommodityType.Resource:
                _resourcesModel.ChangeResourceAmount(commoditySO.ResourceType, commoditySO.AmountToGain, true);
                break;

            case ShopCommodityType.Building:
                OnBuildingBought?.Invoke(commoditySO);
                break;

            case ShopCommodityType.Ship:
                OnShipBought?.Invoke(commoditySO);
                break;

            default:

                break;
        }
    }


    public void Disable() => UnSubscribe();
    public void Dispose()
    {
        UnSubscribe();

        _shopView = null;
    }
}
