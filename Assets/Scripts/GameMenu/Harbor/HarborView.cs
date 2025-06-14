using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class HarborView : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _closeHarborPanelButton;

    [Space(5)]



    [Header("Panel")]
    [SerializeField] private GameObject _harborPanel;


    [SerializeField] private GameObject _shipSlotPrefab;
    [SerializeField] private GameObject _shipSlotsContainer;
    public List<GameObject> DockObjects;

    public List<ShipSO> ShipsSOList;

    public Transform DeliveryPoint;
    public Transform Checkpoint;




    public event Action<GameObject> OnClosePanelButtonPressed;



    private void OnEnable()
    {
        _closeHarborPanelButton.onClick.AddListener(OnClosePanelButton);
    }

    private void OnDisable()
    {
        _closeHarborPanelButton.onClick.RemoveAllListeners();
    }

    private void OnClosePanelButton()
    {
        OnClosePanelButtonPressed?.Invoke(_harborPanel);
    }

    public ShipSlotView InitializeShipUI(ShipStruct ship)
    {
        var newShipSlot = Instantiate(_shipSlotPrefab, _shipSlotsContainer.transform);

        var slotView = newShipSlot.GetComponent<ShipSlotView>();
        slotView.SetData(ship);
        return slotView;
    }

    public void RemoveShipUI(ShipSlotView shipSlotView)
    {
        if (shipSlotView != null && shipSlotView.gameObject != null)
        {
            Destroy(shipSlotView.gameObject);
        }
    }
}
