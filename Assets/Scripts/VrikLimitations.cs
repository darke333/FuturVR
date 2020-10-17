using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrikLimitations : MonoBehaviour
{
    public VRIK ik;
    public RotationLimit[] rotationLimits;
    void Start()
    {
        foreach (RotationLimit limit in rotationLimits)
        {
            limit.enabled = false;
        }
        ik.solver.OnPostUpdate += AfterVRIK;
    }
    private void AfterVRIK()
    {
        foreach (RotationLimit limit in rotationLimits)
        {
            limit.Apply();
        }
    }
}
