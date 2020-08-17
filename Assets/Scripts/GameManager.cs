using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static MazeObject MAZE;
    public static GameManager GAME;
    public bool infir, serpt, eclyp, drake, cross;

    void Awake()
    {
        GAME = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MAZE = this.GetComponent<MazeObject>();
        MAZE.MakeMaze();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1.75f))
            {
                Debug.Log("I am near a " + hit.collider.tag);
                if(hit.collider.tag == "Door")
                {
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
}
