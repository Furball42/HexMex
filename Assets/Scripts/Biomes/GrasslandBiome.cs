using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasslandBiome : IBiome
{
    public float Level0ElevationThreshold { get; set; } = 0.2f;
    public float Level1ElevationThreshold { get; set; } = 0.6f;
    public float Level2ElevationThreshold { get; set; } = 0.8f;
    public float LevelRiverElevationThreshold { get; set; } = 0.00f;
    public float PrecipitationThreshold { get; set; } = 0.0f;
    public float MoistureThreshold { get; set; } = 0.65f;
    public float FoilageRatio { get; set; } = 0.0f;
    public float ScatterTerrainRatio { get; set; } = 0.0f;
    public string[] Materials { get; set; } = new string[4];

    public void SetTerrainMaterials()
    {
        Materials[0] = "Materials/Testing/Grassland_0";
        Materials[1] = "Materials/Testing/Grassland_1";
        Materials[2] = "Materials/Testing/Grassland_2";
        Materials[3] = "Materials/Testing/River_0";
    }
}
