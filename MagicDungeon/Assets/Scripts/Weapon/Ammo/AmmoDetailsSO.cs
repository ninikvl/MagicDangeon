using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/������/������ �����������")]
public class AmmoDetailsSO : ScriptableObject
{
    #region Header Basic Ammo Details
    [Space(10)]
    [Header("������� ������ �����������")]
    #endregion Header
    #region Tooltip
    [Tooltip("�������� ����������")]
    #endregion
    public string ammoName;
    public bool isPlayerAmmo;

    #region Header Ammo Sprite, Prefab and Materials
    [Space(10)]
    [Header("������, ������, ���������")]
    #endregion Header
    #region Tooltip
    [Tooltip("������ ����������")]
    #endregion
    public Sprite ammoSprite;
    #region Tooltip
    [Tooltip("������ �������� ����������, �� ������ ����� ���������� ��������� ������ ��� ��������")]
    #endregion
    public GameObject[] ammoPrefabArray;
    #region Tooltip
    [Tooltip("�������� ����������")]
    #endregion
    public Material ammoMaterial;
    #region Tooltip
    [Tooltip("�������� ���������� �� ����� ����� �������")]
    #endregion
    public float ammoChargeTime = 0.1f;
    #region Tooltip
    [Tooltip("����� ��������� ���������� ��� ������� �������� ����� ��������")]
    #endregion
    public Material ammoChargeMaterial;

    #region Header Ammo Base Parameters
    [Space(10)]
    [Header("������� ��������� �����������")]
    #endregion Header
    #region Tooltip
    [Tooltip("���� ����������")]
    #endregion
    public int ammoDamage = 1;
    #region Tooltip
    [Tooltip("����������� �������� ����������")]
    #endregion
    public float ammoSpeedMin = 20f;
    #region Tooltip
    [Tooltip("������������ �������� ����������")]
    #endregion
    public float ammoSpeedMax = 20f;
    #region Tooltip
    [Tooltip("��������� ����� ����������")]
    #endregion
    public float ammoRange = 20f;
    #region Tooltip
    [Tooltip("�������� �� ���������?")]
    #endregion
    public bool isAmmoSriteRotation = false;
    #region Tooltip
    [Tooltip("�������� ��������  ����������")]
    #endregion
    public float ammoSpriteRotationSpeed = 0f;
    #region Tooltip
    [Tooltip("�������� �������� �������� ����������")]
    #endregion
    public float ammoPatternRotationSpeed = 1f;

    #region Header Ammo Spread Parameters
    [Space(10)]
    [Header("������ �������� �����������")]
    #endregion Header
    #region Tooltip
    [Tooltip("����������� ������� �����������")]
    #endregion
    public float ammoSpreadMin = 0f;
    #region Tooltip
    [Tooltip("������������ ������� �����������")]
    #endregion
    public float ammoSpreadMax = 0f;

    #region Header Ammo Spawn Parameters
    [Space(10)]
    [Header("��������� ������ �����������")]
    #endregion Header
    #region Tooltip
    [Tooltip("����������� ��������� ����������� �����������")]
    #endregion
    public int ammoSpawnAmountMin = 1;
    #region Tooltip
    [Tooltip("������������ ��������� ����������� �����������")]
    #endregion
    public int ammoSpawnAmountMax = 1;
    #region Tooltip
    [Tooltip("����������� ����� ����������� �����������")]
    #endregion
    public float ammoSpawnIntervalMin = 0f;
    #region Tooltip
    [Tooltip("������������ ����� ����������� �����������")]
    #endregion
    public float ammoSpawnIntervalMax = 0f;

    #region Header Ammo Trail Parameters
    [Space(10)]
    [Header("���� �����������")]
    #endregion Header
    #region Tooltip
    [Tooltip("��������� �� ���������� ����?")]
    #endregion
    public bool isAmmoTrail = false;
    #region Tooltip
    [Tooltip("������������ �����")]
    #endregion
    public float ammoTrailTime = 3f;
    #region Tooltip
    [Tooltip("�������� �����")]
    #endregion
    public Material ammoTrailMaterial;
    #region Tooltip
    [Tooltip("��������� ������ �����")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    #region Tooltip
    [Tooltip("�������� ������ �����")]
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
