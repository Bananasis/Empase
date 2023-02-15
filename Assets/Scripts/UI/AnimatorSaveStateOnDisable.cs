using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorSaveStateOnDisable : MonoBehaviour
{
    private Animator _animator;

    private int _animation = -1;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        if (_animation == -1) return;
        _animator.Play(_animation);
        
    }

    private void OnDisable()
    {
        _animation = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
    }
}
