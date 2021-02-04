using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBiome{
    float Level0ElevationThreshold { get; set; }
    float Level1ElevationThreshold { get; set; }
    float Level2ElevationThreshold { get; set; }
    float LevelRiverElevationThreshold { get; set; }
    float PrecipitationThreshold { get; set; }
    float FoilageRatio { get; set; }
    float ScatterTerrainRatio { get; set; }
    string[] Materials { get; set; }

    void SetTerrainMaterials(); //TODO: figure out how to do a constructor
}
