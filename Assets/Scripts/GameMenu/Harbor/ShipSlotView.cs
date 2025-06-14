using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipSlotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _shipNameText;
    [SerializeField] private TextMeshProUGUI _shipRosterText;
    [SerializeField] private TextMeshProUGUI _shipAssignedDockText;
    [SerializeField] private Image _shipIcon;



    public void SetData(ShipStruct ship)
    {
        _shipNameText.text = System.Text.RegularExpressions.Regex.Replace(ship.ShipName.ToString(), "(?<!^)([A-Z])", " $1"); ;
        _shipAssignedDockText.text = $"Assigned to {ship.DockPoint.gameObject.name}";
        _shipRosterText.text = $"Currently: Waiting";
        _shipIcon.sprite = ship.ShipIcon;
    }

    public void UpdateShipStatus(string status)
    {
        _shipRosterText.text = $"Currently: {status}";
    }
}
