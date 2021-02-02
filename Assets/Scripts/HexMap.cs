using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HexMap is responsible for building the tactical map. This includes size and shape, and handling the terrain.

public class HexMap : MonoBehaviour
{
    public int NumCols = 12;
    public int NumRows = 8;
    public GameObject HexPrefab;

    //TODO: move these out to its class/array
    public Material MatLvl0;
    public Material MatLvl1;
    public Material MatLvl2;

    public Mesh MeshLvl0;
    public Mesh MeshLvl1;
    public Mesh MeshLvl2;


    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
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
                
                //add perlin noise vir elevation                
                Vector2 noiseOffset = new Vector2( Random.Range(0f, 1f), Random.Range(0f, 1f) );
                float noiseSample = Mathf.PerlinNoise(q + noiseOffset.x, r + noiseOffset.y);

                //bounds the perlin noise to 0 - 1
                if(noiseSample > 1)
                    noiseSample = 1;
                if(noiseSample < 0)
                    noiseSample = 0;

                //assign elevation to hex
                hex.Elevation += noiseSample;

                // Debug.Log(string.Format("{0},{1} E:{2}", q, r, hex.Elevation));

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

            //set terrain material according to height
            //TODO: Update this to it's own class - Probably part of the BIOME class?
            if(hex.Elevation >= 0.7f){
                mr.material = MatLvl2;
                mf.mesh = MeshLvl2;
            }                
            else if(hex.Elevation >= 0.55f && hex.Elevation < 0.7f){
                mr.material = MatLvl1;
                mf.mesh = MeshLvl1;
            }
            else{
                mr.material = MatLvl0;
                mf.mesh = MeshLvl0;
            }
        }
    }
}
    