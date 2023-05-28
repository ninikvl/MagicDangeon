using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(StayEvent))]
[DisallowMultipleComponent]
public class Stay : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private StayEvent stayEvent;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        stayEvent = GetComponent<StayEvent>();
    }

    private void OnEnable()
    {
        stayEvent.OnStay += StayEvent_OnStay;
    }

    private void OnDisable()
    {
        stayEvent.OnStay -= StayEvent_OnStay;
    }

    private void StayEvent_OnStay(StayEvent stayEvent)
    {
        MoveRigidBody();
    }

    private void MoveRigidBody()
    {
        rigidBody2D.velocity = Vector2.zero;
    }
}
