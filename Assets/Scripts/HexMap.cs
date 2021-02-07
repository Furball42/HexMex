using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HexMap is responsible for building the tactical map. This includes size and shape, and handling the terrain.

public class HexMap : MonoBehaviour
{
    public int NumCols = 12;
    public int NumRows = 8;
    public GameObject HexPrefab;
    public Mesh MeshLvl0;
    public Mesh MeshLvl1;
    public Mesh MeshLvl2;

    public enum BiomesEnum { Grassland, Desert, Tundra };

    public BiomesEnum MapBiome;

    private IBiome biome;
    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    void Start()
    {
        SetupBiome();
        GenerateMap();        
    }

    void Update()
    {
    }

    public void SetupBiome()
    {
        switch(MapBiome){
            case BiomesEnum.Desert:
                biome = new DesertBiome();
            break;

            case BiomesEnum.Grassland:
                biome = new GrasslandBiome();
            break;

            case BiomesEnum.Tundra:
                biome = new TundraBiome();
            break;
        }

        biome.SetTerrainMaterials();
    }

    public void GenerateMap() {

        hexes = new Hex[NumCols, NumRows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();
        
        for (int r = 0; r < NumRows; r++)
        {
            //offset to create rectangle from rhombus
            int r_offset = (int)Mathf.Floor(r/2);

            for (int q = -r_offset; q < NumCols - r_offset; q++) {

                Hex hex = new Hex(q, r);
                // hexes[q, r] = hex;
                
                //set initial params
                hex.Elevation = 0f;

                GameObject hexGO = Instantiate(HexPrefab, hex.Position(), Quaternion.identity, this.transform);

                hexToGameObjectMap[hex] = hexGO;
                
                //add perlin noise for elevation                
                Vector2 noiseOffsetElevation = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) );
                float noiseSample = Mathf.PerlinNoise(q + noiseOffsetElevation.x, r + noiseOffsetElevation.y);

                //add perlin noise for moisture
                Vector2 noiseOffsetMoisture = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) );
                float noiseSampleMoisture = Mathf.PerlinNoise(q + noiseOffsetMoisture.x, r + noiseOffsetMoisture.y);

                //bounds the perlin noise to 0 - 1
                //TODO: determine highest / flatten out using MATHF
                if(noiseSample > 1)
                    noiseSample = 1;
                if(noiseSample < 0)
                    noiseSample = 0;

                if(noiseSampleMoisture > 1)
                    noiseSampleMoisture = 1;
                if(noiseSampleMoisture < 0)
                    noiseSampleMoisture = 0;   

                //assign elevation to hex
                hex.Elevation += noiseSample;

                //assign moisture to hex
                hex.Moisture += noiseSampleMoisture;

                //text mesh element
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", q, r);
            }

        }

        UpdateTileVisuals();
    }

    public void UpdateTileVisuals(){

        foreach(KeyValuePair<Hex, GameObject> entry in hexToGameObjectMap)
        {
            GameObject hexGO = entry.Value;
            Hex hex = entry.Key;

            MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
            MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();

            Debug.Log(hex.Moisture);

            if(hex.Elevation >= biome.Level2ElevationThreshold){
                mr.material = (Material)Resources.Load(biome.Materials[2]);
                mf.mesh = MeshLvl2;
            }                
            else if(hex.Elevation >= biome.Level1ElevationThreshold && hex.Elevation < biome.Level2ElevationThreshold){
                mr.material = (Material)Resources.Load(biome.Materials[1]);
                mf.mesh = MeshLvl1;
            }
            else if(hex.Elevation >= biome.Level0ElevationThreshold && hex.Elevation < biome.Level1ElevationThreshold){
                mr.material = (Material)Resources.Load(biome.Materials[0]);
                mf.mesh = MeshLvl0;
            }
            else {
                mr.material = (Material)Resources.Load(biome.Materials[3]);
                mf.mesh = MeshLvl0;
            }
        }
    }
}
    