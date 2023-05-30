using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/������������/������ ������������")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header Movement Details
    [Space(10)]
    [Header("������ ������������")]
    #endregion
    #region Tooltip
    [Tooltip("����������� �������� ������������.")]
    #endregion
    public float minMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("������������ �������� ������������.")]
    #endregion
    public float maxMoveSpeed = 8f;
    #region Tooltip
    [Tooltip("������������ �������� ���������.")]
    #endregion
    public float blinkSpeed;
    #region Tooltip
    [Tooltip("������������ ��������� ���������.")]
    #endregion
    public float blinkDistance;
    #region Tooltip
    [Tooltip("����������� ���������")]
    #endregion
    public float blinkColldownTime;

    /// <summary>
    /// ���������� ��������� �������� ����� ������������ � ����������� ���������
    /// </summary>
    public float GetMoveSpeed()
    {
        if (minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }

    #region Vakidation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);
        if (blinkDistance != 0f || blinkSpeed != 0 || blinkColldownTime != 0)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(blinkDistance), blinkDistance, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(blinkSpeed), blinkSpeed, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(blinkColldownTime), blinkColldownTime, false);
        }
    }

#endif
    #endregion
}
