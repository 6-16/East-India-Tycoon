using UnityEngine;
using System.Threading.Tasks;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

public class HarborPresenter
{
    private ResourcesModel _resourcesModel;
    private HarborView _harborView;
    private HarborModel _harborModel;
    private DeliveriesView _deliveriesView;
    private ShopPresenter _shopPresenter;

    private Dictionary<ShipStruct, ShipSlotView> _shipSlotViews = new();


    public event Action ClosePanelEvent;

    public HarborPresenter(ResourcesModel resourcesModel, HarborView view, HarborModel model, DeliveriesView deliveriesView, ShopPresenter shopPresenter)
    {
        _resourcesModel = resourcesModel;
        _harborView = view;
        _harborModel = model;
        _deliveriesView = deliveriesView;
        _shopPresenter = shopPresenter;
    }

    // Intro Utils
    public void Init()
    {
        Subscribe();

        InitializeWorkingDocks();

        AddStartingShip();
    }

    private void Subscribe()
    {
        _harborView.OnClosePanelButtonPressed += ClosePanel;

        _deliveriesView.OnDeliveryAccepted += PrepareToSail;
        _shopPresenter.OnBuildingBought += OnBuildingBought;
        _shopPresenter.OnShipBought += OnShipBought;
    }

    private void UnSubscribe()
    {
        _harborView.OnClosePanelButtonPressed -= ClosePanel;

        _deliveriesView.OnDeliveryAccepted -= PrepareToSail;
        _shopPresenter.OnBuildingBought -= OnBuildingBought;
        _shopPresenter.OnShipBought -= OnShipBought;
    }

    public void RefreshView()
    {
        InitializeWorkingDocks();

        foreach (var ship in _harborModel.ActiveShips)
        {
            _harborView.InitializeShipUI(ship);
        }
    }

    // UI logic  
    private void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ClosePanelEvent?.Invoke();
    }

    // Docks & Ships logic
    private void OnBuildingBought(ShopCommoditySO commodity)
    {
        UnlockHarborDock();
    }

    private void OnShipBought(ShopCommoditySO commodity)
    {
        AddShip(commodity.CommodityObject as ShipSO);
    }

    private void AddShip(ShipSO ship)
    {
        if (ship == null) return;

        var freeDock = GetFreeDock();
        if (freeDock == null) return;

        var spawnPoint = freeDock.transform.GetChild(0);
        var shipModel = UnityEngine.Object.Instantiate(ship.ShipPrefab, spawnPoint.position, spawnPoint.rotation);
        shipModel.transform.SetParent(spawnPoint);
        var shipEffects = shipModel.GetComponentsInChildren<ParticleSystem>().ToList();

        var shipStruct = new ShipStruct(ship, shipModel, spawnPoint)
        {
            ShipEffects = shipEffects
        };
        var possibleShipNames = Enum.GetValues(typeof(ShipNames));
        var randomShipName = (ShipNames)possibleShipNames.GetValue(UnityEngine.Random.Range(0, possibleShipNames.Length));
        shipStruct.ShipName = randomShipName;

        Debug.Log($"Spawning ship {shipStruct.ShipName} at dock: {freeDock.name}");

        _harborModel.ActiveShips.Add(shipStruct);
        _harborModel.ShipsDockingPositions[shipModel] = freeDock;
        _harborModel.OccupiedDockSlots++;

        var shipSlotView = _harborView.InitializeShipUI(shipStruct);
        _shipSlotViews[shipStruct] = shipSlotView;
    }

    private GameObject GetFreeDock()
    {
        foreach (var dock in _harborModel.WorkingDocksList)
        {
            if (!_harborModel.ShipsDockingPositions.ContainsValue(dock))
                return dock;
        }

        Debug.Log("No free dock found");
        return null;
    }

    private void InitializeWorkingDocks()
    {
        _harborModel.WorkingDocksList.Clear();

        int docksToActivate = _harborModel.MaxDockSlots;
        int activatedCount = 0;

        foreach (GameObject dock in _harborView.DockObjects)
        {
            if (activatedCount < docksToActivate)
            {
                if (!dock.activeSelf)
                    dock.SetActive(true);
                _harborModel.WorkingDocksList.Add(dock);
                activatedCount++;
            }
            else
            {
                if (dock.activeSelf)
                    dock.SetActive(false);
            }
        }
    }

    public void AddStartingShip()
    {
        AddShip(_harborView.ShipsSOList[0]);
    }

    private void UnlockHarborDock()
    {
        if (_harborModel.MaxDockSlots >= 7)
            return;

        _harborModel.MaxDockSlots++;

        foreach (GameObject dock in _harborView.DockObjects)
        {
            if (!dock.activeSelf)
            {
                dock.SetActive(true);
                _harborModel.WorkingDocksList.Add(dock);
                break;
            }
        }
    }

    private async void PrepareToSail(DeliveryOrder order, ShipStruct shipStruct)
    {
        if (!shipStruct.IsFree || shipStruct.IsMoving) return;

        shipStruct.IsFree = false;
        shipStruct.IsMoving = true;

        int preparationTime = Mathf.CeilToInt(order.DeliveryCargoCapacity / 2f);

        if (_shipSlotViews.TryGetValue(shipStruct, out var view))
        {
            view.UpdateShipStatus($"Preparing for a {order.DeliveryDistance} miles delivery");
        }
        Debug.Log($"Preparing {shipStruct.ShipName} for {preparationTime} seconds before sailing...");

        var cancellationToken = new CancellationTokenSource();
        _harborModel.ShipCancellations[shipStruct.ShipObject] = cancellationToken;

        var dockParent = shipStruct.DockPoint.parent;
        var progressBarObject = dockParent.GetChild(2).gameObject;
        var progressBarView = progressBarObject.GetComponent<ProgressBarView>();
        progressBarView.Show(0);

        try
        {
            float elapsedTime = 0f;
            while (elapsedTime < preparationTime)
            {
                cancellationToken.Token.ThrowIfCancellationRequested();

                elapsedTime += Time.deltaTime;
                progressBarView.SetProgress(0, elapsedTime / preparationTime);
                await Task.Yield();
            }

            progressBarView.Hide(0);

            await SetSail(order, shipStruct, cancellationToken.Token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"{shipStruct.ShipName}'s departure was cancelled.");
            progressBarView.Hide(0);
        }
        finally
        {
            _harborModel.ShipCancellations.Remove(shipStruct.ShipObject);
        }
    }

    private async Task SetSail(DeliveryOrder order, ShipStruct shipStruct, CancellationToken token)
    {
        if (_shipSlotViews.TryGetValue(shipStruct, out var view))
        {
            view.UpdateShipStatus($"Performing a {order.DeliveryDistance} miles delivery");
        }

        var shipObj = shipStruct.ShipObject.transform;
        float speed = shipStruct.ShipData.ShipSpeed;
        float visualSpeed = speed * 0.06f;

        var dockParent = shipStruct.DockPoint.parent;
        var progressBarView = dockParent.GetComponentInChildren<ProgressBarView>(true);
        progressBarView.Show(1);

        float deliveryTime = order.DeliveryDistance - (0.1f * shipStruct.ShipData.ShipSpeed);
        // float returnThreshold = 10f;
        // float waitTime = Mathf.Max(deliveryTime - returnThreshold, 0);

        Vector3 undockTarget = shipObj.position + shipObj.forward * 3f;

        shipStruct.ShipEffects?.ForEach(effect => effect.Play());

        Task progressTask = RunDeliveryCountdown(deliveryTime, progressBarView, token);

        await MoveToTarget(shipObj, undockTarget, visualSpeed, token);
        await MoveToTarget(shipObj, shipStruct.DockPoint.parent.GetChild(1).position, visualSpeed, token);
        await MoveToTarget(shipObj, _harborView.DeliveryPoint.position, visualSpeed, token);

        await progressTask;

        Debug.Log("Starting return journey...");
        await MoveToTarget(shipObj, shipStruct.DockPoint.parent.GetChild(1).position, visualSpeed, token);
        var dockPivot = GetDockPivot(shipStruct);
        await MoveToTarget(shipObj, dockPivot.position, visualSpeed, token);

        progressBarView.Hide(1);

        shipObj.SetParent(dockPivot);
        shipObj.localPosition = Vector3.zero;
        shipObj.localRotation = Quaternion.identity;

        Debug.Log($"{shipStruct.ShipName} has returned to the harbor and is docked.");
        view.UpdateShipStatus("Waiting");

        int index = _harborModel.ActiveShips.FindIndex(s => s.ShipObject == shipStruct.ShipObject);
        if (index >= 0)
        {
            var updated = _harborModel.ActiveShips[index];
            updated.IsFree = true;
            updated.IsMoving = false;
            updated.ShipEffects?.ForEach(effect => effect.Stop());
            _harborModel.ActiveShips[index] = updated;
        }

        ReceiveDeliveryPayment(order);
    }



    private async Task MoveToTarget(Transform obj, Vector3 target, float speed, CancellationToken token)
    {
        float remainingDistance = Vector3.Distance(obj.position, target);

        while (remainingDistance > 0.1f)
        {
            token.ThrowIfCancellationRequested();

            Vector3 direction = (target - obj.position).normalized;
            obj.forward = direction;
            obj.position += direction * speed * Time.deltaTime;
            await Task.Yield();
            remainingDistance = Vector3.Distance(obj.position, target);
        }

        obj.position = target;
    }

    private Transform GetDockPivot(ShipStruct shipStruct)
    {
        return shipStruct.DockPoint;
    }

    private void ReceiveDeliveryPayment(DeliveryOrder order)
    {
        _resourcesModel.ChangeResourceAmount(ResourceType.Gold, order.DeliveryPayment, true);
    }

    private async Task RunDeliveryCountdown(float duration, ProgressBarView progressBarView, CancellationToken token)
    {
        Debug.Log($"Delivering for {duration:F1} seconds...");

        float elapsed = 0f;
        while (elapsed < duration)
        {
            token.ThrowIfCancellationRequested();

            elapsed += Time.deltaTime;
            progressBarView.SetProgress(1, elapsed / duration);
            await Task.Yield();
        }
    }


    // Outro Utils
    public void Disable()
    {
        UnSubscribe();

        var tokens = _harborModel.ShipCancellations.Values.ToList();
        foreach (var token in tokens)
        {
            token.Cancel();
            token.Dispose();
        }

        _harborModel.ShipCancellations.Clear();
    }
    public void Dispose()
    {
        Disable();

        _harborModel = null;
        _harborView = null;
        _deliveriesView = null;
    }

    public void LoadSavedShips()
    {
        if (_harborModel.SavedShipsData == null || _harborModel.SavedShipsData.Count == 0)
            return;

        RestoreWorkingDocks();

        foreach (var shipSaveData in _harborModel.SavedShipsData)
        {
            LoadShipFromSaveData(shipSaveData);
        }

        _harborModel.SavedShipsData.Clear();
        _harborModel.SavedDockNames.Clear();
    }

    private void RestoreWorkingDocks()
    {
        _harborModel.WorkingDocksList.Clear();

        foreach (string dockName in _harborModel.SavedDockNames)
        {
            foreach (GameObject dock in _harborView.DockObjects)
            {
                if (dock.name == dockName)
                {
                    if (!dock.activeSelf)
                        dock.SetActive(true);
                    _harborModel.WorkingDocksList.Add(dock);
                    break;
                }
            }
        }
    }

    private void LoadShipFromSaveData(ShipSaveData shipSaveData)
    {
        ShipSO shipSO = _harborView.ShipsSOList.FirstOrDefault(s => s.name == shipSaveData.shipSOName);
        if (shipSO == null)
        {
            Debug.LogError($"Could not find ShipSO with name: {shipSaveData.shipSOName}");
            return;
        }

        GameObject targetDock = _harborModel.WorkingDocksList.FirstOrDefault(d => d.name == shipSaveData.dockName);
        if (targetDock == null)
        {
            Debug.LogError($"Could not find dock with name: {shipSaveData.dockName}");
            return;
        }

        var spawnPoint = targetDock.transform.GetChild(0);
        var shipModel = UnityEngine.Object.Instantiate(shipSO.ShipPrefab, shipSaveData.position.ToVector3(), shipSaveData.rotation.ToQuaternion());
        shipModel.transform.SetParent(spawnPoint);
        var shipEffects = shipModel.GetComponentsInChildren<ParticleSystem>().ToList();

        var shipStruct = new ShipStruct(shipSO, shipModel, spawnPoint)
        {
            ShipEffects = shipEffects,
            IsFree = shipSaveData.IsFree,
            IsMoving = shipSaveData.IsMoving
        };

        if (Enum.TryParse<ShipNames>(shipSaveData.shipName, out ShipNames parsedName))
        {
            shipStruct.ShipName = parsedName;
        }
        else
        {
            var possibleShipNames = Enum.GetValues(typeof(ShipNames));
            var randomShipName = (ShipNames)possibleShipNames.GetValue(UnityEngine.Random.Range(0, possibleShipNames.Length));
            shipStruct.ShipName = randomShipName;
        }

        _harborModel.ActiveShips.Add(shipStruct);
        _harborModel.ShipsDockingPositions[shipModel] = targetDock;

        var shipSlotView = _harborView.InitializeShipUI(shipStruct);
        _shipSlotViews[shipStruct] = shipSlotView;

        Debug.Log($"Loaded ship {shipStruct.ShipName} at dock: {targetDock.name}");
    }
    
    public void ClearAllShips()
    {
        var tokens = _harborModel.ShipCancellations.Values.ToList();
        foreach (var token in tokens)
        {
            token.Cancel();
            token.Dispose();
        }
        _harborModel.ShipCancellations.Clear();

        foreach (var ship in _harborModel.ActiveShips)
        {
            if (ship.ShipObject != null)
            {
                UnityEngine.Object.Destroy(ship.ShipObject);
            }
        }

        foreach (var shipSlotView in _shipSlotViews.Values)
        {
            if (shipSlotView != null)
            {
                _harborView.RemoveShipUI(shipSlotView);
            }
        }

        _harborModel.ActiveShips.Clear();
        _harborModel.ShipsDockingPositions.Clear();
        _shipSlotViews.Clear();
        _harborModel.OccupiedDockSlots = 0;
    }
}
