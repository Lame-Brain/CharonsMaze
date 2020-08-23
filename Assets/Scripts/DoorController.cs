using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject center;
    public float TimeToClose;
    public AudioSource doorSound;
    private float timercount;

    // Update is called once per frame
    void Update()
    {
        if (center.activeSelf ) timercount = 0;
        if(!center.activeSelf && !GameManager.GAME.paused) timercount++;
        if (timercount > TimeToClose)
        {
            center.SetActive(true);
            doorSound.Play();
        }
    }
}
