using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hex is responsible for holding the data regarding a specific hex on the map, including position, neighbours etc.
public class Hex
{
    public readonly int Q;
    public readonly int R;
    public readonly int S;
    public float Elevation;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public Hex(int q, int r){

        Q = q;
        R = r;
        S = -q-r;
    }

    //Return world space position
    public Vector3 Position(){
        
        float radius = 1f;
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float horizontalOffset = width;
        float verticalOffset = height * 0.75f;
        

        return new Vector3(
            horizontalOffset * (Q + R / 2f),
            0,
            verticalOffset * R
        );        
    }
}
