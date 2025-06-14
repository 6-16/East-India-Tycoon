using System;
using UnityEngine;
using UnityEngine.UI;

public class DeliveriesView : MonoBehaviour
{
    private HarborModel _harborModel;
    // private DeliveriesPresenter _deliveriesPresenter;
    [SerializeField] private GameObject _deliveryOrderSlotsContainer;
    [SerializeField] private GameObject _deliveryOrderSlotPrefab;


    [Header("Buttons")]
    [SerializeField] private Button _closeDeliveriesPanelButton;

    [Space(5)]


    [Header("Panel")]
    [SerializeField] private GameObject _deliveriesPanel;



    public event Action<GameObject> OnClosePanelButtonPressed;
    public event Action<DeliveryOrder, ShipStruct> OnDeliveryAccepted;



    private void OnEnable()
    {
        _closeDeliveriesPanelButton.onClick.AddListener(OnClosePanelButton);
    }

    private void OnDisable()
    {
        _closeDeliveriesPanelButton.onClick.RemoveAllListeners();
    }

    public void Init(HarborModel harborModel, DeliveriesPresenter deliveriesPresenter)
    {
        _harborModel = harborModel;
        // _deliveriesPresenter = deliveriesPresenter;
    }

    private void OnClosePanelButton()
    {
        OnClosePanelButtonPressed?.Invoke(_deliveriesPanel);
    }

    public void InitializeDeliveryOrderUI(DeliveryOrder order)
    {
        var newDeliveryOrderSlot = Instantiate(_deliveryOrderSlotPrefab, _deliveryOrderSlotsContainer.transform);

        var slotView = newDeliveryOrderSlot.GetComponent<DeliveryOrderSlotView>();
        slotView.SetData(order);

        var orderSlotButton = newDeliveryOrderSlot.GetComponentInChildren<Button>();

        if (orderSlotButton != null)
        {
            orderSlotButton.onClick.AddListener(() => OnDeliveryOrderClicked(order, orderSlotButton));
        }
    }

    private void OnDeliveryOrderClicked(DeliveryOrder order, Button button)
    {
        if (_harborModel?.ActiveShips == null || _harborModel.ActiveShips.Count == 0)
        {
            Debug.Log("Ship list is empty or null");
            return;
        }
        
        foreach (var shipStruct in _harborModel.ActiveShips)
        {
            if (!shipStruct.IsFree)
                continue;

            if (shipStruct.ShipData.ShipCapacity >= order.DeliveryCargoCapacity)
                {
                    OnDeliveryAccepted?.Invoke(order, shipStruct);
                    Destroy(button.transform.parent?.gameObject);
                    Debug.Log($"{order} taken by {shipStruct.ShipName}");
                    return;
                }
        }
        
        Debug.Log("No available ship with enough capacity.");
    }
}
