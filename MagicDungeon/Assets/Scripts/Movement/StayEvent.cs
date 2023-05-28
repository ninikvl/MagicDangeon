using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StayEvent : MonoBehaviour
{
    public event Action<StayEvent> OnStay;

    public void CallStayEvent()
    {
        OnStay?.Invoke(this);
    }
}
