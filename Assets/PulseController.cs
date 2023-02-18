using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Cell))]
public class PulseController : MonoBehaviour
{
    [SerializeField] private float defaultMassDefect;
    [SerializeField] private float massDefectMultiplier;
    [SerializeField] private float minMassDefect = 1;
    [SerializeField] private float maxMassDefect = 2;

    [SerializeField, Inject, HideInInspector]
    private LevelManager _levelManager;

    private float timeOffset;
    private Cell cell;

    private void Awake()
    {
        cell = GetComponent<Cell>();
    }

    private void OnEnable()
    {
        timeOffset = Random.value * 100; //todo make deterministic
        defaultMassDefect = cell.cData.cellMass.massDefect;
    }

    private void FixedUpdate()
    {
        massDefectMultiplier =
            (maxMassDefect - minMassDefect) * Mathf.Clamp01(Mathf.Sin(_levelManager.time + timeOffset) * 1.5f + 1) +
            minMassDefect;
        cell.cData.cellMass.massDefect = massDefectMultiplier * defaultMassDefect;
    }
}