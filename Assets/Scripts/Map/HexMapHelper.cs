using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HexMapHelper 
{
    public static Hex GetHexAt(int x, int y, Dictionary<Vector2, Hex> hexes)
    {
        if(hexes == null)
        {
            Debug.LogError("Hexes array not yet instantiated.");
            return null;
        }

        try {

            return hexes.Where(p => p.Key == new Vector2(x, y)).First().Value;
        }
        catch
        {
            // Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }

    public static int GetDistanceBetweenHexes(Hex a, Hex b){
        return (Mathf.Abs(a.Q - b.Q) 
          + Mathf.Abs(a.Q + a.R - b.Q - b.R)
          + Mathf.Abs(a.R - b.R)) / 2;
    }

    public static Hex[] GetHexesWithinRangeOf(Hex centerHex, int range, Dictionary<Vector2, Hex> hexes)
    {
        List<Hex> results = new List<Hex>();

        for (int x = -range; x < range +1; x++)
        {
            for (int y = Mathf.Max(-range, -x-range); y < Mathf.Min(range + 1, -x+range + 1); y++)
            {
                var hex = GetHexAt(centerHex.Q + x, centerHex.R + y, hexes);

                if(hex != null)
                    results.Add( hex );

            }
        }

        return results.ToArray();
    }

    public static Hex[] GetLineDrawingHexes(Hex centerHex, Hex pointerHex) {

        var N = GetDistanceBetweenHexes(centerHex, pointerHex);
        Hex[] hexPath = new Hex[N + 1];
        
        for (int i = 0; i <= N; i++)
        {
            Cube cubeFromHexA = AxialToCube(centerHex);
            Cube cubeFromHexB = AxialToCube(pointerHex);
            Cube resultCube = CubeRound(CubeLerp(cubeFromHexA, cubeFromHexB, (float)1.0/N * i));
            Hex resultHex = CubeToAxial(resultCube);
            hexPath[i] = resultHex;
        }

        return hexPath;
    }

    //use Mathf.Lerp istead?
    private static float CustomLerp(float a, float b, float t){
        return a + (b - a) * t;
    }

    private static Cube CubeLerp(Cube a, Cube b, float t){
        return new Cube(CustomLerp(a.X, b.X, t), 
                CustomLerp(a.Y, b.Y, t),
                CustomLerp(a.Z, b.Z, t));
    }
    private static Hex HexRound(Hex hex){
        return CubeToAxial(CubeRound(AxialToCube(hex)));
    }

    private static Cube CubeRound(Cube cube){
        float rx = Mathf.Round(cube.X);
        float ry = Mathf.Round(cube.Y);
        float rz = Mathf.Round(cube.Z);

        float x_diff = Mathf.Abs(rx - cube.X);
        float y_diff = Mathf.Abs(ry - cube.Y);
        float z_diff = Mathf.Abs(rz - cube.Z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry-rz;
        else if(y_diff > z_diff)
            ry = -rx-rz;
        else
            rz = -rx-ry;

        return new Cube(rx, ry, rz);
    }

    private static Hex CubeToAxial(Cube cube){
        var q = cube.X;
        var r = cube.Z;
        return new Hex((int)q, (int)r);
    }

    private static Cube AxialToCube(Hex hex){
        int x = hex.Q;
        int z = hex.R;
        int y = -x-z;
        return new Cube(x, y, z);
    }
}
