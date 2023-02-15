using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GravityPropulsionControl : GravityMassDot,IMouseAbility
{
    private Player player;
    public PropulsionType type => PropulsionType.GravityPropulsion;
    [Inject] private IInputProvider _inputProvider;
    [SerializeField] private float propulsion = 1;
    [SerializeField] private float acceleration = 0;
    [SerializeField] private Vector2 _pos;
    public override Vector2 position => _pos;
    public override float mass => acceleration;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private IDisposable connection;

    protected override void OnEnable()
    {
        base.OnEnable();
        connection = _inputProvider.OnMousePress.Subscribe((truple) =>
        {
            if (!truple.Item3)
            {
                acceleration = 0;
            }

            acceleration = truple.Item2 * propulsion;
            _pos = truple.Item1;
        });
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        connection.Dispose();
    }


}