using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VoronoiController : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private SpriteRenderer sr;
    [SerializeField] private float multiplier = 1;
    [SerializeField] private bool SizeDependant = false;
    [SerializeField] private float noiseScale;
    private float scale;

    private void Awake()
    {
        scale = transform.localScale.x;
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        var t = transform;
        var pos = t.position;
        mpb.SetVector("PosScale", new Vector4(pos.x, pos.y, t.localScale.x));
        mpb.SetFloat("Weight", (CameraController.cameraScale - 4) / 15);
        sr.SetPropertyBlock(mpb);
    }

    private void Update()
    {
        UpdateCellSize();
    }

    private void UpdateCellSize()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        sr.GetPropertyBlock(mpb);
        var t = transform;
        var pos = t.position;
        mpb.SetVector("PosScale", new Vector4(pos.x, pos.y, t.localScale.x));
        mpb.SetFloat("Weight",
            Mathf.Clamp(
                multiplier * (CameraController.cameraScale) *
                (Mathf.Min(SizeDependant ? transform.localScale.x / 20 : 1, 1)), -0.1f, 1.1f));
        sr.SetPropertyBlock(mpb);
    }

    public void SetSize(float scaleMultiplier)
    {
        transform.localScale = scale * scaleMultiplier * 0.005f * new Vector3(1, 1);
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat("Scale", noiseScale * scaleMultiplier);
        sr.SetPropertyBlock(mpb);
    }
}