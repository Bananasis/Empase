using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]

public class CellButtonMultiplier : MonoBehaviour
{
    public float weightMultiplier;
    public float dencityMultiplier;

    public bool passed;
    // Start is called before the first frame update

    private Image image;
    private static readonly int WeightMultiplier = Shader.PropertyToID("Weight");
    private static readonly int Dencity = Shader.PropertyToID("Dencity");

    void Start()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material);
        
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat(WeightMultiplier, weightMultiplier - (passed ? 0.2f : 0));
        image.material.SetFloat(Dencity, dencityMultiplier);
    }

    public void SetId(int id)
    {
        image.material.SetFloat("Id", id*1000);
    }
}