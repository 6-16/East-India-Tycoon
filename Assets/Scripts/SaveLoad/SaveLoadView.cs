using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadView : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Text _statusText;
    [SerializeField] private Text _messageText;

    public Action OnSaveClicked { get; set; }
    public Action OnLoadClicked { get; set; }
    public Action OnNewGameClicked { get; set; }

    private void Start()
    {
        _saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
        _loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        _newGameButton.onClick.AddListener(() => OnNewGameClicked?.Invoke());
    }

    public void SetLoadButtonEnabled(bool enabled)
    {
        _loadButton.interactable = enabled;
    }

    public void SetSaveStatus(string status)
    {
        if (_statusText != null)
            _statusText.text = status;
    }

    public void ShowMessage(string message)
    {
        if (_messageText != null)
        {
            _messageText.text = message;
            Debug.Log(message);
        }
    }
}