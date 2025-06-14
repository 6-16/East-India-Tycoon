public class SaveLoadPresenter
{
    private readonly SaveLoadService _saveLoadService;
    private readonly ResourcesModel _resourcesModel;
    private readonly ResourcesPresenter _resourcesPresenter;
    private readonly HarborModel _harborModel;
    private readonly HarborPresenter _harborPresenter;
    private readonly TimeController _timeController;
    private readonly SaveLoadView _view;

    public SaveLoadPresenter(SaveLoadView view, ResourcesModel resourcesModel, 
                           HarborModel harborModel, TimeController timeController, HarborPresenter harborPresenter, ResourcesPresenter resourcesPresenter)
    {
        _view = view;
        _resourcesModel = resourcesModel;
        _harborModel = harborModel;
        _timeController = timeController;
        _harborPresenter = harborPresenter;
        _resourcesPresenter = resourcesPresenter;
        _saveLoadService = new SaveLoadService();
    }

    public void Init()
    {
        _view.OnSaveClicked += SaveGame;
        _view.OnLoadClicked += LoadGame;
        _view.OnNewGameClicked += NewGame;
        
        UpdateView();
    }

    public void Dispose()
    {
        _view.OnSaveClicked -= SaveGame;
        _view.OnLoadClicked -= LoadGame;
        _view.OnNewGameClicked -= NewGame;
    }

    private void SaveGame()
    {
        try
        {
            _saveLoadService.SaveGame(_resourcesModel, _harborModel, _timeController);
            _view.ShowMessage("Game Saved!");
            UpdateView();
        }
        catch (System.Exception e)
        {
            _view.ShowMessage($"Save Failed: {e.Message}");
        }
    }

    private void LoadGame()
    {
        try
        {
            _harborPresenter.ClearAllShips();

            _saveLoadService.LoadGame(_resourcesModel, _harborModel, _timeController);
            _view.ShowMessage("Game Loaded!");
            OnGameLoaded?.Invoke();
        }
        catch (System.Exception e)
        {
            _view.ShowMessage($"Load Failed: {e.Message}");
        }
    }

    private void NewGame()
    {
        _saveLoadService.DeleteSaveFile();

        _harborPresenter.ClearAllShips();

        _resourcesModel.PlayerResources.Clear();
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
            {
                _resourcesModel.PlayerResources[resourceType] = 0;
            }

        _harborModel.ActiveShips.Clear();
        _harborModel.WorkingDocksList.Clear();
        _harborModel.ShipsDockingPositions.Clear();
        _harborModel.MaxDockSlots = 3; 
        _harborModel.OccupiedDockSlots = 0;
        _harborPresenter.AddStartingShip();
        
        _view.ShowMessage("New Game Started!");
        OnGameLoaded?.Invoke(); 
        UpdateView();
    }

    private void UpdateView()
    {
        bool hasSave = _saveLoadService.HasSaveFile();
        _view.SetLoadButtonEnabled(hasSave);
        _view.SetSaveStatus(hasSave ? "Save Available" : "No Save");
    }

    public System.Action OnGameLoaded { get; set; }
}