using System;
using UnityEngine;

public class ResourcesPresenter : IDisposable
{
    private ResourcesModel _resourcesModel;
    private ResourcesView _resourcesView;



    public event Action ClosePanelEvent;

    public ResourcesPresenter(ResourcesModel model, ResourcesView view)
    {
        _resourcesModel = model;
        _resourcesView = view;
    }

    public void Init()
    {
        Subscribe();
        InitializeUI();

        TestGold();
    }

    private void TestGold()
    {
        _resourcesModel.ChangeResourceAmount(ResourceType.Gold, 10000, true);
    }

    private void Subscribe()
    {
        _resourcesModel.ResourceAmountChanged += OnResourceAmountChanged;
        _resourcesView.OnClosePanelButtonPressed += ClosePanel;
    }

    private void UnSubscribe()
    {
        _resourcesModel.ResourceAmountChanged -= OnResourceAmountChanged;
        _resourcesView.OnClosePanelButtonPressed -= ClosePanel;
    }

    public void RefreshView()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        foreach (var resourceType in _resourcesModel.PlayerResources)
        {
            OnResourceAmountChanged(resourceType.Key, resourceType.Value);
        }
    }

    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ClosePanelEvent?.Invoke();
    }

    private void OnResourceAmountChanged(ResourceType resourceType, int currentAmount)
    {
        _resourcesView.UpdateResourceAmount(resourceType, currentAmount);
    }

    public void Disable() => UnSubscribe();
    public void Dispose()
    {
        UnSubscribe();

        _resourcesModel = null;
        _resourcesView = null;
    }
}
