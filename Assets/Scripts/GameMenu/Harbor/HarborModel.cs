using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HarborModel
{
    public int MaxDockSlots = 3;
    public int OccupiedDockSlots = 0;

    public List<ShipStruct> ActiveShips = new();
    public List<GameObject> WorkingDocksList = new();
    public Dictionary<GameObject, GameObject> ShipsDockingPositions = new();
    public Dictionary<GameObject, CancellationTokenSource> ShipCancellations = new();
    public List<ShipSaveData> SavedShipsData = new();
    public List<string> SavedDockNames = new();
}
