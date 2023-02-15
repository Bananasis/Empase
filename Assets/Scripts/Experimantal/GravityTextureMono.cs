using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GravityTextureMono : MonoBehaviour
{
    [SerializeField] private RawImage ri;
    [SerializeField] private RawImage ric;
    [SerializeField] private ComputeShader precalc;
    [SerializeField] private ComputeShader gravity;
    [SerializeField] private MassDot[] massDots;
    [SerializeField] private SpriteRenderer sr;
    [Range(32, 2048)] [SerializeField] private int height = 256;
    [Range(32, 2048)] [SerializeField] private int width = 256;
    [SerializeField] private int randomP = 100;

     private RenderTexture rt;
     [SerializeField] private RenderTexture tt;
    private RenderTexture rtDistPrecalc;
    private ComputeBuffer _massDotBuffer;
    private int gsCount;
    private Vector2[] vels;
    private Vector2[] poss;

    private float max = 0;
    private void Awake()
    {
        gsCount = randomP;
        height -= height % 32;
        width -= width % 32;
        vels = new Vector2[randomP];
        poss = new Vector2[randomP];
        massDots = new MassDot[randomP];

        for (var i = 0; i < massDots.Length; i++)
        {
            massDots[i] = new MassDot
            {
                gravitationalMass = Random.value / 10,
                pos = new Vector2(height * Random.value, width * Random.value)
            };
        }
     
        massDots = massDots.ToList().OrderBy((md) => -md.gravitationalMass).ToArray();
        max = massDots[0].gravitationalMass;

        _massDotBuffer = new ComputeBuffer(gsCount, sizeof(float) * 6);
        rt = new RenderTexture(width, height, 0, GraphicsFormat.R16G16B16A16_SFloat)
        {
            enableRandomWrite = true
        };
        ri.texture = rt;
        rt.Create();
        rtDistPrecalc = new RenderTexture(width * 2 , height * 2 , 0, GraphicsFormat.R16G16B16A16_SFloat)
            {
                enableRandomWrite = true
            };
        ric.texture = rtDistPrecalc;
        rtDistPrecalc.Create();
        // t2d = new Texture2D(width, height, TextureFormat.RGBAFloat,false);
        //Debug.Log(t2d.isReadable);
        // _accelerationBuffer = new ComputeBuffer(gsCount, sizeof(float) * 2);

        precalc.SetInt("width", width * 2 - 1);
        precalc.SetInt("height", height * 2 - 1);
        precalc.SetInt("center_X", width);
        precalc.SetInt("center_Y", height);
        precalc.SetTexture(0, "result", rtDistPrecalc);
        precalc.Dispatch(gravity.FindKernel("CSMain"), width / 16, height / 16, 1);
    }

    private void OnDestroy()
    {
        _massDotBuffer.Release();
        rt.Release();
        rtDistPrecalc.Release();
        
    }


    private void Update()
    {
          var mpb = new MaterialPropertyBlock();
          
         sr.GetPropertyBlock(mpb);
         mpb.SetTexture(DIstortion,rt);
        sr.SetPropertyBlock(mpb);
          Render();
    }

    private bool recalculate;
    private static readonly int DIstortion = Shader.PropertyToID("DIstortion");

    private void FixedUpdate()
    {
        recalculate = true;
        //  poss =<Vector4>();
    }

    void Render()
    {
      
        if (!recalculate)
            return;
        recalculate = false;

        _massDotBuffer.SetData(massDots);
        gravity.SetInt("width", width);
        gravity.SetInt("height", height);
        gravity.SetInt("mass_dot_count", gsCount);
        gravity.SetFloat("time", Time.time);
        //gravity.SetTexture(0, "result", t2d);
        
        gravity.SetTexture(gravity.FindKernel("Clear"), "result", rt);
        gravity.Dispatch(gravity.FindKernel("Clear"), width/32, height/32, 1);
        gravity.SetTexture(gravity.FindKernel("CSMain"), "result", rt);
        gravity.SetTexture(gravity.FindKernel("CSMain"), "lookUp", rtDistPrecalc);
        gravity.SetBuffer(gravity.FindKernel("CSMain"), "mass_dots", _massDotBuffer);
        gravity.Dispatch(gravity.FindKernel("CSMain"), width/32, height/32, gsCount);
    }

    void CreateDots()
    {
        return;
        // massDotMonos = new MassDotMono[massDotMax];
        // massDots = new MassDot[massDotMax];
        // massDotTransforms = new Transform[massDotMax];
        // velocities = new Vector2[massDotMax];
        // accelerations = new Vector2[massDotMax];
        //
        // for (int i = 0; i < massDotMax; i++)
        // {
        //     var mass = 20 * Random.value + 1;
        //     massDotMonos[i] = Instantiate(pref, transform);
        //     massDotMonos[i].transform.localScale = new Vector3(mass, mass);
        //     massDotTransforms[i] = massDotMonos[i].transform;
        //     massDotMonos[i].data = new MassDot
        //     {
        //         pos = new Vector2(Random.value - 0.5f, Random.value - 0.5f) * 100f,
        //         gravitationalMass = mass / 20,
        //         inertialMass = 1,
        //         rotation = 0.1f,
        //         attraction = 1,
        //     };
        //     massDots[i] = massDotMonos[i].data;
        // }
    }


    // void FixedUpdate()
    // {
    //     // var dTime = Time.deltaTime;
    //     // for (var i = 0; i < massDotMonos.Length; i++)
    //     // {
    //     //     velocities[i] += accelerations[i] * dTime * speed;
    //     //     massDots[i].pos += velocities[i] * dTime * speed;
    //     //     massDotTransforms[i].position = massDots[i].pos;
    //     // }
    //     //
    //     // time += Time.deltaTime;
    //     // if (time < timeout) return;
    //     // computeReady = true;
    // }

    //private bool computeReady;

    // private void Update()
    // {
    //     if (!computeReady || bufferLocked) return;
    //     ComputeGravity();
    //     //time = 0;
    //     computeReady = false;
    // }

    // private bool bufferLocked;

    private void ComputeGravity()
    {
        // _massDotBuffer.SetData(massDots);
        // gravity.SetInt("mass_dot_count", massDotMax);
        // gravity.SetFloat("delta_time", time);
        // 
        // gravity.SetBuffer(0, "accelerations", _accelerationBuffer);
        //
        // AsyncGPUReadback.Request(_accelerationBuffer, OnCompleteReadBack);
        // bufferLocked = true;
        // computeReady = false;
    }

    public void OnCompleteReadBack(AsyncGPUReadbackRequest rec)
    {
        // if (rec.hasError) return;
        // _accelerationBuffer.GetData(accelerations);
        // bufferLocked = false;
    }
}