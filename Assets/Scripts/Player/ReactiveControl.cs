using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Player))]
public class ReactiveControl : MonoBehaviour,IMouseAbility
{
    public PropulsionType type => PropulsionType.ReactivePropulsion;
    [Inject] private IInputProvider _inputProvider;
    private Player player;
    [Inject] private ICellPool _cellPool;
    [SerializeField] private float propulsion = 1;
    [SerializeField] private float propulsionMass = 0.02f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }



    private IDisposable connection;
    private void OnEnable()
    {
        connection = _inputProvider.OnMouseClick.Subscribe((pos) =>
        {
            var cd = player.cData;
            var mass = cd.cellMass.mass * propulsionMass;
            var newMass = cd.cellMass.mass - mass;
            var newSize = Mathf.Sqrt(newMass / Mathf.PI);
            var littleSize = Mathf.Sqrt(mass / Mathf.PI);
            player.SetSize(newSize);
            var dir = (pos - cd.position).normalized;
            var vel = cd.velocity + dir * propulsion * cd.cellMass.size;
            _cellPool.GetCell(
                new CellData()
                {
                    position = cd.position + dir * (newSize + littleSize),
                    velocity = vel,
                    type = cd.type,
                    cellMass = {mass = mass}
                }
            );
            player.cData.velocity += propulsion * cd.cellMass.size * propulsionMass * -dir;
        });
    }

    private void OnDisable()
    {
        connection.Dispose();
        
    }

 
}