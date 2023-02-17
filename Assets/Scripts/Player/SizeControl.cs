using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Player))]
public class SizeControl : MonoBehaviour,IShiftAbility
{
    public ShiftType type => ShiftType.Size;
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float scale = 1;
    [Inject] private IInputProvider _inputProvider;
    private float lastSize = 1;

    //private float mass;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private readonly List<IDisposable> connections = new List<IDisposable>();

    private void OnEnable()
    {
        scale = 1;
        lastSize = player.cData.cellMass.mass;

        connections.Add(_inputProvider.OnScrollShift.Subscribe((delta) =>
            {
                scale = Mathf.Clamp(scale * (1 - delta.y / 5), minScale, maxScale);
                player.cData.cellMass.massDefect = scale * player.playerData.defaultMassDefect;
                player.OnSizeChange.Invoke((player.cData.cellMass.size,player.cData.cellMass.mass));
            }
        ));

    }

    private void OnDisable()
    {
        connections.Dispose();
    }


}