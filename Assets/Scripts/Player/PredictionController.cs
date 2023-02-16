using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Cell))]
public class PredictionController : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    [SerializeField] private float widthMultiplier = 0.05f;
    [SerializeField] private int sampleSize = 10;
    [SerializeField] private float deltaTime = 0.1f;
    [SerializeField] private float sampleTime = 1;
    [SerializeField] private float sampleDistance = 1;
    [SerializeField] private int iterationCap = 10;
    [SerializeField] private SampleType type;
    [Inject] private IGravityManager _gravityManager;
    private Cell cell;

    private Vector3[] dots;

    private Vector3[] softDots;

    // Start is called before the first frame update
    void Start()
    {
        dots = new Vector3[sampleSize];
        softDots = new Vector3[sampleSize];
        lr.positionCount = sampleSize;
        cell = GetComponent<Cell>();
    }

    private void FixedUpdate()
    {
        lr.startWidth = cell.cData.size*widthMultiplier;
        lr.endWidth = cell.cData.size*widthMultiplier;
        var pos = transform.position;
        dots[0] = pos;
        var timePassed = 0f;
        var vel = (Vector3) cell.cData.velocity;
        var pathLength = 0f;

        for (var i = 1; i < dots.Length; i++)
        {
            bool sampleComplete = false;
            var iter = 0;
            while (!sampleComplete)
            {
                iter++;
                sampleComplete = type switch
                {
                    SampleType.Time => timePassed < i * sampleTime,
                    SampleType.Distance => pathLength < i * sampleDistance,
                    SampleType.Min => timePassed < i * sampleTime || pathLength < i * sampleDistance,
                    SampleType.Max => timePassed < i * sampleTime && pathLength < i * sampleDistance,
                    _ => true
                } || iter > iterationCap;

                var dCoord = deltaTime * vel;
                pos += dCoord;
                foreach (var attractor in _gravityManager.reg)
                {
                    var dir = (Vector3) attractor.position - pos;
                    var dist = dir.magnitude;
                    if (dist < cell.cData.size) dist = cell.cData.size;
                    vel += dir.normalized * deltaTime * attractor.mass / (dist * dist);
                }

                timePassed += deltaTime;
                pathLength += dCoord.magnitude;
            }

            dots[i] = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < dots.Length; i++)
        {
            softDots[i] = Vector3.Lerp(softDots[i], dots[i], Time.deltaTime * 20 * (dots.Length-i));
        }

        lr.SetPositions(softDots);
    }
}

public enum SampleType
{
    Time,
    Distance,
    Min,
    Max,
}