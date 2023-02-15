using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeMeshFilter : MonoBehaviour
{
    [SerializeField] private float sDist = 0.5f;
    [SerializeField] private float oDist = 0.5f;
    [SerializeField] private float sSize = 1;
    [SerializeField] private float oSize = 1;
    [SerializeField] private Vector2 dir = Vector2.up;

    private MeshFilter _mesh;
    private MeshRenderer _meshR;

    private void Start()
    {
        _meshR = GetComponent<MeshRenderer>();
        _mesh = GetComponent<MeshFilter>();
    }

    void Update()
    {
        Mesh mesh = _mesh.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];


        // uvs[0] = new Vector2(0, 0);
        // uvs[1] = new Vector2(1, 0);
        // uvs[2] = new Vector2(0, 1);
        // uvs[3] = new Vector2(1, 1);

        Vector3 dNormalized = dir.normalized;
        var dSide = new Vector3(dNormalized.y, -dNormalized.x, 0);
        var sSideCoef = Mathf.Sqrt(1 - sDist * sDist);
        var oSideCoef = Mathf.Sqrt(1 - oDist * oDist);

        vertices[0] = (dNormalized * sDist - sSideCoef * dSide) * sSize;
        vertices[1] = (dNormalized * sDist + sSideCoef * dSide) * sSize;
        vertices[2] = (-dNormalized * oDist - oSideCoef * dSide) * oSize + dNormalized*(sSize+oSize);
        vertices[3] = (-dNormalized * oDist + oSideCoef * dSide) * oSize + dNormalized*(sSize+oSize);

        uvs[0] = (Vector2) vertices[0] + new Vector2(0.5f, 0.5f);
        uvs[1] = (Vector2) vertices[1] + new Vector2(0.5f, 0.5f);
        uvs[2] = (Vector2) vertices[2] + new Vector2(0.5f, 0.5f);
        uvs[3] = (Vector2) vertices[3] + new Vector2(0.5f, 0.5f);

        mesh.vertices = vertices;
        mesh.uv = uvs;
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        _meshR.GetPropertyBlock(mpb);
         mpb.SetVector("Dir", dNormalized);
         mpb.SetFloat("sSize", sSize);
         mpb.SetFloat("oSize", oSize);
        // mpb.SetVector("Pos4", uvs[3]);
        _meshR.SetPropertyBlock(mpb);
    }
}