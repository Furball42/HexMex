using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMex : IUnit
{
    public int Speed { get; set; } = 3;
    public int Armor { get; set; } = 3;
    public int Internal { get; set; } = 3;
    public Hex Hex { get;set; } //maybe add this to the interace

    public void SetHex(Hex hex){
        Hex = hex;
    }
}
