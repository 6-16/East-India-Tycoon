using UnityEngine;


[CreateAssetMenu(fileName = "NewShip", menuName = "Ship")]
public class ShipSO : ScriptableObject
{
    public int ShipSpeed;
    public int ShipCapacity;
    // public int ShipCondition;
    // public int ShipLevel;
    public GameObject ShipPrefab;
    public Sprite ShipIcon;
    public Transform DockParent;




    // public void UpgradeShip()
    // {
    //     if (ShipLevel < 3)
    //     {
    //         ShipLevel++;
    //         ShipSpeed += (int)(ShipSpeed * 0.3f);
    //         ShipCapacity += 50;
    //     }
    // }
}
