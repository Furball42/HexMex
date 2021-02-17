using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public GameObject SelectedObject;
    public GameObject HoveredObject;    
    public HexMap HexMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMouseOver();
        CheckForMouseDown();
    }

    private void CheckForMouseOver(){
        int layer_mask = LayerMask.GetMask("Terrain", "Mex");
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layer_mask);
        
        if(hit){

            if(hitInfo.transform.gameObject.layer == 8){
                SetHoveredObject(hitInfo.transform.gameObject);
            }
        }
    }

    private void CheckForMouseDown() {

        if(Input.GetMouseButtonDown(0)){
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            
            if(hit){

                GameObject hitGO = hitInfo.transform.gameObject;

                if (hitGO.tag == "Mex")
                {
                    Mex mex = hitInfo.transform.gameObject.GetComponent<Mex>();
                    SelectObject(hitGO);
                }
                else
                    ClearSelectedObject();
            }
        } 
    }

    private void SetHoveredObject(GameObject hoveredGO){
        
        // if(HoveredObject != null)
        //     HoveredObject = null;

        if(hoveredGO.tag == "Terrain"){                        

            if (hoveredGO != HoveredObject && HoveredObject != null)
                HexMap.UpdateVisualsForSingleHex(HoveredObject);

            HoveredObject = hoveredGO;
        }            
    }

    private void SelectObject(GameObject hitGO){
        if(SelectedObject != null){
            
            if(hitGO == SelectedObject)
                return;

            ClearSelectedObject();
        }

        SelectedObject = hitGO;
        Renderer[] rs = SelectedObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rs){
            Material m = r.material;
            m.color = Color.green;
            r.material = m;
        }
    }

    private void ClearSelectedObject(){

        if(SelectedObject == null){            
            return;
        }

        Renderer[] rs = SelectedObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rs){
            Material m = r.material;
            m.color = Color.white;
            r.material = m;
        }

        SelectedObject = null;
    }
}
