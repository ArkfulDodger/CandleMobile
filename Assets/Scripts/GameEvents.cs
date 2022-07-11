using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    // Set Instance as Singleton
    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Events
    public event Action<DoorController> DoorwayTriggerEnter;
    public void DoorwayTriggerEnterHandler(DoorController door)
    {
        DoorwayTriggerEnter?.Invoke(door);
    }

    public event Action<DoorController> DoorwayTriggerExit;
    public void DoorwayTriggerExitHandler(DoorController door)
    {
        DoorwayTriggerExit?.Invoke(door);
    }

}
