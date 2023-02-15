using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasGravity : MonoBehaviour
{


    GravityArray array; 
    Rigidbody2D body;
    
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        array = FindObjectOfType<GravityArray>();
    }


    void FixedUpdate()
    {
        // relative position

        Vector2 worldPos = array.cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, 0));
        Vector2 pos = new Vector2(worldPos.x / array.cam.pixelWidth * array.rendTex.width, worldPos.y / array.cam.pixelWidth * array.rendTex.height);


        // if inside the array:
        if (pos.x - 1 > 0 && pos.x + 1 < array.rendTex.width && pos.y - 1 > 0 && pos.y + 1 < array.rendTex.height)
        {

            // force at center
            Vector2 originForce = array.gravityField[(int)pos.x, (int)pos.y];

            //  accumulate force into this value
            Vector2 forceSum = Vector2.zero;


            // Loop through a 3x3 grid around position
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {

                    Vector2 gridForce = array.gravityField[(int)pos.x - 1 + x, (int)pos.y - 1 + y];


                    forceSum = forceSum + Vector2.one * new Vector2(x - 1, y - 1) * (gridForce - originForce);

                }
            }

            Vector2 force = forceSum * originForce;

            body.AddForce(force);
        }

        

    }

}

