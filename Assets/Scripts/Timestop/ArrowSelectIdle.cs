using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectIdle : MonoBehaviour
{
    public Vector3 position;
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
