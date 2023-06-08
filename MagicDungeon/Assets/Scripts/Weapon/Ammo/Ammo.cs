using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    #region Tooltip
    [Tooltip("����������� �������� ����������� TrailRenderer")]
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
    private bool isColliding = false;

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

        //������ ������� �������� ��������
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;
        ammoRange -= distanceVector.magnitude;
        if (ammoRange < 0f)
        {
            if (ammoDetailsSO.isPlayerAmmo)
            {
                StaticEventHandler.CallMultiplierEvent(false);
            }
            DisableAmmo();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding)
        {
            return;
        }
        
        // Deal Damage To Collision Object
        DealDamage(collision);

        DisableAmmo();

    }

    private void DealDamage(Collider2D collision)
    {
        Health health = collision.GetComponent<Health>();

        bool enemyHit = false;

        if (health != null)
        {
            // Set isColliding to prevent ammo dealing damage multiple times
            isColliding = true;

            health.TakeDamage(ammoDetailsSO.ammoDamage);

            // Enemy hit
            if (health.enemy != null)
            {
                enemyHit = true;
            }
        }

        // If player ammo then update multiplier
        if (ammoDetailsSO.isPlayerAmmo)
        {
            if (enemyHit)
            {
                // multiplier
                StaticEventHandler.CallMultiplierEvent(true);
            }
            else
            {
                // no multiplier
                StaticEventHandler.CallMultiplierEvent(false);
            }
        }

    }

    /// <summary>
    /// �������������� 
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
    /// ��������� ����������� � ����� �������� ������������ �� ������ �������� ���� � �����������, �����������������
    /// ��������� ���������
    /// </summary>
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpeed = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        isColliding = false;

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
    /// ���������� ������� - ����������� ������� � ���
    /// </summary>
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ��������� ��������� ������� 
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
