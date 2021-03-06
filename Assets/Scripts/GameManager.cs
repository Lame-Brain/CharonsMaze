﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public static MazeObject MAZE;
    public static GameManager GAME;
    public bool infir, serpt, eclyp, drake, cross, paused;
    public bool MazeFirstTime = true, EnteredGardenFromMaze = false, restartedFromMenu = false, MazeIsDrawn = false;
    public AudioSource footsteps, door;

    void Awake()
    {
        if(GAME == null) { GAME = this; } else if (GAME != this) { Destroy(this); }
        DontDestroyOnLoad(gameObject);
        Debug.Log("INITILIZING GAMEMANAGER (should only be once");
        MAZE = this.GetComponent<MazeObject>();
        
    }

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {        
        //if (paused) GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        //if (!paused) GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;

        if (SceneManager.GetActiveScene().name == "Main Game" && MazeFirstTime)
        {
            MAZE.InitializeMaze();
            MAZE.MakeMaze();
            MazeFirstTime = false;
            MAZE.DrawMaze();
        }
        if(SceneManager.GetActiveScene().name == "Main Game" && !MazeIsDrawn)
        {
            MAZE.DrawMaze();
            MazeIsDrawn = true;
        }

        if (SceneManager.GetActiveScene().name == "Outside" && EnteredGardenFromMaze)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
            GameObject.FindGameObjectWithTag("Player").transform.rotation = GameObject.FindGameObjectWithTag("Respawn").transform.rotation;
            EnteredGardenFromMaze = false;
        }

        if (SceneManager.GetActiveScene().name == "Outside" && restartedFromMenu)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
            GameObject.FindGameObjectWithTag("Player").transform.rotation = GameObject.FindGameObjectWithTag("Spawn").transform.rotation;
            restartedFromMenu = false;
        }

        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Space))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1.75f))
            {
                Debug.Log("I clicked on " + hit.collider.tag);

                if (hit.collider.tag == "Door")
                {
                    hit.collider.gameObject.SetActive(false);
                    if(!door.isPlaying) door.Play(); //play door open sound
                }

                if (hit.collider.tag == "GoldDoor")
                {
                    bool done = false;
                    if(SceneManager.GetActiveScene().name == "Outside" && !done)
                    {
                        SceneManager.LoadScene("Main Game");
                        MazeIsDrawn = false;
                        done = true;
                    }
                    if (SceneManager.GetActiveScene().name == "Main Game" && !done)
                    {
                        SceneManager.LoadScene("Outside");
                        EnteredGardenFromMaze = true;
                        done = true;
                    }
                    if (!door.isPlaying) door.Play();
                }

                if(hit.collider.tag == "Charon")
                {                    
                    PauseGame(GameObject.FindGameObjectWithTag("Player"));
                    GameObject.FindGameObjectWithTag("SceneManager").GetComponent<OutsideSceneManager>().conversationPanel.SetActive(true);
                }
            }
        }
    }

    public void PauseGame(GameObject player)
    {
        player.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void UnpauseGame(GameObject player)
    {
        player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Outside");
        //PauseGame(GameObject.FindGameObjectWithTag("Player"));
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
