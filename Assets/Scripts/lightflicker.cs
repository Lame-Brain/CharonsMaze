using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightflicker : MonoBehaviour
{
    private float lightLevel, baseLightLevel, time2Change, timer;
    public float LightVariance, minTime, maxTime;

    void Start()
    {
        baseLightLevel = this.GetComponent<Light>().intensity;
        lightLevel = baseLightLevel;
        time2Change = Random.Range(minTime, maxTime);
        timer = 0;
    }


    // Update is called once per frame
    void Update()
    {
        if(timer > time2Change)
        {
            lightLevel = baseLightLevel + Random.Range(-LightVariance, LightVariance);
            timer = 0;
            time2Change = Random.Range(minTime, maxTime);
        }
        timer++;
        if (lightLevel > this.GetComponent<Light>().intensity) this.GetComponent<Light>().intensity += 0.1f;
        if (lightLevel < this.GetComponent<Light>().intensity) this.GetComponent<Light>().intensity -= 0.1f;
    }
}
