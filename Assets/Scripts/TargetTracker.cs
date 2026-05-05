using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    private MaterialModificator _materialModificator;
    
    public int targets = 0;
    private bool _corrupted = false;

    private void Start()
    {
        _materialModificator = gameObject.GetComponent<MaterialModificator>();
    }

    private void Update()
    {
        if (!_corrupted && targets > 0)
        {
            StartCoroutine(_materialModificator.SetCorruptionToValue(1.0f));
            _corrupted = true;
        }
        if (_corrupted && targets == 0)
        {
            StartCoroutine(_materialModificator.SetCorruptionToValue(0.0f));
            _corrupted = false;
        }
    }
}
