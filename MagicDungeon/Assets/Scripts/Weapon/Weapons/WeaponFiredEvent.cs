using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WeaponFiredEvent : MonoBehaviour
{
    public event Action<WeaponFiredEvent, WeaponFiredEventArgs> OnWeaponFired;

    public void CallFireWeaponEvent(Weapon weapon)
    {
        OnWeaponFired?.Invoke(this, new WeaponFiredEventArgs()
        {
            weapon = weapon
        });
    }
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon weapon;
}
