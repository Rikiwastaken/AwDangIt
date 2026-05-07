using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSoundDriver : MonoBehaviour
{
    public SFXDriver stepDriver;
    
    public void PlayStep()
    {
        stepDriver.PlayRandomSound();
    }
}
