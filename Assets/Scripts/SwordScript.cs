using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SwordScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SteamVR_Action_Boolean SwordAnim;
    bool Out;
    // Start is called before the first frame update
    void Start()
    {
        Out = true;
        SwordAnim.onStateDown += SwordAnim_onStateDown;
        animator = GetComponent<Animator>();
    }

    private void SwordAnim_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(animator.enabled == false)
        {
            animator.enabled = true;
        }
        if (Out)
        {
            animator.Play("Out");
        }
        else
        {
            animator.Play("In");

        }
        Out = !Out;
    }



    // Update is called once per frame
    void Update()
    {
        
    }


}
