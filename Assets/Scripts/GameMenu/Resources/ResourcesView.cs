using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ResourcesView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _closeResourcesPanelButton;

    [Space(5)]



    [Header("Panel")]
    [SerializeField] private GameObject _resourcesPanel;

    [Space(5)]



    [Header("Resources / TMP's")]
    [SerializeField] private List<ResourceText> _resourceValueTextList;
    private Dictionary<ResourceType, TextMeshProUGUI> _resourceValueTextDictionary;



    public event Action<GameObject> OnClosePanelButtonPressed;


    private void OnEnable()
    {
        _closeResourcesPanelButton.onClick.AddListener(OnClosePanelButton);
    }

    private void OnDisable()
    {
        _closeResourcesPanelButton.onClick.RemoveAllListeners();
    }

    private void OnClosePanelButton()
    {
        OnClosePanelButtonPressed?.Invoke(_resourcesPanel);
    }

    public void UpdateResourceAmount(ResourceType resourceType, int currentAmount)
    {
        var dictionary = GetResourceValueTextDictionary();

        if (dictionary.TryGetValue(resourceType, out var textObject))
        {
            if (resourceType == ResourceType.Gold)
            {
                textObject.text = $"{currentAmount} £";
            }
            else
            {
                textObject.text = currentAmount.ToString();
            }
        }
    }

    public Dictionary<ResourceType, TextMeshProUGUI> GetResourceValueTextDictionary()
    {
        if (_resourceValueTextDictionary == null)
        {
            _resourceValueTextDictionary = new Dictionary<ResourceType, TextMeshProUGUI>();
            foreach (var valueText in _resourceValueTextList)
            {
                if (valueText.text != null)
                {
                    _resourceValueTextDictionary[valueText.resourceType] = valueText.text;
                }       
            }
        }
        return _resourceValueTextDictionary;
    }
}
