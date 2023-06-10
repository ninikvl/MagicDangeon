using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MaterializeEffect))]
public class Chest : MonoBehaviour, IUseable
{
    #region Tooltip
    [Tooltip("Set this to the colour to be used for the materialization effect")]
    #endregion Tooltip
    [ColorUsage(false, true)]
    [SerializeField] private Color materializeColor;
    #region Tooltip
    [Tooltip("Set this to the time is will take to materialize the chest")]
    #endregion Tooltip
    [SerializeField] private float materializeTime = 3f;
    #region Tooltip
    [Tooltip("Populate withItemSpawnPoint transform")]
    #endregion Tooltip
    [SerializeField] private Transform itemSpawnPoint;
    private int healthPercent;
    private WeaponDetailsSO weaponDetails;
    private int ammoPercent;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private MaterializeEffect materializeEffect;
    private bool isEnabled = false;
    private ChestState chestState = ChestState.closed;
    private GameObject chestItemGameObject;
    private ChestItem chestItem;
    private TextMeshPro messageTextTMP;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materializeEffect = GetComponent<MaterializeEffect>();
        messageTextTMP = GetComponentInChildren<TextMeshPro>();
    }

    /// <summary>
    /// Инициализировать сундук и либо сделать его видимым, либо реализовать его
    /// </summary>
    public void Initialize(bool shouldMaterialize, int healthPercent, WeaponDetailsSO weaponDetails, int ammoPercent)
    {
        this.healthPercent = healthPercent;
        this.weaponDetails = weaponDetails;
        this.ammoPercent = ammoPercent;

        if (shouldMaterialize)
        {
            StartCoroutine(MaterializeChest());
        }
        else
        {
            EnableChest();
        }
    }

    /// <summary>
    /// материализовать сундук
    /// </summary>
    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor, materializeTime, spriteRendererArray, GameResources.Instance.litMaterial));

        EnableChest();
    }

    /// <summary>
    /// Включение сундука
    /// </summary>
    private void EnableChest()
    {
        // Set use to enabled
        isEnabled = true;
    }

    /// <summary>
    /// действие будет различаться в зависимости от состояния сундука.
    /// </summary>
    public void UseItem()
    {
        if (!isEnabled) return;

        switch (chestState)
        {
            case ChestState.closed:
                OpenChest();
                break;

            case ChestState.healthItem:
                CollectHealthItem();
                break;

            case ChestState.ammoItem:
                CollectAmmoItem();
                break;

            case ChestState.weaponItem:
                CollectWeaponItem();
                break;

            case ChestState.empty:
                return;

            default:
                return;
        }
    }

    /// <summary>
    /// Открытие сундука при первом использовании
    /// </summary>
    private void OpenChest()
    {
        animator.SetBool(Settings.use, true);

        //// звуковой эффект открытия сундука
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        // Проверка, есть ли у игрока уже оружие - если да, установка  для оружия значение null
        if (weaponDetails != null)
        {
            if (GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
                weaponDetails = null;
        }

        UpdateChestState();
    }

    /// <summary>
    /// Создание предметов, основываясь на том, что должно быть создано, и состоянии сундука
    /// </summary>
    private void UpdateChestState()
    {
        if (healthPercent != 0)
        {
            chestState = ChestState.healthItem;
            InstantiateHealthItem();
        }
        else if (ammoPercent != 0)
        {
            chestState = ChestState.ammoItem;
            InstantiateAmmoItem();
        }
        else if (weaponDetails != null)
        {
            chestState = ChestState.weaponItem;
            InstantiateWeaponItem();
        }
        else
        {
            chestState = ChestState.empty;
        }
    }

    /// <summary>
    /// создание экземпляра предмета из сундука
    /// </summary>
    private void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }

    /// <summary>
    /// создание экземпляра предмета здоровья, который игрок должен собрать
    /// </summary>
    private void InstantiateHealthItem()
    {
        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.heartIcon, healthPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }


    /// <summary>
    /// сбор здоровья и добавление его к здоровью игроков
    /// </summary>
    private void CollectHealthItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        // Добавление здоровья игроку
        GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);

        //// Воспроизведение звукового эффекта
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

        healthPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    /// <summary>
    /// Создание экземпляра предмета боеприпаса, который игрок должен собрать
    /// </summary>
    private void InstantiateAmmoItem()
    {
        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.bulletIcon, ammoPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }


    /// <summary>
    /// Сбор боеприпасов и добавление к боеприпасам в текущем оружии игрока
    /// </summary>
    private void CollectAmmoItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer();

        // обновление боеприпасов для текущего оружия
        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPercent);

        //// Воспроизведение звукового эффекта
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

        ammoPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    /// <summary>
    /// создание экземпляра предмета оружия, который игрок должен собрать
    /// </summary>
    private void InstantiateWeaponItem()
    {
        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite, weaponDetails.weaponName, itemSpawnPoint.position, materializeColor);
    }

    /// <summary>
    /// Подбор оружия и добавление его к игроку
    /// </summary>
    private void CollectWeaponItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        // Если у игрока еще нет оружия, то добавление его в список игроков
        if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            // добавление оружия игроку
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);

            //// Воспроизведение звукового эффекта
            //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
        }

        else
        {
            // отобразить сообщение о том, что уже есть оружие
            StartCoroutine(DisplayMessage("WEAPON\nALREADY\nEQUIPPED", 5f));

        }
        weaponDetails = null;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    /// <summary>
    /// отобразить сообщение над сундуком
    /// </summary>
    private IEnumerator DisplayMessage(string messageText, float messageDisplayTime)
    {
        messageTextTMP.text = messageText;

        yield return new WaitForSeconds(messageDisplayTime);

        messageTextTMP.text = "";
    }
}