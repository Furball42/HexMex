using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //     if(MouseController.HoveredObject != null)
    //     {
    //         Mex m = MouseController.SelectedObject.GetComponent<Mex>();
    //         UpdateSelectedMexStats(m);
    //     }
    //     else
    //         ClearUI();
    }
}
