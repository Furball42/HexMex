using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mex : MonoBehaviour
{
    public GameObject MexPrefab;
    public GameObject HexMap;
    public Pilot pilot;
    public IUnit Unit;
    public Hex Hex;
    public enum MexAction { Idle, Move, Jump, Shoot };
    public MexAction CurrentActionMode;
    public bool HasMoved = false; //mex has two actions - move + whatever. must move BEFORE action, or just action.

    private MouseController mouseController;
    private MexUIController mexUIController;

    // Start is called before the first frame update
    void Start()
    {
        CurrentActionMode = MexAction.Idle;

        Unit = new TextMex();
        mexUIController = GameObject.Find("MexUIController").GetComponent<MexUIController>();
        mouseController = GameObject.Find("MouseController").GetComponent<MouseController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForActionKeyPressed();
    }

    public void CheckForActionKeyPressed(){

        if(mouseController.SelectedObject != null){
            Mex mouseSelectedObj = mouseController.SelectedObject.GetComponent<Mex>();

            if(this == mouseSelectedObj){

                CurrentActionMode = MexAction.Idle;                

                if(Input.GetKeyUp(KeyCode.Alpha1))
                    CurrentActionMode = MexAction.Move;

                if(Input.GetKeyUp(KeyCode.Alpha2))
                    CurrentActionMode = MexAction.Jump;

                if(Input.GetKeyUp(KeyCode.Alpha3))
                    CurrentActionMode = MexAction.Shoot;

                if(CurrentActionMode != MexAction.Idle)
                    HandleActionPressed();

            }
            else
                CurrentActionMode = MexAction.Idle;
        }
    }

    private void HandleActionPressed(){
        if(CurrentActionMode == MexAction.Move){
            Debug.Log(CurrentActionMode);
            mexUIController.HighlightPossibleMovementHexes(this);
        }

        if(CurrentActionMode == MexAction.Jump){
            Debug.Log(CurrentActionMode);
            mexUIController.HighlightPossibleJumpHexes(this);
        }
    }

    public void SetMexActionState(MexAction action){
        CurrentActionMode = action;
    }

    public void SetMexHex(Hex hex){
        Hex = hex;
    }
}
