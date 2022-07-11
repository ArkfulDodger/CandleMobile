using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Vector3 _closedPosition;
    private float _openHeight = 2f;

    private void Awake()
    {
        _closedPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        GameEvents.Instance.DoorwayTriggerEnter += OnDoorwayTriggerEnter;
        GameEvents.Instance.DoorwayTriggerExit += OnDoorwayTriggerExit;
    }
    
    private void OnDisable()
    {
        GameEvents.Instance.DoorwayTriggerEnter -= OnDoorwayTriggerEnter;
        GameEvents.Instance.DoorwayTriggerExit -= OnDoorwayTriggerExit;
    }

    private void OnDoorwayTriggerEnter(DoorController door)
    {
        if (door != this) return;

        LeanTween.moveLocalY(gameObject, _openHeight, 1f).setEaseOutQuad();
    }

    private void OnDoorwayTriggerExit(DoorController door)
    {
        if (door != this) return;

        LeanTween.moveLocalY(gameObject, _closedPosition.y, 1f).setEaseOutQuad();
    }
}
