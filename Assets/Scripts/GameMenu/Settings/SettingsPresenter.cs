using System;
using UnityEngine;

public class SettingsPresenter : IDisposable
{
    private SettingsView _settingsView;

    public event Action ClosePanelEvent;


    public SettingsPresenter(SettingsView view)
    {
        _settingsView = view;
    }

    public void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _settingsView.OnClosePanelButtonPressed += ClosePanel;
    }

    private void UnSubscribe()
    {
        _settingsView.OnClosePanelButtonPressed -= ClosePanel;
    }

    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ClosePanelEvent?.Invoke();
    }

    public void Disable() => UnSubscribe();
    public void Dispose()
    {
        UnSubscribe();

        _settingsView = null;
    }
}
