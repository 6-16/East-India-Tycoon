using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    // Utils
    private TimeController _timeController;
    private DeliveryGenerator _deliveryGenerator;
    private SeaCameraController _seaCameraController;
    [SerializeField] private SunMover _sunMover;
    [SerializeField] private SoundController _soundController;


    // Views
    [SerializeField] private GamePanelsView _gamePanelsView;
    [SerializeField] private ResourcesView _resourcesView;
    [SerializeField] private DeliveriesView _deliveriesView;
    [SerializeField] private HarborView _harborView;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private SettingsView _settingsView;
    [SerializeField] private SaveLoadView _saveLoadView;


    // Presenters
    private SaveLoadPresenter _saveLoadPresenter;
    private GamePanelsPresenter _gamePanelsPresenter;
    private ResourcesPresenter _resourcesPresenter;
    private DeliveriesPresenter _deliveriesPresenter;
    private HarborPresenter _harborPresenter;
    private ShopPresenter _shopPresenter;
    private SettingsPresenter _settingsPresenter;

    // Models
    private ResourcesModel _resourcesModel;
    private HarborModel _harborModel;



    private void Start()
    {
        _resourcesModel = new ResourcesModel();
        _harborModel = new HarborModel();
        // ResourcesModel resourcesModel = new();
        // HarborModel harborModel = new();

        _timeController = new TimeController();

        _resourcesPresenter = new ResourcesPresenter(_resourcesModel, _resourcesView, _saveLoadView);
        _deliveriesPresenter = new DeliveriesPresenter(_deliveriesView);
        _shopPresenter = new ShopPresenter(_shopView, _resourcesModel);
        _harborPresenter = new HarborPresenter(_resourcesModel, _harborView, _harborModel, _deliveriesView, _shopPresenter);
        _settingsPresenter = new SettingsPresenter(_settingsView);
        _saveLoadPresenter = new SaveLoadPresenter(_saveLoadView, _resourcesModel, _harborModel, _timeController, _harborPresenter, _resourcesPresenter);

        _gamePanelsPresenter = new GamePanelsPresenter(_gamePanelsView, _resourcesPresenter, _deliveriesPresenter, _harborPresenter, _shopPresenter, _settingsPresenter);

        _deliveryGenerator = new DeliveryGenerator(_timeController, _deliveriesPresenter);

        _seaCameraController = new SeaCameraController();

        _saveLoadPresenter.OnGameLoaded += RefreshAllViews;
        _saveLoadPresenter.Init();
        _ = _timeController.Init();
        _sunMover.Init(_timeController);

        _resourcesPresenter.Init();
        _deliveriesPresenter.Init(_deliveryGenerator);
        _harborPresenter.Init();
        _shopPresenter.Init();
        _settingsPresenter.Init();

        _gamePanelsPresenter.Init();
        _deliveryGenerator.Init();
        _seaCameraController.Init();

        _deliveriesView.Init(_harborModel, _deliveriesPresenter);
    }

    private void RefreshAllViews()
    {
        _harborPresenter?.RefreshView();
        _harborPresenter.LoadSavedShips();
        if (_harborModel.ActiveShips.Count == 0)
        {
            _harborPresenter?.AddStartingShip();
        }

        _resourcesPresenter?.RefreshView();
    }

    private void OnDisable()
    {
        _timeController.Disable();
        _deliveryGenerator.Disable();
        _gamePanelsPresenter.Disable();
        _seaCameraController.Disable();

        _resourcesPresenter.Disable();
        _deliveriesPresenter.Disable();
        _harborPresenter.Disable();
        _shopPresenter.Disable();
        _settingsPresenter.Disable();
    }

    private void OnDestroy()
    {
        _timeController.Dispose();
        _deliveryGenerator.Dispose();
        _gamePanelsPresenter.Dispose();
        _seaCameraController.Dispose();
        _saveLoadPresenter.Dispose();


        _resourcesPresenter.Dispose();
        _deliveriesPresenter.Dispose();
        _harborPresenter.Dispose();
        _shopPresenter.Dispose();
        _settingsPresenter.Dispose();
    }
}
