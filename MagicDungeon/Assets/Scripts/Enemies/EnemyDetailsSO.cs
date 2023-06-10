using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_", menuName = "Scriptable Objects/Враги/Детали врагов")]
public class EnemyDetailsSO : ScriptableObject
{
    #region Header BASE ENEMY DETAILS
    [Space(10)]
    [Header("БАЗОВЫЕ СВЕДЕНИЯ О ПРОТИВНИКЕ")]
    #endregion
    #region Tooltip
    [Tooltip("Имя врага")]
    #endregion
    public string enemyName;
    #region Tooltip
    [Tooltip("Префаб для врага")]
    #endregion
    public GameObject enemyPrefab;
    #region Tooltip
    [Tooltip("Расстояние до игрока до того, как враг начнет преследование")]
    #endregion
    public float chaseDistance = 50f;

    #region Header ENEMY MATERIAL
    [Space(10)]
    [Header("МАТЕРИАЛ ВРАГА")]
    #endregion
    #region Tooltip
    [Tooltip("Это стандартный материал для шейдера lit для врага (используется после того, как враг материализуется")]
    #endregion
    public Material enemyStandardMaterial;

    #region Header ENEMY MATERIALIZE SETTINGS
    [Space(10)]
    [Header("НАСТРОЙКИ МАТЕРИАЛИЗАЦИИ ВРАГА")]
    #endregion
    #region Tooltip
    [Tooltip("Время в секундах, необходимое врагу для материализации")]
    #endregion
    public float enemyMaterializeTime;
    #region Tooltip
    [Tooltip("Шейдер, который будет использоваться при материализации врага")]
    #endregion
    public Shader enemyMaterializeShader;
    [ColorUsage(true, true)]
    #region Tooltip
    [Tooltip("Цвет, который следует использовать при материализации врага.  Это цвет HDR, поэтому интенсивность можно настроить так, чтобы вызвать свечение / цветение")]
    #endregion
    public Color enemyMaterializeColor;

    #region Header ENEMY WEAPON SETTINGS
    [Space(10)]
    [Header("НАСТРОЙКИ ВРАЖЕСКОГО ОРУЖИЯ")]
    #endregion
    #region Tooltip
    [Tooltip("Оружие для врага - нет, если у врага нет оружия")]
    #endregion
    public WeaponDetailsSO enemyWeapon;
    #region Tooltip
    [Tooltip("Минимальный интервал задержки в секундах между очередями вражеской стрельбы.  Это значение должно быть больше 0. Будет выбрано случайное значение между минимальным значением и максимальным значением")]
    #endregion
    public float firingIntervalMin = 0.1f;
    #region Tooltip
    [Tooltip("Максимальный интервал задержки в секундах между очередями вражеской стрельбы.  Будет выбрано случайное значение между минимальным значением и максимальным значением")]
    #endregion
    public float firingIntervalMax = 1f;
    #region Tooltip
    [Tooltip("Минимальная продолжительность стрельбы, в течение которой противник стреляет во время очередей.  Это значение должно быть больше нуля.  Будет выбрано случайное значение между минимальным значением и максимальным значением.")]
    #endregion
    public float firingDurationMin = 1f;
    #region Tooltip
    [Tooltip("Максимальная продолжительность стрельбы, в течение которой противник стреляет во время очередей.  Будет выбрано случайное значение между минимальным значением и максимальным значением.")]
    #endregion
    public float firingDurationMax = 2f;
    #region Tooltip
    [Tooltip("Выберите этот параметр, если от игрока требуется прямая видимость до того, как враг выстрелит.  Если линия прицеливания не выбрана, враг будет стрелять независимо от препятствий всякий раз, когда игрок окажется в пределах досягаемости")]
    #endregion
    public bool firingLineOfSightRequired;

    #region Header ENEMY HEALTH
    [Space(10)]
    [Header("ЗДОРОВЬЕ ПРОТИВНИКА")]
    #endregion
    #region Tooltip
    [Tooltip("Здоровье врага для каждого уровня")]
    #endregion
    public EnemyHealthDetails[] enemyHealthDetailsArray;
    #region Tooltip
    [Tooltip("Выберите, имеет ли он период иммунитета сразу после попадания.  Если да, укажите время невосприимчивости в секундах в другом поле")]
    #endregion
    public bool isImmuneAfterHit = false;
    #region Tooltip
    [Tooltip("Время невосприимчивости в секундах после попадания")]
    #endregion
    public float hitImmunityTime;
    #region Tooltip
    [Tooltip("Выберите, чтобы отобразить индикатор здоровья противника")]
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