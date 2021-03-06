﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBiome : IBiome
{
    public float Level0ElevationThreshold { get; set; } = 0.0f;
    public float Level1ElevationThreshold { get; set; } = 0.55f;
    public float Level2ElevationThreshold { get; set; } = 0.7f;
    public float LevelRiverElevationThreshold { get; set; } = -1.0f;
    public float PrecipitationThreshold { get; set; } = 0.0f;
    public float MoistureThreshold { get; set; } = 1.1f;
    public float FoilageRatio { get; set; } = 0.0f;
    public float ScatterTerrainRatio { get; set; } = 0.0f;
    public string[] Materials { get; set; } = new string[4];

    public void SetTerrainMaterials()
    {
        Materials[0] = "Materials/Testing/Desert_0";
        Materials[1] = "Materials/Testing/Desert_1";
        Materials[2] = "Materials/Testing/Desert_2";
        Materials[3] = "Materials/Testing/River_0";
    }
}