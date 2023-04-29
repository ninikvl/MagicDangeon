using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "Scriptable Objects/�������� ����������/������ ����� ������")]
public class RoomNodeTypeListSO : ScriptableObject
{
    #region Header
    [Space(10)]
    [Header("������ ����� ������")]
    #endregion
    #region Tooltip
    [Tooltip("���� ������ ������ ���� �������� ����� ������ ������ ��� ����")]
    #endregion
    public List<RoomNodeTypeSO> list;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumeravbleValues(this, nameof(list), list);
    }
#endif
    #endregion
}
