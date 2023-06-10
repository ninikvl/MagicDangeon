using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    #region Tooltip
    [Tooltip("���������� ������� ���������� ��� ���� ������� � ����������� �� ����, ������� ����� ������ ������ ���� ���������")]
    #endregion Tooltip
    public DungeonLevelSO dungeonLevel;
    #region Tooltip
    [Tooltip("����������� ���������� ������, ������� ����� ��������� � ���� ������� ��� ������� ������ ����������.  ����������� ����� ����� ��������� ��������� ����� ����������� � ������������ ����������")]
    #endregion Tooltip
    public int minTotalEnemiesToSpawn;
    #region Tooltip
    [Tooltip("������������ ���������� ������, ������� ����� ��������� � ���� ������� ��� ������� ������ ����������.  ����������� ����� ����� ��������� ��������� ����� ����������� � ������������ ����������.")]
    #endregion Tooltip
    public int maxTotalEnemiesToSpawn;
    #region Tooltip
    [Tooltip("����������� ���������� ������������ ������������ ������ � ���� ������� ��� ������� ������ ����������.  ����������� ����� ����� ��������� ��������� ����� ����������� � ������������ ����������.")]
    #endregion Tooltip
    public int minConcurrentEnemies;
    #region Tooltip
    [Tooltip("������������ ���������� ������������ ������������ ������ � ���� ������� ��� ������� ������ ����������.  ����������� ����� ����� ��������� ��������� ����� ����������� � ������������ ����������.")]
    #endregion Tooltip
    public int maxConcurrentEnemies;
    #region Tooltip
    [Tooltip("����������� �������� ��������� ������ � ���� ������� ��� ������� ������ ���������� � ��������.  ����������� ����� ����� ��������� ��������� ����� ����������� � ������������ ����������.")]
    #endregion Tooltip
    public int minSpawnInterval;
    #region Tooltip
    [Tooltip("������������ �������� ��������� ������ � ���� ������� ��� ������� ������ ���������� � ��������.  ����������� ����� ����� ��������� ��������� ����� ����������� � ������������ ����������.")]
    #endregion Tooltip
    public int maxSpawnInterval;
}