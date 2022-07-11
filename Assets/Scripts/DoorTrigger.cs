using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private DoorController _door;

    private void Awake()
    {
        _door = transform.parent.GetComponentInChildren<DoorController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_door) return;

        Debug.Log("Trigger Enter Called!");
        GameEvents.Instance.DoorwayTriggerEnterHandler(_door);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_door) return;

        Debug.Log("Trigger Exit Called!");
        GameEvents.Instance.DoorwayTriggerExitHandler(_door);
    }
}
