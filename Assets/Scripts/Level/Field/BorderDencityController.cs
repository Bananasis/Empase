using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BorderDencityController : MonoBehaviour
{
    private MaterialPropertyBlock mpb;
    private SpriteRenderer sr;

    [SerializeField] private float noiseScale;
    private static readonly int Dencity = Shader.PropertyToID("Dencity");
    private float scale;

    // Start is called before the first frame update
    void Awake()
    {
        mpb = new MaterialPropertyBlock();
        scale = transform.localScale.x;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    public void SetSize(float scaleMultiplier)
    {
        transform.localScale = scale * scaleMultiplier * 0.005f * new Vector3(1, 1);
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(Dencity, scaleMultiplier * noiseScale);
        sr.SetPropertyBlock(mpb);
    }
}