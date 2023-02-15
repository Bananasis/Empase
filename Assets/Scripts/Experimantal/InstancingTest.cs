using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InstancingTest : MonoBehaviour
{
    private MaterialPropertyBlock block;
    private SpriteRenderer rend;
    [SerializeField] private float cell;
    void Awake()
    {
        cell = Random.value*10;
        rend = gameObject.GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
        SetBlock();
    }
 
    private void SetBlock()
    {
     
        rend.GetPropertyBlock(block);
        block.SetFloat("Vector1_209956d6f7404e099902cb0f0f16851d", cell);
        rend.SetPropertyBlock(block);
    }

    private void Update()
    {
        rend.GetPropertyBlock(block);
        block.SetFloat("Vector1_209956d6f7404e099902cb0f0f16851d", cell);
        rend.SetPropertyBlock(block);
    }
}
