using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticEventHandler
{
    //Ёвенет изменени€ комнаты
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs()
        {
            room = room
        });
    }
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;
}
