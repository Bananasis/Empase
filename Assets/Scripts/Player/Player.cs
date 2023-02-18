using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : Cell
{
    public PlayerData playerData;
    [Inject] private ICellPool _cellPool;
    [Inject] private IGlobalCellStats _gCellStats;
    [Inject] private IInputProvider _inputProvider;
    private readonly Dictionary<ShiftType, Behaviour> _shiftAbilities = new Dictionary<ShiftType, Behaviour>();
    private readonly Dictionary<PropulsionType, Behaviour> _mouseAbilities = new Dictionary<PropulsionType, Behaviour>();


    protected override void OnAwake()
    {
        base.OnAwake();
        foreach (var component in GetComponents<Behaviour>())
        {
            switch (component)
            {
                case IShiftAbility shift:
                    shift._inputProvider = _inputProvider;
                    _shiftAbilities[shift.type] = component;
                    break;
                case IMouseAbility propulsion:
                    propulsion._inputProvider = _inputProvider;
                    _mouseAbilities[propulsion.type] = component;
                    break;
            }
        }

        SetShiftAbility(_gameManager.CurShiftAbility.val);
        SetMouseAbility(_gameManager.CurMouseAbility.val);
        _gameManager.CurMouseAbility.OnChange.AddListener(SetMouseAbility);
        _gameManager.CurShiftAbility.OnChange.AddListener(SetShiftAbility);
    }

    private void SetShiftAbility(ShiftType type)
    {
        foreach (var keyValue in _shiftAbilities)
        {
            keyValue.Value.enabled = false;
        }

        _shiftAbilities[type].enabled = true;
    }

    private void SetMouseAbility(PropulsionType type)
    {
        foreach (var keyValue in _mouseAbilities)
        {
            keyValue.Value.enabled = false;
        }

        _mouseAbilities[type].enabled = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerData.timeDialition = 1;
        playerData.acceleration = Vector2.zero;
    }

    public override void Die()
    {
        SetSize(0);
        OnDeath.Invoke();
        _cellPool.ReturnPlayer();
    }

    public override void UpdatePropertyBlock()
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(Dencity, Mathf.Sqrt(cData.cellMass.size));
        mpb.SetFloat(Id, (float) id % 1000);
        mpb.SetFloat(Weight, 0);
        sr.SetPropertyBlock(mpb);
    }

    public override void UpdateCell()
    {
        base.UpdateCell();
        _gCellStats.playerPosition = cData.position;
    }

    public override void UpdateVelocity()
    {
        cData.velocity += (lData.gravityAcceleration + playerData.acceleration) * lData.timeMultiplier *
                          playerData.timeDialition *
                          Time.fixedDeltaTime;
        body.velocity = cData.velocity * lData.timeMultiplier;
        Debug.DrawLine(cData.position, cData.position + cData.velocity, Color.yellow);
    }
}

[Serializable]
public struct PlayerData
{
    public ShiftType shiftType;
    public PropulsionType propulsionType;
    public Vector2 acceleration;
    public float timeDialition;
    public float defaultMassDefect;
}

public enum ShiftType
{
    None,
    Time,
    PersonalTime,
    Size,
    Attraction,
}

public enum PropulsionType
{
    Propulsion,
    ReactivePropulsion,
    GravityPropulsion,
    Hook
}