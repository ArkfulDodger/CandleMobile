using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    // Set Instance as Singleton
    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    // Events
    public event Action<DoorController> DoorwayTriggerEnter;
    public event Action<DoorController> DoorwayTriggerExit;

    // Event Handlers
    public void DoorwayTriggerEnterHandler(DoorController door) { DoorwayTriggerEnter?.Invoke(door); }
    public void DoorwayTriggerExitHandler(DoorController door) { DoorwayTriggerExit?.Invoke(door); }
}
