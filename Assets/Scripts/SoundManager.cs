using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager current;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        audioManager = gameObject.GetComponent<AudioManager>();
        onStepSounding += SoundManager_onStepSounding;
    }

    private void SoundManager_onStepSounding(string Foot)
    {
        
        audioManager.Play(Foot +"Step");
    }

    public event Action<string> onStepSounding;

    public void StepSounding(string Foot)
    {
        if(onStepSounding != null)
        {
            onStepSounding(Foot);
        }
    }

}
