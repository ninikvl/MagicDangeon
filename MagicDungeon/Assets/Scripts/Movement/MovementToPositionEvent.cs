using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionArgs> OnMovementToPosition;

    public void CallMovementToPositionEvent(Vector3 movePosition, Vector3 currentPosition, float moveSpeed, Vector2 moveDirection, bool isBlinking)
    {
        OnMovementToPosition?.Invoke(this, new MovementToPositionArgs()
        {
            movePosition = movePosition,
            currentPosition = currentPosition,
            moveSpeed = moveSpeed,
            moveDirection = moveDirection,
            isBlinking = isBlinking
        });
    }
}

public class MovementToPositionArgs : EventArgs
{
    public Vector3 movePosition;
    public Vector3 currentPosition;
    public float moveSpeed;
    public Vector2 moveDirection;
    public bool isBlinking;
}
