using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MexUIController : MonoBehaviour
{
    public Text MexName;
    public Text MexSpeed;
    public Text MexArmor;
    public Text MexInternal;
    public MouseController MouseController;    
    public HexMap HexMap;
    public GameObject ActionPanel;
    public GameObject InfoPanel;
    public Mex SelectedMex;

    private LineRenderer lineRenderer;
    private bool showMovementPath = false;
    

    //REFACTOR TO PROPERLY HANDLE HTE SELECTEDMEX
    void Start() {
        ActionPanel.SetActive(false);
        InfoPanel.SetActive(false);
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    
    void Update()
    {
        if(MouseController.SelectedObject != null)
        {
            Mex newMex = MouseController.SelectedObject.GetComponent<Mex>();

            if(newMex != SelectedMex)
                SelectedMex = MouseController.SelectedObject.GetComponent<Mex>();

            UpdateSelectedMexStats(SelectedMex);
            ActionPanel.SetActive(true);
            InfoPanel.SetActive(true);
            
        }
        else
            ClearUI();

        if(MouseController.HoveredObject != null && showMovementPath){
            TraceMexMovementPath();
        }  
    }

    void TraceMexMovementPath() {

        Hex[] path = HexMapHelper.GetLineDrawingHexes(SelectedMex.Hex, HexMap.GameObjectToHexMap[MouseController.HoveredObject]);
        lineRenderer.positionCount = path.Length;

        if(path.Length > SelectedMex.Unit.Speed)
            lineRenderer.positionCount = SelectedMex.Unit.Speed + 1;

        for (int h = 0; h < path.Length; h++){

            GameObject hexGO = HexMap.HexToGameObjectMap[HexMapHelper.GetHexAt(path[h].Q, path[h].R, HexMap.hexes)];

            if(h <= SelectedMex.Unit.Speed) {
                for (int i = 0; i < hexGO.transform.childCount; i++)
                {
                    if(hexGO.transform.GetChild(i).name == "LineAnchor")
                        lineRenderer.SetPosition(h, hexGO.transform.GetChild(i).transform.position);
                }
            }            
        }
    }

    void UpdateSelectedMexStats(Mex mex){
        MexName.text = string.Format("[{0}]", "Test Mex");
        MexSpeed.text = string.Format("Speed: {0}", mex.Unit.Speed);
        MexArmor.text = string.Format("Armor: {0}", mex.Unit.Armor);
        MexInternal.text = string.Format("Internal: {0}", mex.Unit.Internal);
    }

    public void HighlightPossibleMovementHexes(Mex mex){

        UnhighlightPossibleMovementHexes(mex);

        showMovementPath = true;

        if(mex != null){
            Hex[] listOfPossibleMovement = HexMapHelper.GetHexesWithinRangeOf(mex.Hex, mex.Unit.Speed, HexMap.hexes); //HexMap.GetHexesWithinRangeOf(mex.Hex, mex.Unit.Speed);

            foreach(Hex h in listOfPossibleMovement){

                if(h.Level == 0)
                {
                    HighlightHex(h);
                }
            }
        }        
    }

    public void HighlightPossibleJumpHexes(Mex mex){

        UnhighlightPossibleMovementHexes(mex);

        if(mex != null){
            Hex[] listOfPossibleMovement = HexMapHelper.GetHexesWithinRangeOf(mex.Hex, mex.Unit.Speed, HexMap.hexes); //HexMap.GetHexesWithinRangeOf(mex.Hex, mex.Unit.Speed);

            foreach(Hex h in listOfPossibleMovement){

                if(mex.Hex.Level == 0){
                    //should look for lvl 1
                    if(h.Level == 1)
                    {
                        HighlightHex(h);
                    }
                }
                else if (mex.Hex.Level == 1){
                    HighlightHex(h);
                }
                else if (mex.Hex.Level == 2){
                    if(h.Level == 1){
                        HighlightHex(h);
                    }
                }                                
            }
        }        
    }

    void HighlightHex(Hex h){
        GameObject hexGO = HexMap.HexToGameObjectMap[h];
        GameObject model = hexGO.transform.Find("HexModel").gameObject;
        MeshRenderer mr = model.GetComponentInChildren<MeshRenderer>();
        MeshFilter mf = model.GetComponentInChildren<MeshFilter>();
        MeshCollider mc = model.GetComponentInChildren<MeshCollider>();
        mr.material.color = Color.red;
    }

    public void UnhighlightPossibleMovementHexes(Mex mex){

        if(mex != null){
            Hex[] listOfPossibleMovement = HexMapHelper.GetHexesWithinRangeOf(mex.Hex, mex.Unit.Speed, HexMap.hexes); //HexMap.GetHexesWithinRangeOf(mex.Hex, mex.Unit.Speed);
            foreach(Hex h in listOfPossibleMovement){

                GameObject hexGO = HexMap.HexToGameObjectMap[h];
                HexMap.UpdateVisualsForSingleHex(hexGO);
            }

            showMovementPath = false;
        }        
    }

    public void ClearUI(){
        MexName.text = string.Empty;
        MexSpeed.text = string.Empty;
        MexArmor.text = string.Empty;
        MexInternal.text = string.Empty;
        UnhighlightPossibleMovementHexes(SelectedMex);
        SelectedMex = null;
        ActionPanel.SetActive(false);
        InfoPanel.SetActive(false);
        lineRenderer.positionCount = 0;
    }
}
