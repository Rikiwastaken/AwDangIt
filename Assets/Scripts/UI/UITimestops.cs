using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITimestops : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = gameObject.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int remaining = ModeSwitcher.Instance.switchLimit - ModeSwitcher.Instance.switches;
        _text.text = remaining.ToString();
    }
}
