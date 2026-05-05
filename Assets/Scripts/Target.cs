using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public TargetTracker tracker;

    public void Hit()
    {
        tracker.targets--;
        Destroy(gameObject);
    }
}
