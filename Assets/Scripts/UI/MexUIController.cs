using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MexUIController : MonoBehaviour
{
    public Text MexName;
    public Text MexSpeed;
    public Text MexArmor;
    public Text MexInternal;
    public MouseController MouseController;
    private Mex SelectedMex;
    public HexMap HexMap;

    //REFACTOR TO PROPERLY HANDLE HTE SELECTEDMEX
    
    void Update()
    {
        if(MouseController.SelectedObject != null)
        {
            SelectedMex = MouseController.SelectedObject.GetComponent<Mex>();
            UpdateSelectedMexStats(SelectedMex);
            HighlightPossibleMovementHexes(SelectedMex);
        }
        else
            ClearUI();
    }

    void UpdateSelectedMexStats(Mex mex){
        MexName.text = string.Format("[{0}]", "Test Mex");
        MexSpeed.text = string.Format("Speed: {0}", mex.Unit.Speed);
        MexArmor.text = string.Format("Armor: {0}", mex.Unit.Armor);
        MexInternal.text = string.Format("Internal: {0}", mex.Unit.Internal);
    }

    void HighlightPossibleMovementHexes(Mex mex){

        Hex[] listOfPossibleMovement = HexMap.GetHexesWithinRangeOf(SelectedMex.Hex, SelectedMex.Unit.Speed);
        foreach(Hex h in listOfPossibleMovement){
            //test
            GameObject hexGO = HexMap.HexToGameObjectMap[h];
            GameObject model = hexGO.transform.Find("HexModel").gameObject;
            MeshRenderer mr = model.GetComponentInChildren<MeshRenderer>();
            MeshFilter mf = model.GetComponentInChildren<MeshFilter>();
            MeshCollider mc = model.GetComponentInChildren<MeshCollider>();
            mr.material.color = Color.red;
        }
    }

    void UnhighlightPossibleMovementHexes(Mex mex){

        Hex[] listOfPossibleMovement = HexMap.GetHexesWithinRangeOf(SelectedMex.Hex, SelectedMex.Unit.Speed);
        foreach(Hex h in listOfPossibleMovement){

            GameObject hexGO = HexMap.HexToGameObjectMap[h];
            HexMap.UpdateVisualsForSingleHex(hexGO);
        }
    }

    void ClearUI(){
        MexName.text = string.Empty;
        MexSpeed.text = string.Empty;
        MexArmor.text = string.Empty;
        MexInternal.text = string.Empty;
        UnhighlightPossibleMovementHexes(SelectedMex);
        SelectedMex = null;
    }
}
