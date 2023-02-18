using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Player))]
public class PersonalTimeControl : MonoBehaviour,IShiftAbility
{
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;
    
    private Player player;
    public IInputProvider _inputProvider { get; set; }
    public ShiftType type => ShiftType.PersonalTime;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private IDisposable connection;

    private void OnEnable()
    {
        connection = _inputProvider.OnScrollShift.Subscribe((delta) =>
            player.playerData.timeDialition =
                Mathf.Clamp(player.playerData.timeDialition * (1 + delta.y / 5), minScale, maxScale)
        );
    }

    private void OnDisable()
    {
        connection.Dispose();
    }

    
}