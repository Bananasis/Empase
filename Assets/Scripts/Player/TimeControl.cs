using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Player))]
public class TimeControl : TimeDialator, IShiftAbility
{
    public ShiftType type => ShiftType.Time;
    [Inject] private IInputProvider _inputProvider;
    [SerializeField] private float minScale = 0.25f;
    [SerializeField] private float maxScale = 4f;
    [SerializeField] private float dialation = 1;
    [SerializeField] private Slider slider;
    private float log2 { get; } = Mathf.Log(2);

    private IDisposable connection;

    protected override void OnEnable()
    {
        base.OnEnable();
        slider.gameObject.SetActive(true);
        slider.minValue = Mathf.Log(minScale) / log2;
        slider.maxValue = Mathf.Log(maxScale) / log2;
        dialation = 1;
        slider.value = Mathf.Log(dialation) / log2;
        connection = _inputProvider.OnScrollShift.Subscribe((delta) =>
        {
            dialation = Mathf.Clamp(dialation * (1 + delta.y / 5), minScale, maxScale);
            slider.value = Mathf.Log(dialation) / log2;
        });
    }


    protected override void OnDisable()
    {
        slider.gameObject.SetActive(false);
        connection.Dispose();
        base.OnDisable();
    }

    public override float GetDialition(CellData pos)
    {
        return dialation;
    }
}