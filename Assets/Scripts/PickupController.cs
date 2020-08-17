using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private GameObject go;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        go = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        go.transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.Self);
    }

    public void OnTriggerEnter(Collider other)
    {
        /* 1. determine what object this script is attached to
           2. add counter to inventory
           3. destroy gameobject
           public bool infir, serpt, eclyp, drake, cross;*/
        Debug.Log(other.tag + " picked up " + this.tag);
        if (this.tag == "INFIR_Rune") { GameManager.GAME.infir = true; }
        if (this.tag == "SERPT_Rune") { GameManager.GAME.serpt = true; }
        if (this.tag == "ECLYP_Rune") { GameManager.GAME.eclyp = true; }
        if (this.tag == "DRAKE_Rune") { GameManager.GAME.drake = true; }
        if (this.tag == "Cross") { GameManager.GAME.cross = true; }
        Destroy(this.gameObject);
        //Play pickup sound
    }
}
