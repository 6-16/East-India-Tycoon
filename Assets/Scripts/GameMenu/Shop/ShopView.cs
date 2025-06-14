using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopView : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _previousPageButton;
    [SerializeField] private Button _closeShopPanelButton;
    [Space(5)]
    [Header("Shop Buttons")]
    [SerializeField] private Button _buyTimberButton;
    [SerializeField] private Button _buySailsButton;
    [SerializeField] private Button _buyNailsButton;
    [SerializeField] private Button _buyTarBarrelsButton;
    [SerializeField] private Button _buyDockButton;
    [SerializeField] private Button _buySmallShipButton;
    [SerializeField] private Button _buyMediumShipButton;
    [SerializeField] private Button _buyLargeShipButton;

    [Space(5)]



    [Header("Panel")]
    [SerializeField] private GameObject _shopPanel;

    [Space(5)]


    [Header("Panel Pages")]
    [SerializeField] private List<GameObject> _shopPagesList;
    [SerializeField] private GameObject _defaultShopPage;
    private GameObject _lastShopPage = null;

    [Space(5)]



    [Header("Shop SO's")]
    [SerializeField] private ShopCommoditySO _timberSO;
    [SerializeField] private ShopCommoditySO _sailsSO;
    [SerializeField] private ShopCommoditySO _nailsSO;
    [SerializeField] private ShopCommoditySO _tarBarrelsSO;
    [SerializeField] private ShopCommoditySO _dockSO;
    [SerializeField] private ShopCommoditySO _smallShipSO;
    [SerializeField] private ShopCommoditySO _mediumShipSO;
    [SerializeField] private ShopCommoditySO _largeShipSO;



    public event Action<List<GameObject>, GameObject> OnNextPageButtonPressed;
    public event Action<List<GameObject>, GameObject> OnPreviousPageButtonPressed;
    public event Action<GameObject> OnClosePanelButtonPressed;
    public event Action<ShopCommodityButtons, ShopCommoditySO> OnCommodityButtonPressed;


    private void OnEnable()
    {
        _nextPageButton.onClick.AddListener(OnNextPageButton);
        _previousPageButton.onClick.AddListener(OnPreviousPageButton);
        _closeShopPanelButton.onClick.AddListener(OnClosePanelButton);

        _buyTimberButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.Timber, _timberSO));
        _buySailsButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.Sails, _sailsSO));
        _buyNailsButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.Nails, _nailsSO));
        _buyTarBarrelsButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.TarBarrels, _tarBarrelsSO));
        _buyDockButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.Dock, _dockSO));
        _buySmallShipButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.SmallShip, _smallShipSO));
        _buyMediumShipButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.MediumShip, _mediumShipSO));
        _buyLargeShipButton.onClick.AddListener(() => OnCommodityButton(ShopCommodityButtons.LargeShip, _largeShipSO));
    }

    private void OnDisable()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _previousPageButton.onClick.RemoveAllListeners();
        _closeShopPanelButton.onClick.RemoveAllListeners();

        _buyTimberButton.onClick.RemoveAllListeners();
        _buySailsButton.onClick.RemoveAllListeners();
        _buyNailsButton.onClick.RemoveAllListeners();
        _buyTarBarrelsButton.onClick.RemoveAllListeners();
        _buyDockButton.onClick.RemoveAllListeners();
        _buySmallShipButton.onClick.RemoveAllListeners();
        _buyMediumShipButton.onClick.RemoveAllListeners();
        _buyLargeShipButton.onClick.RemoveAllListeners();
    }

    private void OnClosePanelButton()
    {
        OnClosePanelButtonPressed?.Invoke(_shopPanel);
    }

    private void OnNextPageButton()
    {
        if (_lastShopPage == null)
        {
            OnNextPageButtonPressed?.Invoke(_shopPagesList, _defaultShopPage);
        }
        else
        {
            OnNextPageButtonPressed?.Invoke(_shopPagesList, _lastShopPage);
        }
    }

    private void OnPreviousPageButton()
    {
        if (_lastShopPage == null)
        {
            OnPreviousPageButtonPressed?.Invoke(_shopPagesList, _defaultShopPage);
        }
        else
        {
            OnPreviousPageButtonPressed?.Invoke(_shopPagesList, _lastShopPage);
        }
    }

    public void SetCurrentActivePage(GameObject page)
    {
        _lastShopPage = page;
    }

    private void OnCommodityButton(ShopCommodityButtons commodityButton, ShopCommoditySO commoditySO)
    {
        OnCommodityButtonPressed?.Invoke(commodityButton, commoditySO);
    }
}
