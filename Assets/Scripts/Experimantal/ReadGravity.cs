using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReadGravity : MonoBehaviour
{
    public Texture mainTex;
    public Material displayMaterial;

    Color[] pixels;


    void Start()
    {
        mainTex = GetComponent<RawImage>().mainTexture;
    }


    private void Update()
    {
        print( GetPixelAt(new Vector2(Input.mousePosition.x, Input.mousePosition.y)));
    }



    static public Color GetPixelAt(Vector2 position)
    {
        Texture2D newT2D = new Texture2D(1, 1);
        newT2D.ReadPixels(new Rect(position.x, position.y, 1, 1), 0, 0);

        return (newT2D.GetPixel(0,0));
    }




    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}