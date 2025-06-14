using System;


public class GamePanelsPresenter : IDisposable
{
    private GamePanelsView _gamePanelsView;

    // Presenters
    private ResourcesPresenter _resourcesPresenter;
    private DeliveriesPresenter _deliveriesPresenter;
    private HarborPresenter _harborPresenter;
    private ShopPresenter _shopPresenter;
    private SettingsPresenter _settingsPresenter;

    public event Action<GameScreenButtons> OnTogglePanel;




    public GamePanelsPresenter(GamePanelsView gamePanelsView, ResourcesPresenter resourcesPresenter,
                                DeliveriesPresenter deliveriesPresenter, HarborPresenter harborPresenter,
                                ShopPresenter shopPresenter, SettingsPresenter settingsPresenter)
    {
        _gamePanelsView = gamePanelsView;
        _resourcesPresenter = resourcesPresenter;
        _deliveriesPresenter = deliveriesPresenter;
        _harborPresenter = harborPresenter;
        _shopPresenter = shopPresenter;
        _settingsPresenter = settingsPresenter;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _gamePanelsView.OnPanelButtonPressed += TriggerTogglePanel;
        _resourcesPresenter.ClosePanelEvent += GameMenuPanelClosed;
        _deliveriesPresenter.ClosePanelEvent += GameMenuPanelClosed;
        _harborPresenter.ClosePanelEvent += GameMenuPanelClosed;
        _shopPresenter.ClosePanelEvent += GameMenuPanelClosed;
        _settingsPresenter.ClosePanelEvent += GameMenuPanelClosed;
    }

    private void UnSubscribe()
    {
        _gamePanelsView.OnPanelButtonPressed -= TriggerTogglePanel;
        _resourcesPresenter.ClosePanelEvent -= GameMenuPanelClosed;
        _deliveriesPresenter.ClosePanelEvent -= GameMenuPanelClosed;
        _harborPresenter.ClosePanelEvent -= GameMenuPanelClosed;
        _shopPresenter.ClosePanelEvent -= GameMenuPanelClosed;
        _settingsPresenter.ClosePanelEvent -= GameMenuPanelClosed;
    }

    private void TriggerTogglePanel(GameScreenButtons button)
    {
        TogglePanel(button);
    }

    private void TogglePanel(GameScreenButtons button)
    {
        _gamePanelsView.TogglePanel(button);

        OnTogglePanel?.Invoke(button);
    }

    private void GameMenuPanelClosed()
    {
        _gamePanelsView.ResetLastClickedButton();
    }

    public void Disable()
    {

    }

    public void Dispose()
    {
        UnSubscribe();
        _gamePanelsView = null;
    }
}