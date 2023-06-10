using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/�����/������ ������")]
public class EnemyDetailsSO : ScriptableObject
{
    #region Header BASE ENEMY DETAILS
    [Space(10)]
    [Header("������� �������� � ����������")]
    #endregion
    #region Tooltip
    [Tooltip("��� �����")]
    #endregion
    public string enemyName;
    #region Tooltip
    [Tooltip("������ ��� �����")]
    #endregion
    public GameObject enemyPrefab;
    #region Tooltip
    [Tooltip("���������� �� ������ �� ����, ��� ���� ������ �������������")]
    #endregion
    public float chaseDistance = 50f;

    #region Header ENEMY MATERIAL
    [Space(10)]
    [Header("�������� �����")]
    #endregion
    #region Tooltip
    [Tooltip("��� ����������� �������� ��� ������� lit ��� ����� (������������ ����� ����, ��� ���� ���������������")]
    #endregion
    public Material enemyStandardMaterial;

    #region Header ENEMY MATERIALIZE SETTINGS
    [Space(10)]
    [Header("��������� �������������� �����")]
    #endregion
    #region Tooltip
    [Tooltip("����� � ��������, ����������� ����� ��� ��������������")]
    #endregion
    public float enemyMaterializeTime;
    #region Tooltip
    [Tooltip("������, ������� ����� �������������� ��� �������������� �����")]
    #endregion
    public Shader enemyMaterializeShader;
    [ColorUsage(true, true)]
    #region Tooltip
    [Tooltip("����, ������� ������� ������������ ��� �������������� �����.  ��� ���� HDR, ������� ������������� ����� ��������� ���, ����� ������� �������� / ��������")]
    #endregion
    public Color enemyMaterializeColor;

    #region Header ENEMY WEAPON SETTINGS
    [Space(10)]
    [Header("��������� ���������� ������")]
    #endregion
    #region Tooltip
    [Tooltip("������ ��� ����� - ���, ���� � ����� ��� ������")]
    #endregion
    public WeaponDetailsSO enemyWeapon;
    #region Tooltip
    [Tooltip("����������� �������� �������� � �������� ����� ��������� ��������� ��������.  ��� �������� ������ ���� ������ 0. ����� ������� ��������� �������� ����� ����������� ��������� � ������������ ���������")]
    #endregion
    public float firingIntervalMin = 0.1f;
    #region Tooltip
    [Tooltip("������������ �������� �������� � �������� ����� ��������� ��������� ��������.  ����� ������� ��������� �������� ����� ����������� ��������� � ������������ ���������")]
    #endregion
    public float firingIntervalMax = 1f;
    #region Tooltip
    [Tooltip("����������� ����������������� ��������, � ������� ������� ��������� �������� �� ����� ��������.  ��� �������� ������ ���� ������ ����.  ����� ������� ��������� �������� ����� ����������� ��������� � ������������ ���������.")]
    #endregion
    public float firingDurationMin = 1f;
    #region Tooltip
    [Tooltip("������������ ����������������� ��������, � ������� ������� ��������� �������� �� ����� ��������.  ����� ������� ��������� �������� ����� ����������� ��������� � ������������ ���������.")]
    #endregion
    public float firingDurationMax = 2f;
    #region Tooltip
    [Tooltip("�������� ���� ��������, ���� �� ������ ��������� ������ ��������� �� ����, ��� ���� ���������.  ���� ����� ������������ �� �������, ���� ����� �������� ���������� �� ����������� ������ ���, ����� ����� �������� � �������� ������������")]
    #endregion
    public bool firingLineOfSightRequired;

    #region Header ENEMY HEALTH
    [Space(10)]
    [Header("�������� ����������")]
    #endregion
    #region Tooltip
    [Tooltip("�������� ����� ��� ������� ������")]
    #endregion
    public EnemyHealthDetails[] enemyHealthDetailsArray;
    #region Tooltip
    [Tooltip("��������, ����� �� �� ������ ���������� ����� ����� ���������.  ���� ��, ������� ����� ����������������� � �������� � ������ ����")]
    #endregion
    public bool isImmuneAfterHit = false;
    #region Tooltip
    [Tooltip("����� ����������������� � �������� ����� ���������")]
    #endregion
    public float hitImmunityTime;
    #region Tooltip
    [Tooltip("��������, ����� ���������� ��������� �������� ����������")]
    #endregion
    public bool isHealthBarDisplayed = false;



    #region Validation
#if UNITY_EDITOR
    // Validate the scriptable object details entered
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(enemyName), enemyName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyPrefab), enemyPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(chaseDistance), chaseDistance, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyStandardMaterial), enemyStandardMaterial);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(enemyMaterializeTime), enemyMaterializeTime, true);
        HelperUtilities.ValidateCheckNullValue(this, nameof(enemyMaterializeShader), enemyMaterializeShader);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin, nameof(firingIntervalMax), firingIntervalMax, false);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax), firingDurationMax, false);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyHealthDetailsArray), enemyHealthDetailsArray);
        if (isImmuneAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }
    }

#endif
    #endregion

}