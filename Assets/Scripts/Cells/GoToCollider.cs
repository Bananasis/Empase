using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Cell))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GoToCollider : MonoBehaviour
{
    [Inject,SerializeField,HideInInspector] private LevelManager _levelManager;
    private Cell _cell;
    private float loading;
    private MaterialPropertyBlock mpb;
    private SpriteRenderer sr;
    private bool reached;
    private bool loaded;

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        _cell = GetComponent<Cell>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        loaded = false;
        reached = false;
        loading = 0;
    }

    private void Update()
    {
        if (loaded) return;
        loading += (reached ? Time.fixedDeltaTime : -Time.fixedDeltaTime)/5;
        loading = Mathf.Clamp(loading, 0, 1.1f);
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("Loading", loading);
        sr.SetPropertyBlock(mpb);
        
        if (loading < 1) return;
        loaded = true;
        _levelManager.targetsLeft--;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null) return;
        Collide(player);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null) return;
        Collide(player);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player == null) return;
        reached = false;
    }

    private void Collide(Cell player)
    {
        reached = false;
        var dist = (player.cData.position - _cell.cData.position).magnitude;
        if (dist < player.cData.size) reached = true;
    }
}