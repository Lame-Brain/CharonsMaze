using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsideSceneManager : MonoBehaviour
{
    public GameObject[] tree;
    public GameObject Charon, player;
    private float xCharon, yCharon, zCharon, bobCharon = 0.0005f;
    public GameObject Ferry, conversationPanel, whyMePanel, howManyPanel, whoIsGhostPanel, how2LeavePanel, mazeRegenPanel, payTollPanel, broughtRunesPanel, gameMenu, optionsMenu, confirmMenu;
    public GameObject mazeRegenBtn, broughtRunesBtn, payTollBtn;
    private float xFerry, yFerry, zFerry, bobFerry = 0.001f;
    public bool readyToPayToll = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("INITILIZING OUTSIDESCENEMANAGER (should only be once");
        //Spawn trees
        for (int i = 0; i < 5; i++)
        {
            Instantiate(tree[Random.Range(0,3)], new Vector3(Random.Range(9.5f, 11f),-0.8f, Random.Range(-12.92f, 3.24f)), Quaternion.identity);
            Instantiate(tree[Random.Range(0, 3)], new Vector3(Random.Range(-11f, -10.56f), -0.8f, Random.Range(-12.92f, 3.24f)), Quaternion.identity);
        }
        for (int i = 0; i < 15; i++)
        {
            Instantiate(tree[Random.Range(0, 3)], new Vector3(Random.Range(8.5f, 49.13f), -0.8f, Random.Range(-10.99f, 3.24f)), Quaternion.identity);
            Instantiate(tree[Random.Range(0, 3)], new Vector3(Random.Range(-48.7f, -9.56f), -0.8f, Random.Range(-10.99f, 3.24f)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        xCharon = Charon.transform.position.x; yCharon = Charon.transform.position.y; zCharon = Charon.transform.position.z;
        yCharon += bobCharon;
        if (yCharon + bobCharon > 1.5) bobCharon = -0.0005f;
        if (yCharon + bobCharon < 1.3) bobCharon = 0.0005f;
        Charon.transform.position = new Vector3(xCharon, yCharon + bobCharon, zCharon);

        xFerry = Ferry.transform.position.x; yFerry = Ferry.transform.position.y; zFerry = Ferry.transform.position.z;
        yFerry += bobFerry;
        if (yFerry + bobFerry > 0) bobFerry = -0.001f;
        if (yFerry + bobFerry < -0.34) bobFerry = 0.001f;
        Ferry.transform.position = new Vector3(xFerry+bobFerry, yFerry + bobFerry, zFerry);

        //UI STUFF (Charon Conversation)
        if (GameManager.GAME.MazeFirstTime) mazeRegenBtn.SetActive(false);
        if (!GameManager.GAME.MazeFirstTime) mazeRegenBtn.SetActive(true);
        if (GameManager.GAME.infir && GameManager.GAME.serpt && GameManager.GAME.eclyp && GameManager.GAME.drake) broughtRunesBtn.SetActive(true);
        if (!GameManager.GAME.infir || !GameManager.GAME.serpt || !GameManager.GAME.eclyp || !GameManager.GAME.drake) broughtRunesBtn.SetActive(false);
        payTollBtn.SetActive(readyToPayToll);

        if (Input.GetKeyUp(KeyCode.Escape))
        {            
            if (conversationPanel.activeSelf)
            {                
                if ((!whyMePanel.activeSelf) && (!howManyPanel.activeSelf) && (!whoIsGhostPanel.activeSelf) && (!how2LeavePanel.activeSelf) && (!mazeRegenPanel.activeSelf) && (!payTollPanel.activeSelf) && (!broughtRunesPanel.activeSelf)) { conversationPanel.SetActive(false); UnpauseGame(); }
                whyMePanel.SetActive(false);
                howManyPanel.SetActive(false);
                whoIsGhostPanel.SetActive(false);
                how2LeavePanel.SetActive(false);
                mazeRegenPanel.SetActive(false);
                payTollPanel.SetActive(false);
                broughtRunesPanel.SetActive(false);
            }
            else
            {
                if (optionsMenu.activeSelf || confirmMenu.activeSelf)
                {
                    optionsMenu.SetActive(false);
                    confirmMenu.SetActive(false);
                }
                else
                {
                    if (gameMenu.activeSelf) GameManager.GAME.UnpauseGame(player);
                    if (!gameMenu.activeSelf) GameManager.GAME.PauseGame(player);
                    gameMenu.SetActive(!gameMenu.activeSelf);                    
                }
            }
        }
    }

    public void UnpauseGame()
    {
        GameManager.GAME.UnpauseGame(player);
    }

    public void MazeRegen()
    {
        GameManager.MAZE.MakeMaze();
    }

    public void ReadyToPayToll() { readyToPayToll = true; }
}
