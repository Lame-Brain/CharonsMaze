using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Application.Quit();
    }

    public void PayToll()
    {
        SceneManager.LoadScene("End-Won");
    }

    public void DestroyRunes()
    {
        SceneManager.LoadScene("End-Lost");
    }

    public void RestartGame()
    {
        GameManager.GAME.infir = false;
        GameManager.GAME.serpt = false;
        GameManager.GAME.eclyp = false;
        GameManager.GAME.drake = false;
        GameManager.GAME.paused = false;
        GameManager.GAME.EnteredGardenFromMaze = true;
        SceneManager.LoadScene("Outside");
    }
}
