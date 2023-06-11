using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimWeaponEvent))]
[DisallowMultipleComponent]
public class AimWeapon : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Заполняется координатами объекта 'WeaponRotationPoint'")]
    #endregion
    [SerializeField] private Transform weaponRotationPointTransform;

    private AimWeaponEvent aimWeaponEvent;

    private void Awake()
    {
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
    }

    private void OnEnable()
    {
        //Подписка на эвент
        aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    /// <summary>
    /// Обработчик эвента "OnWeaponAim"
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        Aim(aimWeaponEventArgs.aimDirection, aimWeaponEventArgs.aimAngle);
    }

    /// <summary>
    /// Вращение оружия
    /// </summary>
    private void Aim (AimDirection aimDirection, float aimAngle)
    {
        weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);

        switch (aimDirection)
        {
            case AimDirection.UpLeft:
                weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle / 5f - 15f);
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;
            case AimDirection.Left:
                weaponRotationPointTransform.localScale = new Vector3(1f, -1f, 0f);
                break;

            case AimDirection.Up:
                weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle / 5f - 15f);
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;

            case AimDirection.UpRight:
                weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle / 5f - 15f);
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;

            case AimDirection.Right:
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;

            case AimDirection.Down:
                weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle / -5f -15f);
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f);
                break;
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponRotationPointTransform), weaponRotationPointTransform);
    }

#endif
    #endregion Validation
}
