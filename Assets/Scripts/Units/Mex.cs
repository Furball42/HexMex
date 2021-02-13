using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mex : MonoBehaviour
{
    public GameObject MexPrefab;
    public GameObject HexMap;
    public Pilot pilot;
    public IUnit Unit;

    // Start is called before the first frame update
    void Start()
    {
        Unit = new TextMex();
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
