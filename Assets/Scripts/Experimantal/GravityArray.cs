using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArray : MonoBehaviour
{

    public RenderTexture rendTex;
    public Vector2[,] gravityField;
    public Camera cam;

    public float force = 10;



    // Start is called before the first frame update
    void Start()
    {
        cam = this.GetComponent<Camera>();
        rendTex.width = 240;
        rendTex.height = 240;
        gravityField = new Vector2[rendTex.width, rendTex.height];
       
    }

    //  <<<  UPDATE  >>>  
    void Update()
    {

        // Read the screen to the gravity Array
        StartCoroutine(GetArray());

    }

    public IEnumerator GetArray()
    {

        // Call at end of frame. Replace this way with a manual call.
        yield return new WaitForEndOfFrame();

        RenderTexture.active = rendTex;

        Texture2D tex = new Texture2D(rendTex.width, rendTex.height, TextureFormat.RGBA32, false);

        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        Color[] colorRow = tex.GetPixels();

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                Color color = colorRow[tex.width * y + x];
                gravityField[x, y] = new Vector2(color.r , color.g ) * force;
            }
        }
        RenderTexture.active = null;

        yield return null;
    }

}
