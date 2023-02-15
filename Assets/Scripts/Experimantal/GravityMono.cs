using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[Serializable]
public struct MassDot
{
    public Vector2 pos;
    public float gravitationalMass;
    public float attraction;
    public float rotation;
    public float inertialMass;
}


public class GravityMono : MonoBehaviour
{
    public int massDotMax = 5000;
    public MassDotMono pref;
    public ComputeShader gravity;
    private ComputeBuffer _massDotBuffer;
    private ComputeBuffer _accelerationBuffer;
    public MassDotMono[] massDotMonos;
    public Transform[] massDotTransforms;
    public float speed = 1;
    public MassDot[] massDots;

    public Vector2[] velocities;

    public Vector2[] accelerations;
    // Start is called before the first frame update

    private void Awake()
    {
        _massDotBuffer = new ComputeBuffer(massDotMax, sizeof(float) * 6);
        _accelerationBuffer = new ComputeBuffer(massDotMax, sizeof(float) * 2);
    }

    private void OnDestroy()
    {
        _massDotBuffer.Release();
    }


    private void Start()
    {
        CreateDots();
    }

    void CreateDots()
    {
        massDotMonos = new MassDotMono[massDotMax];
        massDots = new MassDot[massDotMax];
        massDotTransforms = new Transform[massDotMax];
        velocities = new Vector2[massDotMax];
        accelerations = new Vector2[massDotMax];
        
        for (int i = 0; i < massDotMax; i++)
        {
            var mass = 20*Random.value+1;
            massDotMonos[i] = Instantiate(pref, transform);
            massDotMonos[i].transform.localScale = new Vector3(mass, mass);
            massDotTransforms[i] = massDotMonos[i].transform;
            massDotMonos[i].data = new MassDot
            {
                pos = new Vector2(Random.value-0.5f, Random.value-0.5f) * 100f,
                gravitationalMass = mass/20,
                inertialMass = 1,
                rotation = 0.1f,
                attraction = 1,
                
            };
            massDots[i] = massDotMonos[i].data;
        }
    }

    // Update is called once per frame
    private float time = 0;
    public float timeout = 0.2f;

    void FixedUpdate()
    {
        var dTime = Time.fixedDeltaTime;
        for (var i = 0; i < massDotMonos.Length; i++)
        {
            velocities[i] += accelerations[i] * dTime*speed;
            accelerations[i] = Vector2.zero;
            massDots[i].pos += velocities[i] * dTime*speed;
            massDotTransforms[i].position = massDots[i].pos;
        }

        time += Time.fixedDeltaTime;
        if (time < timeout) return;
        computeReady = true;
    }

    private bool computeReady;

    private void Update()
    {
        if (!computeReady || bufferLocked) return;
        ComputeGravity();
        time = 0;
        computeReady = false;
    }

    private bool bufferLocked;

    private void ComputeGravity()
    {
        _massDotBuffer.SetData(massDots);
        _accelerationBuffer.SetData(accelerations);
        gravity.SetInt("mass_dot_count", massDotMax);
        gravity.SetFloat("delta_time", time);
        gravity.SetBuffer(0, "mass_dots", _massDotBuffer);
        gravity.SetBuffer(0, "accelerations", _accelerationBuffer);
        gravity.Dispatch(0, 32, 32, 1);
        AsyncGPUReadback.Request(_accelerationBuffer, OnCompleteReadBack);
        bufferLocked = true;
        computeReady = false;
        
  
    }

    public void OnCompleteReadBack(AsyncGPUReadbackRequest rec)
    {
        if (rec.hasError) return;
        _accelerationBuffer.GetData(accelerations);
        bufferLocked = false;

    }
}