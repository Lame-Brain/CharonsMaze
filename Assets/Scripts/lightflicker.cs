using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightflicker : MonoBehaviour
{
    private float lightLevel, lightDelta;
    public float lowerLightIntensity, maxLightIntensity, speedOfChange;

    void Start()
    {
        lightLevel = Random.Range(lowerLightIntensity, maxLightIntensity);
        this.GetComponent<Light>().intensity = lightLevel;
        lightDelta = speedOfChange;
    }


    void Update()
    {
        if ((lightLevel + lightDelta) >= maxLightIntensity) lightDelta = speedOfChange * -1;
        if ((lightLevel + lightDelta) <= lowerLightIntensity) lightDelta = speedOfChange;
        lightLevel = lightLevel + lightDelta;
        this.GetComponent<Light>().intensity = lightLevel;
    }
}
