using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "Scriptable Objects/�������� ����������/��� �������")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("������������ � ��������� �����")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("�������")]
    #endregion Header
    public bool isCorridor;
    #region Header
    [Header("������� ����� ��")]
    #endregion Header
    public bool isCorridorNs;
    #region Header
    [Header("������� ����� ������")]
    #endregion Header
    public bool isCorridorEW;
    #region Header
    [Header("��������� �������")]
    #endregion Header
    public bool isEntrance;
    #region Header
    [Header("������� � ������")]
    #endregion Header
    public bool isBossRoom;
    #region Header
    [Header("��� ����")]
    #endregion Header
    public bool isNone;
    #region Header
    [Header("������� � ������� �� ������")]
    #endregion Header
    public bool isEndRoom;
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}