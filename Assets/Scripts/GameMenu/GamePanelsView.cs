using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelsView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _resourcesPanelButton;
    [SerializeField] private Button _deliveryOrdersPanelButton;
    [SerializeField] private Button _harborPanelButton;
    [SerializeField] private Button _shopPanelButton;
    [SerializeField] private Button _settingsPanelButton;

    [Space(5)]


    [Header("Panels")]
    [SerializeField] private GameObject _resourcesPanel;
    [SerializeField] private GameObject _deliveryOrdersPanel;
    [SerializeField] private GameObject _harborPanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _settingsPanel;

    [Space(5)]

    private GameScreenButtons? _lastClickedPanelButton = null;


    public event Action<GameScreenButtons> OnPanelButtonPressed;






    private void OnEnable()
    {
        _resourcesPanelButton.onClick.AddListener(OnResources);
        _deliveryOrdersPanelButton.onClick.AddListener(OnOrders);
        _harborPanelButton.onClick.AddListener(OnHarbor);
        _shopPanelButton.onClick.AddListener(OnShop);
        _settingsPanelButton.onClick.AddListener(OnSettings);
    }

    private void OnDisable()
    {
        _resourcesPanelButton.onClick.RemoveAllListeners();
        _deliveryOrdersPanelButton.onClick.RemoveAllListeners();
        _harborPanelButton.onClick.RemoveAllListeners();
        _shopPanelButton.onClick.RemoveAllListeners();
        _settingsPanelButton.onClick.RemoveAllListeners();
    }

    public void TogglePanel(GameScreenButtons button)
    {
        if (_lastClickedPanelButton == button)
        {
            DeactivatePanels();
            ResetLastClickedButton();
            return;
        }

        DeactivatePanels();

        switch (button)
        {
            case GameScreenButtons.Resources:
                _resourcesPanel.SetActive(!_resourcesPanel.activeSelf);
                break;

            case GameScreenButtons.DeliveryOrders:
                _deliveryOrdersPanel.SetActive(!_deliveryOrdersPanel.activeSelf);
                break;

            case GameScreenButtons.Harbor:
                _harborPanel.SetActive(!_harborPanel.activeSelf);
                break;

            case GameScreenButtons.Shop:
                _shopPanel.SetActive(!_shopPanel.activeSelf);
                break;

            case GameScreenButtons.Settings:
                _settingsPanel.SetActive(!_settingsPanel.activeSelf);
                break;
        }

        _lastClickedPanelButton = button;

    }

    public void ResetLastClickedButton()
    {
        _lastClickedPanelButton = null;
    }

    private void DeactivatePanels()
    {
        _resourcesPanel.SetActive(false);
        _deliveryOrdersPanel.SetActive(false);
        _harborPanel.SetActive(false);
        _shopPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }


    private void OnResources() => OnPanelButtonPressed?.Invoke(GameScreenButtons.Resources);
    private void OnOrders() => OnPanelButtonPressed?.Invoke(GameScreenButtons.DeliveryOrders);
    private void OnHarbor() => OnPanelButtonPressed?.Invoke(GameScreenButtons.Harbor);
    private void OnShop() => OnPanelButtonPressed?.Invoke(GameScreenButtons.Shop);
    private void OnSettings() => OnPanelButtonPressed?.Invoke(GameScreenButtons.Settings);
}
