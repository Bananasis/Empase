using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoBehaviour
{
    //Here assign the volume game object. You could also use GetComponent
    private Volume volume;

    private DepthOfField depthOfField = null;
    private Camera cam;
    [SerializeField] private float min;
    [SerializeField] private float focal=10;

    public void Start()
    {
        cam = Camera.main;
        volume = GetComponent<Volume>();
        volume.profile.TryGet<DepthOfField>(out depthOfField);
    }

    public void Update()
    {
        depthOfField.focusDistance.value = Mathf.Pow(-1 / cam.transform.position.z , 1/focal) + min;
    }
}