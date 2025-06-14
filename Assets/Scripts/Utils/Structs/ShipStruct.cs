using System.Collections.Generic;
using UnityEngine;

public class ShipStruct
{
    public ShipNames ShipName;
    public ShipSO ShipData;
    public GameObject ShipObject;
    public Transform DockPoint;
    public bool IsFree;
    public bool IsMoving;
    public Sprite ShipIcon;
    public DeliveryOrder CurrentDeliveryOrder;
    public List<ParticleSystem> ShipEffects;

    public ShipStruct(ShipSO data, GameObject obj, Transform dock)
    {
        ShipData = data;
        ShipObject = obj;
        DockPoint = dock;
        IsFree = true;
        ShipIcon = data.ShipIcon;
    }
}
