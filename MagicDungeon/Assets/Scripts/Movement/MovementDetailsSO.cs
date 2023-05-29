using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/ѕередвижение/ƒетали передвижени€")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header Movement Details
    [Space(10)]
    [Header("детали передвижени€")]
    #endregion
    #region Tooltip
    [Tooltip("ћинимальна€ скорость передвижени€.")]
    #endregion
    public float minMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("ћаксимальна€ скорость передвижени€.")]
    #endregion
    public float maxMoveSpeed = 8f;

    /// <summary>
    /// ¬озвращает случайную скорость между максимальным и минимальным значением
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
    }

#endif
    #endregion
}
