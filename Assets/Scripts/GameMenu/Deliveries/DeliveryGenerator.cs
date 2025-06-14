using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class DeliveryGenerator : IDisposable
{
    private TimeController _timeController;
    private DeliveriesPresenter _deliveriesPresenter;


    private int _basicOrderWaitingTime = 60;


    public event Action<DeliveryOrder> OnDeliveryOrderGeneration;

    private CancellationTokenSource _cancellationTokenSource;


    public DeliveryGenerator(TimeController timeController, DeliveriesPresenter deliveriesPresenter)
    {
        _timeController = timeController;
        _deliveriesPresenter = deliveriesPresenter;
    }

    public void Init()
    {
        Subscribe();

        _cancellationTokenSource = new CancellationTokenSource();
        _ = GeneratingDeliveryOrders(_cancellationTokenSource.Token);

        CallInitialOrders();
    }

    private void Subscribe()
    {
        _timeController.OnNewPlayweekEvent += GenerateWeeklyDeliveryOrder;
    }

    private void UnSubscribe()
    {
        _timeController.OnNewPlayweekEvent -= GenerateWeeklyDeliveryOrder;
    }

    private async Task GeneratingDeliveryOrders(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (!_timeController.IsTimePaused && _deliveriesPresenter.HasFreeDeliverySlots())
                {
                    GenerateDeliveryOrder();
                }

                var delay = UnityEngine.Random.Range(30, 120);
                await Task.Delay(delay * 1000, token);
            }
        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {
            Debug.LogError($"Delivery generation error: {ex}");
        }
    }

    private DeliveryOrder GenerateDeliveryOrder()
    {
        var newDeliveryOrder = new DeliveryOrder();
        newDeliveryOrder.DeliveryWaitingTimer = _basicOrderWaitingTime;
        newDeliveryOrder.DeliveryCargoCapacity = UnityEngine.Random.Range(30, 150);
        newDeliveryOrder.DeliveryDistance = UnityEngine.Random.Range(15, 250);
        newDeliveryOrder.DeliveryPayment = (newDeliveryOrder.DeliveryDistance * 2) + (newDeliveryOrder.DeliveryCargoCapacity * 2);

        OnDeliveryOrderGeneration?.Invoke(newDeliveryOrder);
        Debug.Log($"Delivery order generated. Cargo capacity required {newDeliveryOrder.DeliveryCargoCapacity}. Delivery distance - {newDeliveryOrder.DeliveryDistance} miles. You will receive {newDeliveryOrder.DeliveryPayment} £ for completing this delivery.");
        return newDeliveryOrder;
        
    }

    private void GenerateWeeklyDeliveryOrder()
    {
        if (_deliveriesPresenter.HasFreeDeliverySlots())
        {
            GenerateDeliveryOrder();
        }
        else
        {
            Debug.Log("Delivery slots are full");
        }
    }

    private void CallInitialOrders()
    {
        GenerateDeliveryOrder();

        GenerateDeliveryOrder();
    }

    public void Disable() => UnSubscribe();
    public void Dispose()
    {
        UnSubscribe();

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        _timeController = null;
        _deliveriesPresenter = null;
    }
}
