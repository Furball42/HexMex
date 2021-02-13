using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    int Speed { get;set; }
    int Armor{ get;set; }
    int Internal { get;set; }
    Hex Hex {get;set;}
    void SetHex(Hex hex);
}
