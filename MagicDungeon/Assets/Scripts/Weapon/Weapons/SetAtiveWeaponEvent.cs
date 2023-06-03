using System;
using UnityEngine;

[DisallowMultipleComponent]
public class SetAtiveWeaponEvent : MonoBehaviour
{
    public event Action<SetAtiveWeaponEvent, SetAtiveWeaponArgs> OnSetActiveWeapon;

    public void CallSetActiveWeaponEvent(Weapon weapon)
    {
        OnSetActiveWeapon?.Invoke(this, new SetAtiveWeaponArgs()
        {
            weapon = weapon
        });
    }
}

public class SetAtiveWeaponArgs : EventArgs
{
    public Weapon weapon;
}