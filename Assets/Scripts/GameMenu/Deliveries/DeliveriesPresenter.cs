using System;
using UnityEngine;

public class DeliveriesPresenter : IDisposable
{
    private DeliveriesView _deliveriesView;
    private DeliveryGenerator _deliveryGenerator;
    private int _maxDeliverySlots = 5;
    private int _occupiedDeliverySlots = 0;


    public event Action ClosePanelEvent;

    public DeliveriesPresenter(DeliveriesView view)
    {
        _deliveriesView = view;
    }

    public void Init(DeliveryGenerator generator)
    {
        SetServices(generator);

        Subscribe();
    }

    private void Subscribe()
    {
        _deliveryGenerator.OnDeliveryOrderGeneration += InitializeDeliveryOrder;
        _deliveriesView.OnClosePanelButtonPressed += ClosePanel;
        _deliveriesView.OnDeliveryAccepted += FreeDeliverySlot;
    }

    private void UnSubscribe()
    {
        _deliveryGenerator.OnDeliveryOrderGeneration -= InitializeDeliveryOrder;
        _deliveriesView.OnClosePanelButtonPressed -= ClosePanel;
        _deliveriesView.OnDeliveryAccepted -= FreeDeliverySlot;
    }

    private void SetServices(DeliveryGenerator generator)
    {
        _deliveryGenerator = generator;
    }

    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ClosePanelEvent?.Invoke();
    }

    public bool HasFreeDeliverySlots()
    {
        if (_occupiedDeliverySlots < _maxDeliverySlots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FreeDeliverySlot(DeliveryOrder order, ShipStruct shipStruct)
    {
        _occupiedDeliverySlots--;
    }

    private void InitializeDeliveryOrder(DeliveryOrder order)
    {
        _deliveriesView.InitializeDeliveryOrderUI(order);
        OccupyDeliverySlot();
    }

    private void OccupyDeliverySlot()
    {
        _occupiedDeliverySlots++;
    }

    public void Disable() => UnSubscribe();
    public void Dispose()
    {
        UnSubscribe();

        _deliveriesView = null;
    }
}
