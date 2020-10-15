using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSource : MonoBehaviour
{
    [SerializeField] Foot foot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "metal" || other.tag == "normal")
        {
            print(other.tag + foot.ToString());
            SoundManager.current.StepSounding(other.tag + foot.ToString());
        }
    }

    public enum Foot
    {
        Right, Left
    }



}

