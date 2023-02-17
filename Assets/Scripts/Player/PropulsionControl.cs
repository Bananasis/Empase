using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PropulsionControl : MonoBehaviour,IMouseAbility
{
    private Player player;
    [Inject] private IInputProvider _inputProvider;
    [SerializeField] private float propulsion = 1;
    private bool pressed;
    public PropulsionType type => PropulsionType.Propulsion;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        pressed = Input.GetMouseButton(0);
    }

    private IDisposable connection;

    private void OnEnable()
    {
        connection =_inputProvider.OnMousePress.Subscribe((truple) =>
        {
            var (vector2, _, item3) = truple;
            if (!item3)
            {
                player.playerData.acceleration = Vector2.zero;
                return;
            }

            var dir = (player.cData.position - vector2).normalized;
            player.playerData.acceleration = dir * propulsion * player.cData.cellMass.size;
        });
    }

    private void OnDisable()
    {
        connection.Dispose();
    }


}