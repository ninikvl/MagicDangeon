using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    #region Tooltip
    [Tooltip("Заполняется дочерним компонентом TrailRenderer")]
    #endregion
    [SerializeField] private TrailRenderer trailRenderer;
    private float ammoRange = 0f;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetailsSO;
    private float ammoChargeTimer;
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetailsSO.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        //расчёт вектора движения снарядов
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;
        ammoRange -= distanceVector.magnitude;
        if (ammoRange < 0f)
        {
            DisableAmmo();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DisableAmmo();
    }

    /// <summary>
    /// Инициализирует 
    /// </summary>
    public void InitializeAmmo(AmmoDetailsSO ammoDetailsSO, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        #region Ammo

        this.ammoDetailsSO = ammoDetailsSO;
        SetFireDirection(ammoDetailsSO, aimAngle, weaponAimAngle, weaponAimDirectionVector);
        spriteRenderer.sprite = ammoDetailsSO.ammoSprite;

        if (ammoDetailsSO.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetailsSO.ammoChargeTime;
            SetAmmoMaterial(ammoDetailsSO.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetailsSO.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        ammoRange = ammoDetailsSO.ammoRange;
        this.ammoSpeed = ammoSpeed;
        this.overrideAmmoMovement = overrideAmmoMovement;
        gameObject.SetActive(true);

        #endregion Ammo

        #region Trail
        if (ammoDetailsSO.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetailsSO.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetailsSO.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetailsSO.ammoTrailEndWidth;
            trailRenderer.time = ammoDetailsSO.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }


        #endregion Trail
    }

    /// <summary>
    /// Установка направления и угола стрельбы боеприпасами на основе входного угла и направления, скорректированных
    /// случайным разбросом
    /// </summary>
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpeed = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }

        fireDirectionAngle += spreadToggle * randomSpeed;
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    /// <summary>
    /// Отключение снаряда - возвращение объекта в пул
    /// </summary>
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Установка материала снаряда 
    /// </summary>
    private void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);
    }
#endif
    #endregion

}
