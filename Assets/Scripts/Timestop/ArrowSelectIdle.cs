using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectIdle : MonoBehaviour
{
    public static ArrowSelectIdle Instance { get; private set; }
    
    public Vector3 position;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position + new Vector3(0, 5.0f * Mathf.Sin(Time.fixedTime * 3.0f), 0);
        transform.rotation = Quaternion.Euler(0, Time.fixedTime * 60.0f, 0);
    }
}
