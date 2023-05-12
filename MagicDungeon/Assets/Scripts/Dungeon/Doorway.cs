using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����������� ����������� �������� ����������� Unity
[System.Serializable] 
public class Doorway
{
    #region Header
    [Header("�������� ��������� �������� �����")]
    #endregion
    public Vector2Int position;
    public Orientation orientation;
    public GameObject doorPrefab;

    #region Header
    [Header("������� ����� ������� ������ ���������� ����� ��� �������� �������")]
    #endregion
    public Vector2Int doorwayStartCopyPos;
    #region Header
    [Header("������ ������ �������� ����� ��� �����������")]
    #endregion
    public int doorwayCopyTileWidth;
    #region Header
    [Header("������ ������ �������� ����� ��� �����������")]
    #endregion
    public int doorwayCopyTileHeight;

    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isUnavailable = false;

}
