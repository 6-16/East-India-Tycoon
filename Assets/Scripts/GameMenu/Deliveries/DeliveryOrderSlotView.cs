using UnityEngine;
using TMPro;

public class DeliveryOrderSlotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cargoText;
    [SerializeField] private TextMeshProUGUI _distanceText;
    [SerializeField] private TextMeshProUGUI _paymentText;



    public void SetData(DeliveryOrder order)
    {
        _cargoText.text = $"{order.DeliveryCargoCapacity} cwt";
        _distanceText.text = $"{order.DeliveryDistance} miles";
        _paymentText.text = $"{order.DeliveryPayment} £";
    }
}
