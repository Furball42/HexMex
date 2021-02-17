using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//HexMap is responsible for building the tactical map. This includes size and shape, and handling the terrain.

public class HexMap : MonoBehaviour
{
    public int NumCols = 12;
    public int NumRows = 8;
    public GameObject HexPrefab;
    public GameObject Board;
    public Mesh MeshLvl0;
    public Mesh MeshLvl1;
    public Mesh MeshLvl2;
    public enum BiomesEnum { Grassland, Desert, Tundra };
    public GameObject[] GrasslandTreeMeshes; //probs gonna change
    public BiomesEnum MapBiome;
    public PlayerController PlayerController; //gotta change this - probably game controller

    private IBiome biome;
    public Hex[,] hexes; //this must be private? //AAAA
    Dictionary<Vector2, Hex> wtfDict = new Dictionary<Vector2, Hex>(); //BBBB
    public Dictionary<Hex, GameObject> HexToGameObjectMap;
    public Dictionary<GameObject, Hex> GameObjectToHexMap;

    void Start()
    {
        SetupBiome();
        GenerateMap();
        PlayerController.GeneratePlayerMex();
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
        HexToGameObjectMap = new Dictionary<Hex, GameObject>();
        GameObjectToHexMap = new Dictionary<GameObject, Hex>();
        
        //this has gotta change
        bool hasAssignedAsStart = false;        

        
        
        for (int r = 0; r < NumRows; r++)
        {
            //offset to create rectangle from rhombus
            int r_offset = (int)Mathf.Floor(r/2);

            for (int q = -r_offset; q < NumCols - r_offset; q++) {

                Hex hex = new Hex(q, r);

                //TODO: fix this shit
                // hexes[q + r_offset, r] = hex;
                wtfDict.Add(new Vector2(q, r), hex);

                //TEMP
                if(hasAssignedAsStart)
                {
                    hex.IsStartingPosition = true;
                    hasAssignedAsStart = true;
                }
                
                //set initial params
                hex.Elevation = 0f;

                GameObject hexGO = Instantiate(HexPrefab, hex.Position(), Quaternion.identity, this.transform);
                HexToGameObjectMap[hex] = hexGO;
                GameObjectToHexMap[hexGO] = hex;;
                
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
                hexGO.GetComponentInChildren<TextMesh>().text = string.Empty;
                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", q, r);
            }

        }

        UpdateTileVisuals();
        AddBoard();
    }

    public Hex GetHexAt(int x, int y)
    {
        // var h = hexes[x,y];
        // return h;

        // Debug.Log("Hexes:" + hexes.Length);

        if(wtfDict == null)
        {
            Debug.LogError("Hexes array not yet instantiated.");
            return null;
        }

        try {

            return wtfDict.Where(p => p.Key == new Vector2(x, y)).First().Value;

            // return hexes[x, y];
        }
        catch
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();

        // var results = []
        // for each -N ≤ x ≤ +N:
        //     for each max(-N, -x-N) ≤ y ≤ min(+N, -x+N):
        //         var z = -x-y
        //         results.append(cube_add(center, Cube(x, y, z)))

        for (int x = -range; x < range; x++)
        {
            //left-down | | right up
            for (int y = Mathf.Max(-range, -x-range); y < Mathf.Min(range + 1, -x+range + 1); y++)
            {
                Debug.Log(centerHex.Q + x +"/" + centerHex.R + y);
                var hex = GetHexAt(centerHex.Q + x, centerHex.R + y);

                if(hex != null)
                    results.Add( hex );

            }
        }

        // for (int dx = -range; dx < range-1; dx++)
        // {
        //     for (int dy = Mathf.Max(-range+1, -dx-range); dy < Mathf.Min(range, -dx+range-1); dy++)
        //     {
        //         Debug.Log(dx + "/" + dy);
        //         var hex = GetHexAt(centerHex.Q + dx, centerHex.R + dy);

        //         if(hex != null)
        //             results.Add( GetHexAt(centerHex.Q + dx, centerHex.R + dy) );
        //     }
        // }

        return results.ToArray();
    }

    public void UpdateTileVisuals(){

        foreach(KeyValuePair<Hex, GameObject> entry in HexToGameObjectMap)
        {
            GameObject hexGO = entry.Value;
            Hex hex = entry.Key;

            MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
            MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
            MeshCollider mc = hexGO.GetComponentInChildren<MeshCollider>(); //to set new colliders for raycast

            if(hex.Elevation >= biome.Level2ElevationThreshold){
                mr.material = (Material)Resources.Load(biome.Materials[2]);
                mf.mesh = MeshLvl2;
                mc.sharedMesh = MeshLvl2;
                hex.Level = 2;
            }                
            else if(hex.Elevation >= biome.Level1ElevationThreshold && hex.Elevation < biome.Level2ElevationThreshold){
                mr.material = (Material)Resources.Load(biome.Materials[1]);
                mf.mesh = MeshLvl1;
                mc.sharedMesh = MeshLvl1;
                hex.Level = 1;
            }
            else if(hex.Elevation >= biome.Level0ElevationThreshold && hex.Elevation < biome.Level1ElevationThreshold){
                mr.material = (Material)Resources.Load(biome.Materials[0]);
                mf.mesh = MeshLvl0;
                mc.sharedMesh = MeshLvl0;
                hex.Level = 0;
            }
            else {
                mr.material = (Material)Resources.Load(biome.Materials[3]);
                mf.mesh = MeshLvl0;
                mc.sharedMesh = MeshLvl0;
                hex.Level = 0;
            }

            DrawTrees(hex, hexGO, mr);
        }
    }

    public void UpdateVisualsForSingleHex(GameObject hexGO){

        Hex hex = GameObjectToHexMap[hexGO];

        MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
        MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
        MeshCollider mc = hexGO.GetComponentInChildren<MeshCollider>(); //to set new colliders for raycast

        if(hex.Elevation >= biome.Level2ElevationThreshold){
            mr.material = (Material)Resources.Load(biome.Materials[2]);
            mf.mesh = MeshLvl2;
            mc.sharedMesh = MeshLvl2;
            hex.Level = 2;
        }                
        else if(hex.Elevation >= biome.Level1ElevationThreshold && hex.Elevation < biome.Level2ElevationThreshold){
            mr.material = (Material)Resources.Load(biome.Materials[1]);
            mf.mesh = MeshLvl1;
            mc.sharedMesh = MeshLvl1;
            hex.Level = 1;
        }
        else if(hex.Elevation >= biome.Level0ElevationThreshold && hex.Elevation < biome.Level1ElevationThreshold){
            mr.material = (Material)Resources.Load(biome.Materials[0]);
            mf.mesh = MeshLvl0;
            mc.sharedMesh = MeshLvl0;
            hex.Level = 0;
        }
        else {
            mr.material = (Material)Resources.Load(biome.Materials[3]);
            mf.mesh = MeshLvl0;
            mc.sharedMesh = MeshLvl0;
            hex.Level = 0;
        }
    }

    private void DrawTrees(Hex hex, GameObject hexGO, MeshRenderer mr){
        //testing for tree location
        Material testMat = (Material)Resources.Load("Materials/Testing/Tree_test");
        if(hex.Moisture >= biome.MoistureThreshold && GrasslandTreeMeshes.Length > 0){
            mr.material = testMat;
            GameObject anchorGO;

            //find first child CHANGE THIS
            anchorGO = hexGO.transform.GetChild(0).gameObject;

            //find true anchor irt Elevation
            Vector3 anchorPosition = hexGO.transform.position;
            if(hex.Level == 1)
                anchorPosition.y += 1;
            if(hex.Level == 2)
                anchorPosition.y += 2;

            //random tree
            GameObject treePrefab = GrasslandTreeMeshes[Random.Range(0, 3)];

            //attachObject to anchor point;
            GameObject newTreeGO = Instantiate(treePrefab, anchorPosition, Quaternion.identity, this.transform);
                                    
            //set new parent to tile
            newTreeGO.transform.SetParent(hexGO.transform);

            //set hex class variable
            hex.HasTree = true;

        }
    }

    private void AddBoard(){
        Vector3 boardPosition = new Vector3(13.45f, -0.5f, 8.45f);
        Instantiate(Board, boardPosition, Quaternion.identity, this.transform);
    }
}
    