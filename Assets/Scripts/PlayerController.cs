using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HexMapGO;
    public GameObject[] PlayerMex;
    public GameObject PlayerMexContainer;
    public Mex SelectedMex;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //a method to create a Mex for player
    //this will probably be moved
    public void GeneratePlayerMex() {

        HexMap hexMap = HexMapGO.GetComponent<HexMap>();
        
        GameObject firstMex = PlayerMex[0];
        Hex hex = hexMap.HexToGameObjectMap.First().Key;
        GameObject hexGO = hexMap.HexToGameObjectMap.First().Value;
        
        Vector3 anchorPosition = hexGO.transform.position;
            if(hex.Level == 1)
                anchorPosition.y += 1;
            if(hex.Level == 2)
                anchorPosition.y += 2;

        //arbitrary - needs to change
        anchorPosition.y += 1.5f;

        //test gethex
        var startHex = hexMap.GetHexAt(0,4);
        var startHexGO = hexMap.HexToGameObjectMap[startHex];

        GameObject mexGO = Instantiate(firstMex, startHexGO.transform.position, Quaternion.identity, PlayerMexContainer.transform);
        Mex mex = mexGO.GetComponent<Mex>();
        mex.SetMexHex(startHex);
    }
}
