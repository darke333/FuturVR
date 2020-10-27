using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Slot : MonoBehaviour
{
    float Distance;
    [SerializeField] SteamVR_Action_Boolean ToggleGripButton;
    Hand LeftHand;
    Hand RightHand;



    // Start is called before the first frame update
    void Start()
    {
       // if ()
       //     Valve.VR.InteractionSystem.Player.instance.rightHand.;
       // ToggleGripButton.onStateDown += ToggleGripButton_onStateDown;
    }

    private void ToggleGripButton_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        GameObject ConnectedObj;
        //if ()
        //{
//
        ///
    }




    // Update is called once per frame
    void Update()
    {
        Distance = GetComponent<SphereCollider>().radius;
    }
}
