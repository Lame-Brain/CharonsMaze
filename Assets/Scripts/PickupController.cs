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
        //Debug.Log(other.tag + " picked up " + this.tag);
        GameManager.GAME.PauseGame(GameObject.FindGameObjectWithTag("Player"));
        GameObject go = GameObject.FindGameObjectWithTag("Conversation");
        if (this.tag == "INFIR_Rune" && !GameManager.GAME.infir)
        {
            GameManager.GAME.infir = true;            
            go.GetComponent<MazeUIController>().infirRunePanel.SetActive(true);
            go.GetComponent<MazeUIController>().infirStory.text = go.GetComponent<MazeUIController>().getStory(0);
        }
        if (this.tag == "SERPT_Rune" && !GameManager.GAME.serpt)
        {
            GameManager.GAME.serpt = true;
            go.GetComponent<MazeUIController>().serptRunePanel.SetActive(true);
            go.GetComponent<MazeUIController>().serptStory.text = go.GetComponent<MazeUIController>().getStory(0);
        }
        if (this.tag == "ECLYP_Rune" && !GameManager.GAME.eclyp)
        {
            GameManager.GAME.eclyp = true;
            go.GetComponent<MazeUIController>().eclypRunePanel.SetActive(true);
            go.GetComponent<MazeUIController>().eclypStory.text = go.GetComponent<MazeUIController>().getStory(0);
        }
        if (this.tag == "DRAKE_Rune" && !GameManager.GAME.drake)
        {
            GameManager.GAME.drake = true;
            go.GetComponent<MazeUIController>().drakeRunePanel.SetActive(true);
            go.GetComponent<MazeUIController>().drakeStory.text = go.GetComponent<MazeUIController>().getStory(0);
        }
        if (this.tag == "Cross" && !GameManager.GAME.cross)
        {
            GameManager.GAME.cross = true;
            go.GetComponent<MazeUIController>().crossPanel.SetActive(true);
            go.GetComponent<MazeUIController>().crossStory.text = go.GetComponent<MazeUIController>().getStory(5);
        }
        Destroy(this.gameObject);
        //Play pickup sound
    }
}
