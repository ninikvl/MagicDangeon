using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Оружие/Детали боеприпасов")]
public class AmmoDetailsSO : ScriptableObject
{
    #region Header Basic Ammo Details
    [Space(10)]
    [Header("Базовые детали боеприпасов")]
    #endregion Header
    #region Tooltip
    [Tooltip("Название боеприпаса")]
    #endregion
    public string ammoName;
    public bool isPlayerAmmo;

    #region Header Ammo Sprite, Prefab and Materials
    [Space(10)]
    [Header("Спрайт, Префаб, Материалы")]
    #endregion Header
    #region Tooltip
    [Tooltip("Спрайт боеприпаса")]
    #endregion
    public Sprite ammoSprite;
    #region Tooltip
    [Tooltip("Список префабов боеприпаса, из списка будет выбираться случайный префаб при выстреле")]
    #endregion
    public GameObject[] ammoPrefabArray;
    #region Tooltip
    [Tooltip("Материал боеприпаса")]
    #endregion
    public Material ammoMaterial;
    #region Tooltip
    [Tooltip("Задержка боеприпаса на месте после вытрела")]
    #endregion
    public float ammoChargeTime = 0.1f;
    #region Tooltip
    [Tooltip("Смена материала боеприпаса при наличии задержки после выстрела")]
    #endregion
    public Material ammoChargeMaterial;

    #region Header Ammo Base Parameters
    [Space(10)]
    [Header("Базовые настройки боеприпасов")]
    #endregion Header
    #region Tooltip
    [Tooltip("Урон боеприпаса")]
    #endregion
    public int ammoDamage = 1;
    #region Tooltip
    [Tooltip("Минимальная скорость боеприпаса")]
    #endregion
    public float ammoSpeedMin = 20f;
    #region Tooltip
    [Tooltip("Максимальная скорость боеприпаса")]
    #endregion
    public float ammoSpeedMax = 20f;
    #region Tooltip
    [Tooltip("Дальность полёта боеприпаса")]
    #endregion
    public float ammoRange = 20f;
    #region Tooltip
    [Tooltip("Крутится ли боеприпас?")]
    #endregion
    public bool isAmmoSriteRotation = false;
    #region Tooltip
    [Tooltip("Скорость поворота  боеприпаса")]
    #endregion
    public float ammoSpriteRotationSpeed = 0f;
    #region Tooltip
    [Tooltip("Скорость поворота паттерна боеприпаса")]
    #endregion
    public float ammoPatternRotationSpeed = 1f;

    #region Header Ammo Spread Parameters
    [Space(10)]
    [Header("Детали разброса боеприпасов")]
    #endregion Header
    #region Tooltip
    [Tooltip("Минимальный разброс боеприпасов")]
    #endregion
    public float ammoSpreadMin = 0f;
    #region Tooltip
    [Tooltip("Максимальный разброс боеприпасов")]
    #endregion
    public float ammoSpreadMax = 0f;

    #region Header Ammo Spawn Parameters
    [Space(10)]
    [Header("Параметры спавна боеприпасов")]
    #endregion Header
    #region Tooltip
    [Tooltip("Минимальное колчество спавнящихся боеприпасов")]
    #endregion
    public int ammoSpawnAmountMin = 1;
    #region Tooltip
    [Tooltip("Максимальное колчество спавнящихся боеприпасов")]
    #endregion
    public int ammoSpawnAmountMax = 1;
    #region Tooltip
    [Tooltip("Минимальное время спавнящихся боеприпасов")]
    #endregion
    public float ammoSpawnIntervalMin = 0f;
    #region Tooltip
    [Tooltip("Максимальное время спавнящихся боеприпасов")]
    #endregion
    public float ammoSpawnIntervalMax = 0f;

    #region Header Ammo Trail Parameters
    [Space(10)]
    [Header("След боеприпасов")]
    #endregion Header
    #region Tooltip
    [Tooltip("Оставляют ли боеприпасы след?")]
    #endregion
    public bool isAmmoTrail = false;
    #region Tooltip
    [Tooltip("Длительность следа")]
    #endregion
    public float ammoTrailTime = 3f;
    #region Tooltip
    [Tooltip("Материал следа")]
    #endregion
    public Material ammoTrailMaterial;
    #region Tooltip
    [Tooltip("Стартовая ширина следа")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    #region Tooltip
    [Tooltip("Конечная ширина следа")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailEndWidth;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoSprite), ammoSprite);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoPrefabArray), ammoPrefabArray);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoMaterial), ammoMaterial);
        if (ammoChargeTime > 0)
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoChargeMaterial), ammoChargeMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoDamage), ammoDamage, false);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(ammoSpeedMin), ammoSpeedMin, nameof(ammoSpeedMax), ammoSpeedMax, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoRange), ammoRange, false);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(ammoSpreadMin), ammoSpreadMin, nameof(ammoSpreadMax), ammoSpreadMax, false);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(ammoSpawnAmountMin), ammoSpawnAmountMin, nameof(ammoSpawnAmountMax), 
            ammoSpawnAmountMax, false);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(ammoSpawnIntervalMin), ammoSpawnIntervalMin, nameof(ammoSpawnIntervalMax), 
            ammoSpawnIntervalMax, true);
        if (isAmmoTrail)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailTime), ammoTrailTime, false);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailStartWidth), ammoTrailStartWidth, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailEndWidth), ammoTrailEndWidth, false);
        }
    }
#endif
    #endregion
}
