using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private float minScaleMultiplier = 1;
    [SerializeField] private float maxScaleMultiplier = 20;
    [Inject] private IInputProvider _inputProvider;
    [Inject] private IGlobalCellStats _cellManager;
    public float minScale => minScaleMultiplier * Mathf.Max(_cellManager.playerSize, 1);
    public float maxScale => maxScaleMultiplier * Mathf.Max(_cellManager.maxPlayerSize, 1);
    [SerializeField] private Transform player;
    [Range(0, 1)] public float scale;
    public static float cameraScale = 1;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private IDisposable connection;

    private void OnEnable()
    {
        connection = _inputProvider.OnScrollNoShift.Subscribe((delta) =>
        {
            var size = Mathf.Clamp(-delta.y + scale * maxScale + minScale, minScale, maxScale);
            cameraScale = (size - minScale) / maxScale;
            scale = cameraScale;
        });
    }

    private void OnDisable()
    {
        connection.Dispose();
    }

    void Update()
    {
        var size = Mathf.Clamp(scale * maxScale + minScale, minScale, maxScale);
        cameraScale = (size - minScale) / maxScale;
        scale = cameraScale;
        var pos = player.position;
        transform.position = new Vector3(pos.x, pos.y, Mathf.Lerp(transform.position.z, -size, Time.deltaTime * 10));
    }
}