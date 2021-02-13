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

    // Update is called once per frame
    void Update()
    {
        if(MouseController.SelectedObject != null)
        {
            Mex m = MouseController.SelectedObject.GetComponent<Mex>();
            UpdateSelectedMexStats(m);
        }
        else
            ClearUI();
    }

    public void UpdateSelectedMexStats(Mex mex){
        MexName.text = string.Format("[{0}]", "Test Mex");
        MexSpeed.text = string.Format("Speed: {0}", mex.Unit.Speed);
        MexArmor.text = string.Format("Armor: {0}", mex.Unit.Armor);
        MexInternal.text = string.Format("Internal: {0}", mex.Unit.Internal);
    }

    private void ClearUI(){
        MexName.text = string.Empty;
        MexSpeed.text = string.Empty;
        MexArmor.text = string.Empty;
        MexInternal.text = string.Empty;
    }
}
