using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;


public class DistortionManager : MonoBehaviour
{
    [SerializeField] private RawImage ri;
    [SerializeField] private ComputeShader precalc;
    [SerializeField] private ComputeShader gravity;
    [SerializeField] private MassDot[] massDots;
    [Range(32, 2048)] [SerializeField] private int height = 256;
    [Range(32, 2048)] [SerializeField] private int width = 256;
    [Inject] private IGravityManager _gravityManager;
    private RenderTexture rt;
    private RenderTexture rtDistPrecalc;
    private ComputeBuffer _massDotBuffer;
    private int MaxGSources = 10;
    private bool recalculate;
    private static readonly int Distortion = Shader.PropertyToID("Distortion");

    private void Awake()
    {
        height -= height % 32;
        width -= width % 32;
        massDots = new MassDot[MaxGSources];

        for (var i = 0; i < massDots.Length; i++)
        {
            massDots[i] = new MassDot
            {
                gravitationalMass = 0,
                pos = new Vector2(0, 0)
            };
        }

        _massDotBuffer = new ComputeBuffer(MaxGSources, sizeof(float) * 6);
        rt = new RenderTexture(width, height, 0, GraphicsFormat.R16G16B16A16_SFloat)
        {
            enableRandomWrite = true
        };
        ri.texture = rt;
        rt.Create();
        rtDistPrecalc = new RenderTexture(width * 2, height * 2, 0, GraphicsFormat.R16G16B16A16_SFloat)
        {
            enableRandomWrite = true
        };

        precalc.SetInt("width", width * 2 - 1);
        precalc.SetInt("height", height * 2 - 1);
        precalc.SetInt("center_X", width);
        precalc.SetInt("center_Y", height);
        precalc.SetTexture(0, "result", rtDistPrecalc);
        precalc.Dispatch(gravity.FindKernel("CSMain"), width / 16, height / 16, 1);
        var position = Camera.main.transform.position;
        trc = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height) - position.z * Vector3.forward);
        blc = Camera.main.ScreenToWorldPoint(-position.z * Vector3.forward);
    }

    private void OnDestroy()
    {
        _massDotBuffer.Release();
        rt.Release();
        rtDistPrecalc.Release();
    }


    private void Update()
    {
        Render();
    }

    private Vector2 trc;
    private Vector2 blc;
    private static readonly int Trc = Shader.PropertyToID("TRC");
    private static readonly int Blc = Shader.PropertyToID("BLC");

    private void FixedUpdate()

    {
        var position = Camera.main.transform.position;
        trc = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height) - position.z * Vector3.forward);
        blc = Camera.main.ScreenToWorldPoint(-position.z * Vector3.forward);
        var size = trc - blc;
        var squaredSize = new Vector2(Mathf.Max(size.x, size.y), Mathf.Max(size.x, size.y));
        var attrs = _gravityManager.reg;

        for (var i = 0; i < massDots.Length && i < attrs.Count; i++)
        {
            var pos = attrs[i].position;
            massDots[i] = new MassDot
            {
                gravitationalMass = attrs[i].mass,
                pos = new Vector2((pos.x - blc.x) * (width / squaredSize.x), (pos.y - blc.y) * (height / squaredSize.y))
            };
        }

        recalculate = true;
    }

    void Render()
    {
        if (!recalculate)
            return;
        recalculate = false;

        _massDotBuffer.SetData(massDots);
        gravity.SetInt("width", width);
        gravity.SetInt("height", height);
        gravity.SetInt("mass_dot_count", MaxGSources);
        gravity.SetFloat("time", Time.time);

        // gravity.SetTexture(gravity.FindKernel("Clear"), "result", rt);
        // gravity.Dispatch(gravity.FindKernel("Clear"), width / 32, height / 32, 1);
        gravity.SetTexture(gravity.FindKernel("CSMain"), "result", rt);
        gravity.SetTexture(gravity.FindKernel("CSMain"), "lookUp", rtDistPrecalc);
        gravity.SetBuffer(gravity.FindKernel("CSMain"), "mass_dots", _massDotBuffer);
        gravity.Dispatch(gravity.FindKernel("CSMain"), width / 32, height / 32, MaxGSources);
    }
}