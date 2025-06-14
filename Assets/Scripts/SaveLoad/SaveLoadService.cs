using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SaveLoadService
{
    private const string SAVE_FILE_NAME = "game_save.json";
    private readonly string _saveFilePath;

    public SaveLoadService()
    {
        _saveFilePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
    }

    public bool HasSaveFile()
    {
        return File.Exists(_saveFilePath);
    }

    public void SaveGame(ResourcesModel resourcesModel, HarborModel harborModel, TimeController timeController)
    {
        var saveData = new GameSaveData
        {
            resources = SerializeResources(resourcesModel),
            harbor = SerializeHarbor(harborModel),
            time = SerializeTime(timeController)
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_saveFilePath, json);
    }

    public void LoadGame(ResourcesModel resourcesModel, HarborModel harborModel, TimeController timeController)
    {
        if (!HasSaveFile()) return;

        string json = File.ReadAllText(_saveFilePath);
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        DeserializeResources(saveData.resources, resourcesModel);
        DeserializeHarbor(saveData.harbor, harborModel);
        DeserializeTime(saveData.time, timeController);
    }

    public void DeleteSaveFile()
    {
        if (HasSaveFile())
        {
            File.Delete(_saveFilePath);
        }
    }

    private ResourcesSaveData SerializeResources(ResourcesModel model)
    {
        var types = new List<int>();
        var amounts = new List<int>();

        foreach (var kvp in model.PlayerResources)
        {
            types.Add((int)kvp.Key);
            amounts.Add(kvp.Value);
        }

        return new ResourcesSaveData
        {
            resourceTypes = types.ToArray(),
            resourceAmounts = amounts.ToArray()
        };
    }

    private HarborSaveData SerializeHarbor(HarborModel model)
    {
        var shipSaveData = new List<ShipSaveData>();
        var dockNames = new List<string>();

        foreach (var ship in model.ActiveShips)
        {
            var shipData = new ShipSaveData
            {
                shipName = ship.ShipName.ToString(),
                shipSOName = ship.ShipData.name, 
                dockName = GetDockNameForShip(ship, model),
                IsFree = ship.IsFree,
                IsMoving = ship.IsMoving,
                position = ship.ShipObject.transform.position,
                rotation = ship.ShipObject.transform.rotation,
                hasActiveDelivery = false 
            };
            
            shipSaveData.Add(shipData);
        }

        foreach (var dock in model.WorkingDocksList)
        {
            if (dock != null)
                dockNames.Add(dock.name);
        }

        return new HarborSaveData
        {
            maxDockSlots = model.MaxDockSlots,
            occupiedDockSlots = model.OccupiedDockSlots,
            activeShips = shipSaveData.ToArray(),
            workingDockNames = dockNames.ToArray()
        };
    }

    private string GetDockNameForShip(ShipStruct ship, HarborModel model)
    {
        foreach (var kvp in model.ShipsDockingPositions)
        {
            if (kvp.Key == ship.ShipObject)
            {
                return kvp.Value.name;
            }
        }
        return string.Empty;
    }

    private TimeSaveData SerializeTime(TimeController controller)
    {
        return new TimeSaveData
        {
            isTimePaused = controller.IsTimePaused,
            currentTime = Time.time
        };
    }

    private void DeserializeResources(ResourcesSaveData data, ResourcesModel model)
    {
        if (data?.resourceTypes == null) return;

        model.PlayerResources.Clear();
        for (int i = 0; i < data.resourceTypes.Length; i++)
        {
            var resourceType = (ResourceType)data.resourceTypes[i];
            model.PlayerResources[resourceType] = data.resourceAmounts[i];
        }
    }

    private void DeserializeHarbor(HarborSaveData data, HarborModel model)
    {
        if (data.activeShips == null) return;

        model.MaxDockSlots = data.maxDockSlots;
        model.OccupiedDockSlots = data.occupiedDockSlots;
        
        model.ActiveShips.Clear();
        model.ShipsDockingPositions.Clear();
        model.WorkingDocksList.Clear();

        model.SavedShipsData = data.activeShips.ToList();
        model.SavedDockNames = data.workingDockNames.ToList();
    }

    private void DeserializeTime(TimeSaveData data, TimeController controller)
    {
        controller.IsTimePaused = data.isTimePaused;
    }
}
