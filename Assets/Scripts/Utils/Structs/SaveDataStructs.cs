using UnityEngine;

[System.Serializable]
public struct GameSaveData
{
    public ResourcesSaveData resources;
    public HarborSaveData harbor;
    public TimeSaveData time;
    public DeliveryOrderSaveData deliveryOrders;
}

[System.Serializable]
public class ResourcesSaveData
{
    public int[] resourceTypes;
    public int[] resourceAmounts;
}

[System.Serializable]
public struct HarborSaveData
{
    public int maxDockSlots;
    public int occupiedDockSlots;
    public ShipSaveData[] activeShips;
    public string[] workingDockNames;
}

[System.Serializable]
public struct ShipSaveData
{
    public string shipName;
    public string shipSOName;
    public string dockName;
    public bool IsFree;
    public bool IsMoving;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public DeliveryOrderSaveData currentDeliveryOrder;
    public bool hasActiveDelivery;
}

[System.Serializable]
public struct DeliveryOrderSaveData
{
    public int deliveryCargoCapacity;
    public int deliveryDistance;
    public int deliveryPayment;
}

[System.Serializable]
public struct TimeSaveData
{
    public bool isTimePaused;
    public float currentTime;
}

[System.Serializable]
public struct SerializableVector3
{
    public float x, y, z;
    
    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    
    public static implicit operator SerializableVector3(Vector3 vector)
    {
        return new SerializableVector3(vector);
    }
    
    public static implicit operator Vector3(SerializableVector3 serializable)
    {
        return serializable.ToVector3();
    }
}

public struct SerializableQuaternion
{
    public float x, y, z, w;
    
    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }
    
    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
    
    public static implicit operator SerializableQuaternion(Quaternion quaternion)
    {
        return new SerializableQuaternion(quaternion);
    }
    
    public static implicit operator Quaternion(SerializableQuaternion serializable)
    {
        return serializable.ToQuaternion();
    }
}
