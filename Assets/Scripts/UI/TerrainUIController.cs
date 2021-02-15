using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TerrainUIController : MonoBehaviour
{
    public Text TerrainType;
    public Text TerrainElevation;
    public Text TerrainCover;
    public MouseController MouseController;
    public HexMap HexMap;

    // Update is called once per frame
    void Update()
    {
        if(MouseController.HoveredObject != null)
        {
            Hex hex = HexMap.GameObjectToHexMap[MouseController.HoveredObject];
            HighlightHexObject(MouseController.HoveredObject);
            UpdateHoverdHexInfo(hex);
        }
        else
            ClearUI();
    }
    void UpdateHoverdHexInfo(Hex hex){
        TerrainType.text = string.Format("Type: {0}", "Grassland");
        TerrainElevation.text = string.Format("Elevation: {0}", hex.Level);
        
        if(hex.HasTree)
            TerrainCover.text = string.Format("Cover: {0}", "Tree");
        else
            TerrainCover.text = string.Format("Cover: {0}", "None");

    }

    void HighlightHexObject(GameObject hexGO){        

        var model = hexGO.transform.Find("HexModel");
        MeshRenderer mr = model.transform.GetComponent<MeshRenderer>();
        mr.material.color = Color.red;
    }

    void ClearUI(){
        TerrainType.text = string.Empty;
        TerrainElevation.text = string.Empty;
        TerrainCover.text = string.Empty;
    }
}
