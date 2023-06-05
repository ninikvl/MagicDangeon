using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent, ReloadWeaponEventArgs> OnReloadWeapon;

    /// <summary>
    /// Укажите оружие, для которого требуется перезарядить обойму. 
    /// Если общий боезапас также должен быть увеличен, то укажите процент пополнения боезапаса.
    /// </summary>
    public void CallReloadWeaponEvent(Weapon weapon, int topUpAmmoPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArgs() { weapon = weapon, topUpAmmoPercent = topUpAmmoPercent });
    }
}


public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int topUpAmmoPercent;
}