using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float firePreChargeTimer = 0f;
    private float fireRrateCoolDownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;
    private ReloadWeaponEvent reloadWeaponEvent;

    private void Awake()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
    }

    private void OnEnable()
    {
        fireWeaponEvent.OnfireWeapon += FireWeaponEvent_OnfireWeapon;
    }

    private void OnDisable()
    {
        fireWeaponEvent.OnfireWeapon -= FireWeaponEvent_OnfireWeapon;
    }

    private void Update()
    {
        fireRrateCoolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Обработка евента выстрела оружия
    /// </summary>
    private void FireWeaponEvent_OnfireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponFiredEvent(fireWeaponEventArgs);
    }

    /// <summary>
    /// Выстрел оружия
    /// </summary>
    private void WeaponFiredEvent(FireWeaponEventArgs fireWeaponEventArgs)
    {
        // Handle weapon precharge timer.
        WeaponPreCharge(fireWeaponEventArgs);

        if (fireWeaponEventArgs.fire)
        {
            if (IsWeaponReadyToFire())
            {
                FireAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle, fireWeaponEventArgs.weaponAimDirectionVector);
                ResetCoolDownTimer();
                ResetPrechargeTimer();
            }
        }
    }

    /// <summary>
    /// Handle weapon precharge.
    /// </summary>
    private void WeaponPreCharge(FireWeaponEventArgs fireWeaponEventArgs)
    {
        // Weapon precharge.
        if (fireWeaponEventArgs.firePreviousFrame)
        {
            // Decrease precharge timer if fire button held previous frame.
            firePreChargeTimer -= Time.deltaTime;
        }
        else
        {
            // else reset the precharge timer.
            ResetPrechargeTimer();
        }
    }

    /// <summary>
    /// Возвращает правду если оружие готово к стрельбе
    /// </summary>
    private bool IsWeaponReadyToFire()
    {
        if (activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
            return false;
        if (activeWeapon.GetCurrentWeapon().isWeaponReloading)
            return false;
        if (fireRrateCoolDownTimer > 0f)
            return false;
        // If the weapon isn't precharged or is cooling down then return false.
        if (firePreChargeTimer > 0f || fireRrateCoolDownTimer > 0f)
            return false;
        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity && activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);
            return false;
        }
        return true;
    }

    /// <summary>
    /// Настройка снаряда в пуле объектов
    /// </summary>
    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if (currentAmmo != null)
        {
            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0, currentAmmo.ammoPrefabArray.Length)];
            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);
            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
            ammo.InitializeAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector);

            if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
            {
                activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
                activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
            }

            weaponFiredEvent.CallFireWeaponEvent(activeWeapon.GetCurrentWeapon());
        }
    }
    
    /// <summary>
    /// Сброс таймера задержки между выстрелами
    /// </summary>
    private void ResetCoolDownTimer()
    {
        fireRrateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;
    }

    /// <summary>
    /// Reset precharge timers
    /// </summary>
    private void ResetPrechargeTimer()
    {
        // Reset precharge timer
        firePreChargeTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponPrechargeTime;
    }
}
