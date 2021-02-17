using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMex : IUnit
{
    public int Speed { get; set; } = 3;
    public int Armor { get; set; } = 3;
    public int Internal { get; set; } = 3;
     //maybe add this to the interace

    // public Hex[] GetHexesInMovementRange(){

    //     Hex[] result;

    //     int lowerLimitSpeed = Speed * -1;

    //     for (int x = lowerLimitSpeed; x < Speed; x++)
    //     {
    //         var yLowerLimit = Mathf.Max(lowerLimitSpeed, x - Speed);
    //         var yUpperLimit = Mathf.Min(Speed, -x + Speed);

    //         for (int y = yLowerLimit; y < yUpperLimit; y++)
    //         {
    //             var z = -x-y;

    //         }
    //     }

    //     // var results = []
    //     // for each -N ≤ x ≤ +N:
    //     //     for each max(-N, -x-N) ≤ y ≤ min(+N, -x+N):
    //     //         var z = -x-y
    //     //             results.append(cube_add(center, Cube(x, y, z)))
    // }
}
