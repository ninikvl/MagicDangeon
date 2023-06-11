using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [System.Serializable]
    private struct RangeByLevel
    {
        public DungeonLevelSO dungeonLevel;
        [Range(0, 100)] public int min;
        [Range(0, 100)] public int max;
    }

    #region Header CHEST PREFAB
    [Space(10)]
    [Header("Префаб сундука")]
    #endregion Header CHEST PREFAB
    #region Tooltip
    [Tooltip("Заполните префаб сундука")]
    #endregion Tooltip
    [SerializeField] private GameObject chestPrefab;

    #region Header CHEST SPAWN CHANCE
    [Space(10)]
    [Header("Шанс спавна сундука")]
    #endregion Header CHEST SPAWN CHANCE
    #region Tooltip
    [Tooltip("Минимальная вероятность появления сундука")]
    #endregion Tooltip
    [SerializeField] [Range(0, 100)] private int chestSpawnChanceMin;
    #region Tooltip
    [Tooltip("Максимальная вероятность появления сундука")]
    #endregion Tooltip
    [SerializeField] [Range(0, 100)] private int chestSpawnChanceMax;
    #region Tooltip
    [Tooltip("Вы можете изменить шанс появления сундука в зависимости от уровня подземелья")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> chestSpawnChanceByLevelList;

    #region Header CHEST SPAWN DETAILS
    [Space(10)]
    [Header("Деталии спавна сундука")]
    #endregion Header CHEST SPAWN DETAILS
    [SerializeField] private ChestSpawnEvent chestSpawnEvent;
    [SerializeField] private ChestSpawnPosition chestSpawnPosition;
    #region Tooltip
    [Tooltip("Минимальное количество предметов для создания (обратите внимание, что будет создано не более 1 каждого типа боеприпасов, здоровья и оружия")]
    #endregion Tooltip
    [SerializeField] [Range(0, 3)] private int numberOfItemsToSpawnMin;
    #region Tooltip
    [Tooltip("Максимальное количество предметов для создания (обратите внимание, что будет создано не более 1 для каждого типа боеприпасов, здоровья и оружия")]
    #endregion Tooltip
    [SerializeField] [Range(0, 3)] private int numberOfItemsToSpawnMax;

    #region Header CHEST CONTENT DETAILS
    [Space(10)]
    [Header("Деталии содержащихся в сундуке предметов")]
    #endregion Header CHEST CONTENT DETAILS
    #region Tooltip
    [Tooltip("Оружие, которое будет появляться на каждом уровне подземелья, и соотношение их появления")]
    #endregion Tooltip
    [SerializeField] private List<SpawnableObjectsByLevel<WeaponDetailsSO>> weaponSpawnByLevelList;
    #region Tooltip
    [Tooltip("Диапазон здоровья для появления на каждом уровне")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> healthSpawnByLevelList;
    #region Tooltip
    [Tooltip("Количество боеприпасов, которые будут появляться на каждом уровне")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> ammoSpawnByLevelList;

    private bool chestSpawned = false;
    private Room chestRoom;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;
    }

    /// <summary>
    /// Обработчик событий OnRoomChanged
    /// </summary>
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        // Получить комнату, в которой находится сундук
        if (chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }
        // Если сундук появился при входе в комнату
        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onRoomEntry && chestRoom == roomChangedEventArgs.room)
        {
            SpawnChest();
        }
    }

    /// <summary>
    /// Обработчик событий OnRoomEnemiesDefeated
    /// </summary>
    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        // Получить комнату, в которой находится сундук
        if (chestRoom == null)
        {
            chestRoom = GetComponentInParent<InstantiatedRoom>().room;
        }
        // сундук появляется, когда враги побеждены,
        if (!chestSpawned && chestSpawnEvent == ChestSpawnEvent.onEnemiesDefeated && chestRoom == roomEnemiesDefeatedArgs.room)
        {
            SpawnChest();
        }
    }

    /// <summary>
    /// Создание префаба судука
    /// </summary>
    private void SpawnChest()
    {
        chestSpawned = true;

        // Должен ли сундук создаваться на основе указанного шанса?
        if (!RandomSpawnChest()) return;

        // Получитение количества боеприпасов, здоровья и предметов оружия для Спавна (максимум по 1 для каждого)
        GetItemsToSpawn(out int ammoNum, out int healthNum, out int weaponNum);

        // создать экземпляр сундука
        GameObject chestGameObject = Instantiate(chestPrefab, this.transform);

        // Положение сундука
        if (chestSpawnPosition == ChestSpawnPosition.atSpawnerPosition)
        {
            chestGameObject.transform.position = this.transform.position;
        }
        else if (chestSpawnPosition == ChestSpawnPosition.atPlayerPosition)
        {
            // получить ближайшую позицию игрока
            Vector3 spawnPosition = HelperUtilities.GetSpawnPositionToPlayer(GameManager.Instance.GetPlayer().transform.position);

            // вычислить случайное изменение
            Vector3 variation = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

            chestGameObject.transform.position = spawnPosition + variation;
        }

        Chest chest = chestGameObject.GetComponent<Chest>();

        // Инициализировать сундук
        if (chestSpawnEvent == ChestSpawnEvent.onRoomEntry)
        {
            chest.Initialize(false, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));
        }
        else
        {
            chest.Initialize(true, GetHealthPercentToSpawn(healthNum), GetWeaponDetailsToSpawn(weaponNum), GetAmmoPercentToSpawn(ammoNum));
        }
    }

    /// <summary>
    /// проверяет, должен ли быть создан сундук, исходя из вероятности появления сундука - возвращает значение true, 
    /// если сундук должен быть создан, в противном случае значение false
    /// </summary>
    private bool RandomSpawnChest()
    {
        int chancePercent = Random.Range(chestSpawnChanceMin, chestSpawnChanceMax + 1);

        // проверяет, установлен ли процент вероятности переопределения для текущего уровня
        foreach (RangeByLevel rangeByLevel in chestSpawnChanceByLevelList)
        {
            if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                chancePercent = Random.Range(rangeByLevel.min, rangeByLevel.max + 1);
                break;
            }
        }

        int randomPercent = Random.Range(1, 100 + 1);

        if (randomPercent <= chancePercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// получить количество предметов для появления - максимум 1 из каждого - всего не более 3
    /// </summary>
    private void GetItemsToSpawn(out int ammo, out int health, out int weapons)
    {
        ammo = 0;
        health = 0;
        weapons = 0;

        int numberOfItemsToSpawn = Random.Range(numberOfItemsToSpawnMin, numberOfItemsToSpawnMax + 1);

        int choice;

        if (numberOfItemsToSpawn == 1)
        {
            choice = Random.Range(0, 3);
            if (choice == 0) { weapons++; return; }
            if (choice == 1) { ammo++; return; }
            if (choice == 2) { health++; return; }
            return;
        }
        else if (numberOfItemsToSpawn == 2)
        {
            choice = Random.Range(0, 3);
            if (choice == 0) { weapons++; ammo++; return; }
            if (choice == 1) { ammo++; health++; return; }
            if (choice == 2) { health++; weapons++; return; }
        }
        else if (numberOfItemsToSpawn >= 3)
        {
            weapons++;
            ammo++;
            health++;
            return;
        }
    }

    /// <summary>
    /// получить процент боезапаса для появления
    /// </summary>
    private int GetAmmoPercentToSpawn(int ammoNumber)
    {
        if (ammoNumber == 0) return 0;

        // Get ammo spawn percent range for level
        foreach (RangeByLevel spawnPercentByLevel in ammoSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    /// <summary>
    /// Получить количество здоровья для появления
    /// </summary>
    private int GetHealthPercentToSpawn(int healthNumber)
    {
        if (healthNumber == 0) 
            return 0;

        foreach (RangeByLevel spawnPercentByLevel in healthSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(0, 5);
            }
        }

        return 0;
    }

    /// <summary>
    /// Получить сведения об оружии для создания - вернуть значение null, 
    /// если оружие не должно быть создано или у игрока уже есть оружие
    /// </summary>
    private WeaponDetailsSO GetWeaponDetailsToSpawn(int weaponNumber)
    {
        if (weaponNumber == 0) return null;

        RandomSpawnableObject<WeaponDetailsSO> weaponRandom = new RandomSpawnableObject<WeaponDetailsSO>(weaponSpawnByLevelList);

        WeaponDetailsSO weaponDetails = weaponRandom.GetItem();

        return weaponDetails;
    }

    #region Validation
#if UNITY_EDITOR

    // Validate prefab details enetered
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(chestPrefab), chestPrefab);
        HelperUtilities.vakidateCheckPositiveRange(this, nameof(chestSpawnChanceMin), chestSpawnChanceMin, nameof(chestSpawnChanceMax), chestSpawnChanceMax, true);

        if (chestSpawnChanceByLevelList != null && chestSpawnChanceByLevelList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(chestSpawnChanceByLevelList), chestSpawnChanceByLevelList);

            foreach (RangeByLevel rangeByLevel in chestSpawnChanceByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelperUtilities.vakidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max, true);
            }
        }

        HelperUtilities.vakidateCheckPositiveRange(this, nameof(numberOfItemsToSpawnMin), numberOfItemsToSpawnMin, nameof(numberOfItemsToSpawnMax), numberOfItemsToSpawnMax, true);

        if (weaponSpawnByLevelList != null && weaponSpawnByLevelList.Count > 0)
        {
            foreach (SpawnableObjectsByLevel<WeaponDetailsSO> weaponDetailsByLevel in weaponSpawnByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(weaponDetailsByLevel.dungeonLevel), weaponDetailsByLevel.dungeonLevel);

                foreach (SpawnableObjectRatio<WeaponDetailsSO> weaponRatio in weaponDetailsByLevel.spawnableObjectRatioList)
                {
                    HelperUtilities.ValidateCheckNullValue(this, nameof(weaponRatio.dungeonObject), weaponRatio.dungeonObject);

                    HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponRatio.ratio), weaponRatio.ratio, true);
                }
            }
        }

        if (healthSpawnByLevelList != null && healthSpawnByLevelList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(healthSpawnByLevelList), healthSpawnByLevelList);

            foreach (RangeByLevel rangeByLevel in healthSpawnByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelperUtilities.vakidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max, true);
            }
        }

        if (ammoSpawnByLevelList != null && ammoSpawnByLevelList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(ammoSpawnByLevelList), ammoSpawnByLevelList);
            foreach (RangeByLevel rangeByLevel in ammoSpawnByLevelList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                HelperUtilities.vakidateCheckPositiveRange(this, nameof(rangeByLevel.min), rangeByLevel.min, nameof(rangeByLevel.max), rangeByLevel.max, true);
            }
        }

    }

#endif

    #endregion Validation

}