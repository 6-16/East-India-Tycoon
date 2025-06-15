using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadView : MonoBehaviour
{
    [SerializeField] private Button _saveSettingsMenuButton;
    [SerializeField] private Button _loadSettingsMenuButton;
    [SerializeField] private Button _newGameSettingsMenuButton;
    [SerializeField] private TextMeshProUGUI _saveStatusSettingsMenuText;

    [SerializeField] private Button _loadMainMenuButton;
    [SerializeField] private Button _newGameMainMenuButton;
    [SerializeField] private TextMeshProUGUI _saveStatusMainMenuText;

    [SerializeField] private GameObject _playerHUD_UI;
    [SerializeField] private GameObject _mainMenu_UI;

    [SerializeField] private Button _backToMainMenuButton;

    [SerializeField] private GamePanelsView _gamePanelsView;
    // [SerializeField] private Text _messageText;

    public Action OnSaveClicked { get; set; }
    public Action OnLoadClicked { get; set; }
    public Action OnNewGameClicked { get; set; }

    private void OnEnable()
    {
        _saveSettingsMenuButton.onClick.AddListener(() => OnSaveClicked?.Invoke());

        _loadSettingsMenuButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        _loadMainMenuButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        _loadMainMenuButton.onClick.AddListener(OperateStartGameUI);

        _newGameSettingsMenuButton.onClick.AddListener(() => OnNewGameClicked?.Invoke());
        _newGameSettingsMenuButton.onClick.AddListener(StartNewFromSettingsMenu);
        _newGameMainMenuButton.onClick.AddListener(() => OnNewGameClicked?.Invoke());
        _newGameMainMenuButton.onClick.AddListener(OperateStartGameUI);

        _backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDisable()
    {
        _saveSettingsMenuButton.onClick.RemoveAllListeners();

        _loadSettingsMenuButton.onClick.RemoveAllListeners();
        _loadMainMenuButton.onClick.RemoveAllListeners();

        _newGameSettingsMenuButton.onClick.RemoveAllListeners();
        _newGameMainMenuButton.onClick.RemoveAllListeners();

        _backToMainMenuButton.onClick.RemoveAllListeners();
    }

    private void OperateStartGameUI()
    {
        _mainMenu_UI.SetActive(false);
        _playerHUD_UI.SetActive(true);
    }

    private void BackToMainMenu()
    {
        OnSaveClicked?.Invoke();
        _playerHUD_UI.SetActive(false);
        _mainMenu_UI.SetActive(true);
        _gamePanelsView.TogglePanel(GameScreenButtons.Settings);
    }

    private void StartNewFromSettingsMenu()
    {
        _gamePanelsView.TogglePanel(GameScreenButtons.Settings);
    }

    public void SetLoadButtonEnabled(bool enabled)
    {
        _loadSettingsMenuButton.interactable = enabled;
        _loadMainMenuButton.interactable = enabled;
    }

    public void SetSaveStatus(string status)
    {
        _saveStatusSettingsMenuText.text = status;
        _saveStatusMainMenuText.text = status;
    }

    // public void ShowMessage(string message)
    // {
    //     if (_messageText != null)
    //     {
    //         _messageText.text = message;
    //         Debug.Log(message);
    //     }
    // }
}