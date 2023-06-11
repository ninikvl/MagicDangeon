using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject exitPrefab;
    private Room exitRoom;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        if (exitRoom == null)
            exitRoom = GetComponentInParent<InstantiatedRoom>().room;

        if (exitRoom != null && exitRoom == roomChangedEventArgs.room)
            SpawnExit();
    }

    private void SpawnExit()
    {
        GameObject exitGameObject = Instantiate(exitPrefab, this.transform);
    }
}
