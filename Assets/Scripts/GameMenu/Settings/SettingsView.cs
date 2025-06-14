using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _closeSettingsPanelButton;

    [Space(5)]



    [Header("Panel")]
    [SerializeField] private GameObject _settingsPanel;


    public event Action<GameObject> OnClosePanelButtonPressed;



    private void OnEnable()
    {
        _closeSettingsPanelButton.onClick.AddListener(OnClosePanelButton);
    }

    private void OnDisable()
    {
        _closeSettingsPanelButton.onClick.RemoveAllListeners();
    }

    private void OnClosePanelButton()
    {
        OnClosePanelButtonPressed?.Invoke(_settingsPanel);
    }
}
