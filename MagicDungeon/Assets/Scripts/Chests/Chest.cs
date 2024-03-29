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
    /// ���������������� ������ � ���� ������� ��� �������, ���� ����������� ���
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
    /// ��������������� ������
    /// </summary>
    private IEnumerator MaterializeChest()
    {
        SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

        yield return StartCoroutine(materializeEffect.MaterializeRoutine(GameResources.Instance.materializeShader, materializeColor, materializeTime, spriteRendererArray, GameResources.Instance.litMaterial));

        EnableChest();
    }

    /// <summary>
    /// ��������� �������
    /// </summary>
    private void EnableChest()
    {
        // Set use to enabled
        isEnabled = true;
    }

    /// <summary>
    /// �������� ����� ����������� � ����������� �� ��������� �������.
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
    /// �������� ������� ��� ������ �������������
    /// </summary>
    private void OpenChest()
    {
        animator.SetBool(Settings.use, true);

        //// �������� ������ �������� �������
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.chestOpen);

        // ��������, ���� �� � ������ ��� ������ - ���� ��, ���������  ��� ������ �������� null
        if (weaponDetails != null)
        {
            if (GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
                weaponDetails = null;
        }

        UpdateChestState();
    }

    /// <summary>
    /// �������� ���������, ����������� �� ���, ��� ������ ���� �������, � ��������� �������
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
    /// �������� ���������� �������� �� �������
    /// </summary>
    private void InstantiateItem()
    {
        chestItemGameObject = Instantiate(GameResources.Instance.chestItemPrefab, this.transform);

        chestItem = chestItemGameObject.GetComponent<ChestItem>();
    }

    /// <summary>
    /// �������� ���������� �������� ��������, ������� ����� ������ �������
    /// </summary>
    private void InstantiateHealthItem()
    {
        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.heartIcon, healthPercent.ToString(), itemSpawnPoint.position, materializeColor);
    }


    /// <summary>
    /// ���� �������� � ���������� ��� � �������� �������
    /// </summary>
    private void CollectHealthItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        // ���������� �������� ������
        GameManager.Instance.GetPlayer().health.AddHealth(healthPercent);

        //// ��������������� ��������� �������
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.healthPickup);

        healthPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    /// <summary>
    /// �������� ���������� �������� ����������, ������� ����� ������ �������
    /// </summary>
    private void InstantiateAmmoItem()
    {
        InstantiateItem();

        chestItem.Initialize(GameResources.Instance.bulletIcon, ammoPercent.ToString() + "%", itemSpawnPoint.position, materializeColor);
    }


    /// <summary>
    /// ���� ����������� � ���������� � ����������� � ������� ������ ������
    /// </summary>
    private void CollectAmmoItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        Player player = GameManager.Instance.GetPlayer();

        // ���������� ����������� ��� �������� ������
        player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), ammoPercent);

        //// ��������������� ��������� �������
        //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.ammoPickup);

        ammoPercent = 0;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    /// <summary>
    /// �������� ���������� �������� ������, ������� ����� ������ �������
    /// </summary>
    private void InstantiateWeaponItem()
    {
        InstantiateItem();

        chestItemGameObject.GetComponent<ChestItem>().Initialize(weaponDetails.weaponSprite, weaponDetails.weaponName, itemSpawnPoint.position, materializeColor);
    }

    /// <summary>
    /// ������ ������ � ���������� ��� � ������
    /// </summary>
    private void CollectWeaponItem()
    {
        if (chestItem == null || !chestItem.isItemMaterialized) return;

        // ���� � ������ ��� ��� ������, �� ���������� ��� � ������ �������
        if (!GameManager.Instance.GetPlayer().IsWeaponHeldByPlayer(weaponDetails))
        {
            // ���������� ������ ������
            GameManager.Instance.GetPlayer().AddWeaponToPlayer(weaponDetails);

            //// ��������������� ��������� �������
            //SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.weaponPickup);
        }

        else
        {
            // ���������� ��������� � ���, ��� ��� ���� ������
            StartCoroutine(DisplayMessage("WEAPON\nALREADY\nEQUIPPED", 5f));

        }
        weaponDetails = null;

        Destroy(chestItemGameObject);

        UpdateChestState();
    }

    /// <summary>
    /// ���������� ��������� ��� ��������
    /// </summary>
    private IEnumerator DisplayMessage(string messageText, float messageDisplayTime)
    {
        messageTextTMP.text = messageText;

        yield return new WaitForSeconds(messageDisplayTime);

        messageTextTMP.text = "";
    }
}