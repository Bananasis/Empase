using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailController : MonoBehaviour
{
    private TrailRenderer tr;

    private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        tr = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var sqrt = Mathf.Sqrt(parent.localScale.x);
        tr.time = sqrt;
        tr.startWidth = parent.localScale.x*0.8f;
    }
}


