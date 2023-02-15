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
        lastSize = player.cData.mass;

        connections.Add(_inputProvider.OnScrollShift.Subscribe((delta) =>
            {
                var curScale = scale;
                scale = Mathf.Clamp(scale * (1 + delta.y / 5), minScale, maxScale);
                player.cData.mass *= scale / curScale;
                lastSize = player.cData.mass;
            }
        ));
        connections.Add(player.OnSizeChange.Subscribe((s) =>
        {
            var deltaMass = player.cData.mass - lastSize;
            deltaMass = deltaMass > 0 ? deltaMass * (scale - 1) : 0;
            player.cData.mass += deltaMass;
            lastSize = player.cData.mass;
        }));
    }

    private void OnDisable()
    {
        connections.Dispose();
    }


}