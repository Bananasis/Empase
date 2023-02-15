using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class BlackHoleButtonController : MonoBehaviour
{
    public float gradientMultiplier;

    // Start is called before the first frame update

    private Image image;
    private static readonly int GradientMultiplier = Shader.PropertyToID("GradientMultiplier");

    void Start()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material);
       
    }

    // Update is called once per frame
    void Update()
    {
        image.material.SetFloat(GradientMultiplier, gradientMultiplier);
    }
}