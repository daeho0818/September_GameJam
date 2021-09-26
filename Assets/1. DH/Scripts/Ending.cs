using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public Transform logo;
    float value = 0;
    bool up = true;
    void Start()
    {

    }

    void Update()
    {
        value += up ? 1 : -1;

        if (value > 45) up = false;
        else if (value < -45) up = true;

        logo.transform.Translate(new Vector2(0, Mathf.Sin(value / 50f * Mathf.PI / 180)));
    }
}
