using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class AttractorButtonController : MonoBehaviour
{
    public float weightMultiplier;

    // Start is called before the first frame update

    private Image image;
    private static readonly int WeightMultiplier = Shader.PropertyToID("Weight");

    void Start()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material);
        image.material.SetFloat("Id",Random.value*1000);
       
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat(WeightMultiplier, weightMultiplier);
    }
}