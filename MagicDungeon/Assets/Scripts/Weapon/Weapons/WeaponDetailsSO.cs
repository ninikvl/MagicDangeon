using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Оружие/Детали оружия")]
public class WeaponDetailsSO : ScriptableObject
{
    #region Header Weapon Base Details
    [Space(10)]
    [Header("Базовые детали оружия")]
    #endregion Header
    #region Tooltip
    [Tooltip("Название оружия")]
    #endregion
    public string weaponName;
    #region Tooltip
    [Tooltip("Спрайт оружия")]
    #endregion
    public Sprite weaponSprite;

    #region Header Weapon Configuration
    [Space(10)]
    [Header("Настройки оружия")]
    #endregion Header
    #region Tooltip
    [Tooltip("Точка спавна пуль оружия")]
    #endregion
    public Vector3 weaponShootPosition;
    #region Tooltip
    [Tooltip("Вид боеприпасов")]
    #endregion
    public AmmoDetailsSO weaponCurrentAmmo;

    #region Header Weapon Configuration
    [Space(10)]
    [Header("Параметры работы оружия")]
    #endregion Header
    #region Tooltip
    [Tooltip("Имеет ли оружие бесконечный боезапас")]
    #endregion
    public bool hasInfiniteAmmo = false;
    #region Tooltip
    [Tooltip("Имеет ли оружие бесконечный магазин")]
    #endregion
    public bool hasInfiniteClipCapacity = false;
    #region Tooltip
    [Tooltip("Ёмкость магазина")]
    #endregion
    public int weaponClipAmmoCapacity = 6;
    #region Tooltip
    [Tooltip("Общий боезапас")]
    #endregion
    public int weaponAmmoCapacity = 100;
    #region Tooltip
    [Tooltip("Скорострельность (0.1 - это 10 выстрелов в секунду)")]
    #endregion
    public float weaponFireRate = 0.2f;
    #region Tooltip
    [Tooltip("Задержка перед выстрелом")]
    #endregion
    public float weaponPrechargeTime = 0f;
    #region Tooltip
    [Tooltip("Время перезарядки")]
    #endregion
    public float weaponReloadTime = 0f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }
        if (!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }
#endif
    #endregion
}


