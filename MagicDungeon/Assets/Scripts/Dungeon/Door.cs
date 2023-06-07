using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Door : MonoBehaviour
{
    #region Header Object References
    [Space(10)]
    [Header("�������� �������")]
    #endregion
    #region Tooltip
    [Tooltip("����������� BoxCollider2D �� DoorColider")]
    #endregion
    [SerializeField] private BoxCollider2D doorCollider;
    private BoxCollider2D doorTrigger;
    private bool isOpen = false;
    private bool previouslyOpened = false;
    private Animator animator;

    private void Awake()
    {
        doorCollider.enabled = false;
        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();

    }

    private void OnEnable()
    {
        //���� ����� ������� ������, �� ������������ ������ �����������, ��-�� ����� ������������ ��������
        animator.SetBool(Settings.open, isOpen);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Settings.playerTag || collision.tag == Settings.playerWeapon)
        {
            OpenDoor();
        }
    }

    /// <summary>
    /// ������� �����
    /// </summary>
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            previouslyOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;

            animator.SetBool(Settings.open, true);
        }
    }

    /// <summary>
    /// �������� �����
    /// </summary>
    public void LockDoor()
    {
        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        animator.SetBool(Settings.open, false);

        //���������� ������ ��������� ��� ����� � �������� ����� �������
        if (GameManager.Instance.GetPreviouslyRoom().roomNodeType.isCorridorEW || GameManager.Instance.GetPreviouslyRoom().roomNodeType.isCorridorNs)
        {
            foreach (Door door in GameManager.Instance.GetPreviouslyRoom().doorArray)
            {
                door.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// �������� �����
    /// </summary>
    public void UnlockDoor()
    {
        doorCollider.enabled = false;
        doorTrigger.enabled = true;

        if (previouslyOpened == true)
        {
            isOpen = false;
            OpenDoor();
        }
    }

    #region Vakidation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);
    }

#endif
    #endregion
}
