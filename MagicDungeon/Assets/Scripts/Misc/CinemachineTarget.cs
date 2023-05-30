using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] private Transform cursorTarget;

    private void Awake()
    {
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    private void Start()
    {
        SetCinemachineTargetGroup();
    }

    private void Update()
    {
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition();
    }

    /// <summary>
    /// Установка группы целей для камеры
    /// </summary>
    private void SetCinemachineTargetGroup()
    {
        CinemachineTargetGroup.Target cinemachineTarget_player = new CinemachineTargetGroup.Target
        {
            weight = 1f,
            radius = 2.5f,
            target = GameManager.Instance.GetPlayer().transform
        };
        CinemachineTargetGroup.Target cinemachineTarget_cursor = new CinemachineTargetGroup.Target
        {
            weight = 1f,
            radius = 1f,
            target = cursorTarget
        };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target []
        {
            cinemachineTarget_player,
            cinemachineTarget_cursor
        };

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;
    }

    
}
